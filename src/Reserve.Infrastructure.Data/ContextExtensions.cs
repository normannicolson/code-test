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
    public static async Task<IQueryable<Room>> FindAvailableRooms(
        this IContext context,
        DateTimeOffset start,
        DateTimeOffset end,
        int numberOfPeople,
        CancellationToken cancellationToken = default)
    {
        // Get rooms that have slots in the time range
        var roomsWithSlots = context.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.RoomType)
            .Join(context.ScheduleRooms, r => r.Id, sr => sr.RoomId, (r, sr) => new { r, sr })
            .Join(context.Schedules, x => x.sr.ScheduleId, s => s.Id, (x, s) => new { x.r, x.sr, s })
            .Join(context.ScheduleSlots, x => x.s.Id, ss => ss.ScheduleId, (x, ss) => new { x.r, ss })
            .Join(context.Slots, x => x.ss.SlotId, slot => slot.Id, (x, slot) => new { x.r, slot })
            .Where(x => x.slot.Start <= end && x.slot.End >= start)
            .Select(x => x.r);

        // Filter out rooms with bookings and filter by capacity
        var availableRooms = roomsWithSlots
            .Where(r => !context.BookingRooms
                .Where(br => br.RoomId == r.Id)
                .Any(br => br.Booking.Start <= end && br.Booking.End >= start))
            .Where(r => r.RoomType == null || r.RoomType.MaxOccupancy >= numberOfPeople)
            .Distinct();

        return availableRooms;
    }

    public static async Task<IQueryable<Room>> FindHotelAvailableRooms(
        this IContext context,
        DateTimeOffset start,
        DateTimeOffset end,
        int numberOfPeople,
        Guid hotelId,
        CancellationToken cancellationToken = default)
    {
        // Get rooms that have slots in the time range
        var roomsWithSlots = context.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.RoomType)
            .Join(context.ScheduleRooms, r => r.Id, sr => sr.RoomId, (r, sr) => new { r, sr })
            .Join(context.Schedules, x => x.sr.ScheduleId, s => s.Id, (x, s) => new { x.r, x.sr, s })
            .Join(context.ScheduleSlots, x => x.s.Id, ss => ss.ScheduleId, (x, ss) => new { x.r, ss })
            .Join(context.Slots, x => x.ss.SlotId, slot => slot.Id, (x, slot) => new { x.r, slot })
            .Where(x => x.slot.Start <= end && x.slot.End >= start)
            .Select(x => x.r);

        // Filter out rooms with bookings and filter by capacity
        var availableRooms = roomsWithSlots
            .Where(r => !context.BookingRooms
                .Where(br => br.RoomId == r.Id)
                .Any(br => br.Booking.Start <= end && br.Booking.End >= start))
            .Where(r => r.RoomType == null || r.RoomType.MaxOccupancy >= numberOfPeople)
            .Where(r => r.HotelId == hotelId)
            .Distinct();

        return availableRooms;
    }

    public static async Task<bool> HasOverlappingBooking(
        this IContext context,
        DateTimeOffset start,
        DateTimeOffset end,
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        // Check if the room has any overlapping bookings
        var hasOverlappingBooking = await context.BookingRooms
            .Where(br => br.RoomId == roomId)
            .AnyAsync(br => br.Booking.Start <= end && br.Booking.End >= start, cancellationToken);
            
        return hasOverlappingBooking;
    }   

    public static IQueryable<Hotel> GetHotels(
        this IContext context)
    {
        var hotels = context.Hotels;
        
        return hotels;
    }


    public static IQueryable<Hotel> FindHotel(
        this IContext context,
        string name, 
        CancellationToken cancellationToken)
    {
        var hotels = context.Hotels
            .Where(i => i.Name.Contains(name));
        
        return hotels;
    }

    public static async Task<Booking> GetBooking(
        this IContext context,
        Guid id, 
        CancellationToken cancellationToken)
    {
        var booking = await context.Bookings
            .FirstAsync(i => i.Id.Equals(id), cancellationToken);
        
        return booking;
    }


            // var bookings = await dbContext.BookingRooms
            // .Where(br => br.Room.HotelId == query.HotelId)
            // .Select(br => br.Booking)
            // .Distinct()
            // .Select(b => new BookingDto
            // {
            //     Id = b.Id,
            //     Name = b.Name
            // })
            // .ToListAsync(token);
}
