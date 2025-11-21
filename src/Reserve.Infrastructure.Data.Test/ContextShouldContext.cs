using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reserve.Infrastructure.Data;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data.Test;

public class ContextShouldContext
{
    private Context Context { get; }

    private IEnumerable<Resource> AvailableResources { get; set; } = null;

    public ContextShouldContext()
    {
        var databaseName = DateTime.UtcNow.Ticks.ToString();

        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        this.Context = new Context(options);
    }

    public ContextShouldContext GivenResource(string name)
    {
        var resource = new Reserve.Infrastructure.Data.Entities.Resource
        {
            Id = Guid.NewGuid(),
            Name = name,
            Start = new DateTime(2021, 01, 01, 0, 0, 0),
            End = new DateTime(2022, 01, 01, 0, 0, 0)
        };

        this.Context.Resources.Add(resource);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenSlot(string name, DateTimeOffset start, DateTimeOffset end)
    {
        var slot = new Reserve.Infrastructure.Data.Entities.Slot
        {
            Id = Guid.NewGuid(),
            Name = name,
            Start = start,
            End = end
        };

        this.Context.Slots.Add(slot);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenSlotDay(string name, int year, int month, int day)
    {
        var start = new DateTime(year, month, day, 15, 0, 0);
        var plus = start.AddDays(1);
        var end = new DateTime(plus.Year, plus.Month, plus.Day, 11, 0, 0);

        return GivenSlot(name, start, end);
    }

    public ContextShouldContext GivenSlotTwoDays(string name, int year, int month, int day)
    {
        var start = new DateTime(year, month, day, 15, 0, 0);
        var plus = start.AddDays(2);
        var end = new DateTime(plus.Year, plus.Month, plus.Day, 11, 0, 0);

        return GivenSlot(name, start, end);
    }

    public ContextShouldContext GivenSlotWeek(string name, int year, int month, int day)
    {
        var start = new DateTime(year, month, day, 15, 0, 0);
        var plus = start.AddDays(7);
        var end = new DateTime(plus.Year, plus.Month, plus.Day, 11, 0, 0);

        return GivenSlot(name, start, end);
    }

    public ContextShouldContext GivenSlotNineHour(string name, int year, int month, int day, int hour)
    {
        var start = new DateTime(year, month, day, hour, 0, 0);
        var plus = start.AddHours(7);
        var end = new DateTime(plus.Year, plus.Month, plus.Day, plus.Hour, 0, 0);

        return GivenSlot(name, start, end);
    }

    public ContextShouldContext GivenSlotHour(string name, int year, int month, int day, int hour)
    {
        var start = new DateTime(year, month, day, hour, 0, 0);
        var plus = start.AddHours(1);
        var end = new DateTime(plus.Year, plus.Month, plus.Day, plus.Hour, 0, 0);

        return GivenSlot(name, start, end);
    }

    public ContextShouldContext GivenSchedule(string name)
    {
        //Given schedule
        var schedule = new Reserve.Infrastructure.Data.Entities.Schedule
        {
            Id = Guid.NewGuid(),
            Name = name,
            Start = new DateTime(2021, 03, 27, 0, 0, 0),
            End = new DateTime(2021, 03, 27, 0, 0, 0)
        };

        this.Context.Schedules.Add(schedule);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenScheduleResource(string scheduleName, string resourceName)
    {
        var schedule = Context.Schedules.FirstOrDefault(i => i.Name.Equals(scheduleName));
        var resource = Context.Resources.FirstOrDefault(i => i.Name.Equals(resourceName));

        var scheduleResource = new Entities.ScheduleResource
        {
            Id = Guid.NewGuid(),
            ScheduleId = schedule.Id,
            ResourceId = resource.Id
        };

        this.Context.ScheduleResources.Add(scheduleResource);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenScheduleSlot(string scheduleName, string slotName)
    {
        var schedule = Context.Schedules.FirstOrDefault(i => i.Name.Equals(scheduleName));
        var slot = Context.Slots.FirstOrDefault(i => i.Name.Equals(slotName));

        var scheduleSlot = new Entities.ScheduleSlot
        {
            Id = Guid.NewGuid(),
            ScheduleId = schedule.Id,
            SlotId = slot.Id
        };

        this.Context.ScheduleSlots.Add(scheduleSlot);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenBooking(string name, DateTime start, DateTime end)
    {
        var booking = new Reserve.Infrastructure.Data.Entities.Booking
        {
            Id = Guid.NewGuid(),
            Name = name,
            Start = start,
            End = end
        };

        this.Context.Bookings.Add(booking);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenBookingResource(string bookingName, string resourceName)
    {
        var booking = Context.Bookings.FirstOrDefault(i => i.Name.Equals(bookingName));
        var resource = Context.Resources.FirstOrDefault(i => i.Name.Equals(resourceName));

        var bookingResource = new Entities.BookingResource
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            ResourceId = resource.Id
        };

        this.Context.BookingResources.Add(bookingResource);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenBookingSlot(string bookingName, string slotName)
    {
        var booking = Context.Bookings.FirstOrDefault(i => i.Name.Equals(bookingName));
        var slot = Context.Slots.FirstOrDefault(i => i.Name.Equals(slotName));

        var bookingSlot = new Entities.BookingSlot
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            SlotId = slot.Id
        };

        this.Context.BookingSlots.Add(bookingSlot);

        this.Context.SaveChanges();
        return this;
    }

    public async Task<ContextShouldContext> WhenFindAvailableResourceIsCalled(DateTimeOffset start, DateTimeOffset end)
    {
        this.AvailableResources = await this.Context.FindAvailableResources(start, end, CancellationToken.None);

        return this;
    }

    public ContextShouldContext ThenAvailableResourceCount(int count)
    {
        this.AvailableResources.Count()
            .Should().Be(count);

        return this;
    }

    public ContextShouldContext ThenAvailableResourcesContains(string name)
    {
        var expected = this.Context.Resources
            .First(i => i.Name.Equals(name));

        this.AvailableResources
            .Should().Contain(expected);

        return this;
    }
}
