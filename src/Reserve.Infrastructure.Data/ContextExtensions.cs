using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data;

public static class ContextExtensions
{
    public static async Task<IEnumerable<Resource>> FindAvailableResources(
        this IContext context,
        DateTimeOffset start,
        DateTimeOffset end,
        CancellationToken cancellationToken = default)
    {
        // Get resources that have slots in the time range
        var resourcesWithSlots = context.Resources
            .Join(context.ScheduleResources, r => r.Id, sr => sr.ResourceId, (r, sr) => new { r, sr })
            .Join(context.Schedules, x => x.sr.ScheduleId, s => s.Id, (x, s) => new { x.r, x.sr, s })
            .Join(context.ScheduleSlots, x => x.s.Id, ss => ss.ScheduleId, (x, ss) => new { x.r, ss })
            .Join(context.Slots, x => x.ss.SlotId, slot => slot.Id, (x, slot) => new { x.r, slot })
            .Where(x => x.slot.Start <= end && x.slot.End >= start)
            .Select(x => x.r);

        // Filter out resources with bookings
        var availableResources = resourcesWithSlots
            .Where(r => !context.BookingResources
                .Where(br => br.ResourceId == r.Id)
                .Any(br => br.Booking.Start <= end && br.Booking.End >= start))
            .Distinct();

        return availableResources;
    }
}
