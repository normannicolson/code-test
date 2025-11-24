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

    private IEnumerable<Room>? AvailableRooms { get; set; } = null;

    public ContextShouldContext()
    {
        var databaseName = DateTime.UtcNow.Ticks.ToString();

        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        this.Context = new Context(options);

        this.Context.RoomTypes.Add(
            new RoomType
            {
                Id = Reserve.Core.RoomType.Single,
                DisplayName = "Single",
                MaxOccupancy = 1
            });

        this.Context.RoomTypes.Add(
            new RoomType
            {
                Id = Reserve.Core.RoomType.Double,
                DisplayName = "Double",
                MaxOccupancy = 2
            });

        this.Context.RoomTypes.Add(
            new RoomType
            {
                Id = Reserve.Core.RoomType.Deluxe,
                DisplayName = "Deluxe",
                MaxOccupancy = 4
            });

        this.Context.SaveChanges();
    }

    public ContextShouldContext GivenHotel(string name)
    {
        var hotel = new Reserve.Infrastructure.Data.Entities.Hotel
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        this.Context.Hotels.Add(hotel);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenRoom(string hotelName, string name)
    {
        var roomType = this.Context.RoomTypes.First(i => i.Id == Reserve.Core.RoomType.Double);
        var hotel = this.Context.Hotels.First(i => i.Name.Equals(hotelName));

        var room = new Reserve.Infrastructure.Data.Entities.Room
        {
            Id = Guid.NewGuid(),
            Name = name,
            Start = new DateTime(2021, 01, 01, 0, 0, 0),
            End = new DateTime(2022, 01, 01, 0, 0, 0),
            RoomType = roomType,
            HotelId = hotel.Id,
            Hotel = hotel
        };

        this.Context.Rooms.Add(room);

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

    public ContextShouldContext GivenScheduleRoom(string scheduleName, string roomName, string hotelName)
    {
        var schedule = Context.Schedules.First(i => i.Name.Equals(scheduleName));

        var room = Context.Rooms
            .Include(i => i.Hotel)
            .Where(i => i.Hotel!.Name.Equals(hotelName))
            .First(i => i.Name.Equals(roomName));

        var scheduleRoom = new Entities.ScheduleRoom
        {
            Id = Guid.NewGuid(),
            ScheduleId = schedule!.Id,
            RoomId = room!.Id,
            Schedule = schedule,
            Room = room
        };

        this.Context.ScheduleRooms.Add(scheduleRoom);

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
            ScheduleId = schedule!.Id,
            SlotId = slot!.Id,
            Schedule = schedule,
            Slot = slot            
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
            End = end,
        };

        this.Context.Bookings.Add(booking);

        this.Context.SaveChanges();
        return this;
    }

    public ContextShouldContext GivenBookingRoom(string bookingName, string roomName)
    {
        var booking = Context.Bookings.FirstOrDefault(i => i.Name.Equals(bookingName));
        var room = Context.Rooms.FirstOrDefault(i => i.Name.Equals(roomName));

        var bookingRoom = new Entities.BookingRoom
        {
            Id = Guid.NewGuid(),
            BookingId = booking!.Id,
            RoomId = room!.Id,
            Booking = booking,
            Room = room
        };

        this.Context.BookingRooms.Add(bookingRoom);

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
            BookingId = booking!.Id,
            SlotId = slot!.Id,
            Booking = booking,
            Slot = slot
        };

        this.Context.BookingSlots.Add(bookingSlot);

        this.Context.SaveChanges();
        return this;
    }

    public async Task<ContextShouldContext> WhenFindAvailableRoomIsCalled(DateTimeOffset start, DateTimeOffset end)
    {
        this.AvailableRooms = await this.Context.FindAvailableRooms(start, end, numberOfPeople: 2, CancellationToken.None);

        return this;
    }

    public ContextShouldContext ThenAvailableRoomCount(int count)
    {
        Assert.IsNotNull(this.AvailableRooms);

        this.AvailableRooms.Count()
            .Should().Be(count);

        return this;
    }

    public ContextShouldContext ThenAvailableRoomsContains(string name)
    {
        var expected = this.Context.Rooms
            .First(i => i.Name.Equals(name));

        this.AvailableRooms
            .Should().Contain(expected);

        return this;
    }
}
