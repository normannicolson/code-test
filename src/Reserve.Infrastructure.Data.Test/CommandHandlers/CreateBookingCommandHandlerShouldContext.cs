using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Reserve.Application.Commands;
using Reserve.Application.Results;
using Reserve.Infrastructure.Data.CommandHandlers;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data.Test.CommandHandlers;

public class CreateBookingCommandHandlerShouldContext
{
    private Moq.Mock<ILogger<CreateBookingCommandHandler>> loggerMock;

    private Context Context { get; }

    private Result<Guid>? Result { get; set; } = null;

    public CreateBookingCommandHandlerShouldContext()
    {
        this.loggerMock = new Moq.Mock<ILogger<CreateBookingCommandHandler>>();

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

    public CreateBookingCommandHandlerShouldContext GivenHotel(string name)
    {
        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        this.Context.Hotels.Add(hotel);
        this.Context.SaveChanges();
        return this;
    }

    public CreateBookingCommandHandlerShouldContext GivenRoom(string hotelName, string name)
    {
        var roomType = this.Context.RoomTypes.First(i => i.Id == Reserve.Core.RoomType.Double);
        var hotel = this.Context.Hotels.First(i => i.Name.Equals(hotelName));

        var room = new Room
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

    public CreateBookingCommandHandlerShouldContext GivenSlot(string name, DateTimeOffset start, DateTimeOffset end)
    {
        var slot = new Slot
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

    public CreateBookingCommandHandlerShouldContext GivenSchedule(string name)
    {
        var schedule = new Schedule
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

    public CreateBookingCommandHandlerShouldContext GivenScheduleRoom(string scheduleName, string roomName, string hotelName)
    {
        var schedule = Context.Schedules.First(i => i.Name.Equals(scheduleName));

        var room = Context.Rooms
            .Include(i => i.Hotel)
            .Where(i => i.Hotel!.Name.Equals(hotelName))
            .First(i => i.Name.Equals(roomName));

        var scheduleRoom = new ScheduleRoom
        {
            Id = Guid.NewGuid(),
            ScheduleId = schedule.Id,
            RoomId = room.Id,
            Schedule = schedule,
            Room = room
        };

        this.Context.ScheduleRooms.Add(scheduleRoom);
        this.Context.SaveChanges();
        return this;
    }

    public CreateBookingCommandHandlerShouldContext GivenScheduleSlot(string scheduleName, string slotName)
    {
        var schedule = Context.Schedules.First(i => i.Name.Equals(scheduleName));
        var slot = Context.Slots.First(i => i.Name.Equals(slotName));

        var scheduleSlot = new ScheduleSlot
        {
            Id = Guid.NewGuid(),
            ScheduleId = schedule.Id,
            SlotId = slot.Id,
            Schedule = schedule,
            Slot = slot
        };

        this.Context.ScheduleSlots.Add(scheduleSlot);
        this.Context.SaveChanges();
        return this;
    }

    public CreateBookingCommandHandlerShouldContext GivenBooking(string name, DateTime start, DateTime end)
    {
        var booking = new Booking
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

    public CreateBookingCommandHandlerShouldContext GivenBookingRoom(string bookingName, string roomName)
    {
        var booking = Context.Bookings.First(i => i.Name.Equals(bookingName));
        var room = Context.Rooms.First(i => i.Name.Equals(roomName));

        var bookingRoom = new BookingRoom
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            RoomId = room.Id,
            Booking = booking,
            Room = room
        };

        this.Context.BookingRooms.Add(bookingRoom);
        this.Context.SaveChanges();
        return this;
    }

    public CreateBookingCommandHandlerShouldContext GivenBookingSlot(string bookingName, string slotName)
    {
        var booking = Context.Bookings.First(i => i.Name.Equals(bookingName));
        var slot = Context.Slots.First(i => i.Name.Equals(slotName));

        var bookingSlot = new BookingSlot
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            SlotId = slot.Id,
            Booking = booking,
            Slot = slot
        };

        this.Context.BookingSlots.Add(bookingSlot);
        this.Context.SaveChanges();
        return this;
    }

    public async Task<CreateBookingCommandHandlerShouldContext> WhenCreateBookingIsCalled(
        string name,
        string roomName,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        var room = Context.Rooms.First(i => i.Name.Equals(roomName));

        var command = new CreateBookingCommand(name, room.Id, start, end);
        var handler = new CreateBookingCommandHandler(this.Context, this.loggerMock.Object);

        this.Result = await handler.Handle(command, CancellationToken.None);

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenResultIsSuccess()
    {
        Assert.IsNotNull(this.Result);
        this.Result.IsSuccess.Should().BeTrue();

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenResultIsFailure()
    {
        Assert.IsNotNull(this.Result);
        this.Result.IsFailure.Should().BeTrue();

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenErrorCodeIs(string code)
    {
        Assert.IsNotNull(this.Result);
        this.Result.Should().BeOfType<ErrorResult<Guid>>();

        var errorResult = (ErrorResult<Guid>)this.Result;
        errorResult.Code.Should().Be(code);

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingExists(string name)
    {
        var booking = Context.Bookings.FirstOrDefault(i => i.Name.Equals(name));
        booking.Should().NotBeNull();

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingCount(int count)
    {
        Context.Bookings.Count().Should().Be(count);

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingRoomCount(int count)
    {
        Context.BookingRooms.Count().Should().Be(count);

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingSlotCount(int count)
    {
        Context.BookingSlots.Count().Should().Be(count);

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingHasRoom(string bookingName, string roomName)
    {
        var booking = Context.Bookings.First(i => i.Name.Equals(bookingName));
        var room = Context.Rooms.First(i => i.Name.Equals(roomName));

        var bookingRoom = Context.BookingRooms
            .FirstOrDefault(br => br.BookingId == booking.Id && br.RoomId == room.Id);

        bookingRoom.Should().NotBeNull();

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingHasSlot(string bookingName, string slotName)
    {
        var booking = Context.Bookings.First(i => i.Name.Equals(bookingName));
        var slot = Context.Slots.First(i => i.Name.Equals(slotName));

        var bookingSlot = Context.BookingSlots
            .FirstOrDefault(bs => bs.BookingId == booking.Id && bs.SlotId == slot.Id);

        bookingSlot.Should().NotBeNull();

        return this;
    }

    public CreateBookingCommandHandlerShouldContext ThenBookingDoesNotHaveSlot(string bookingName, string slotName)
    {
        var booking = Context.Bookings.First(i => i.Name.Equals(bookingName));
        var slot = Context.Slots.First(i => i.Name.Equals(slotName));

        var bookingSlot = Context.BookingSlots
            .FirstOrDefault(bs => bs.BookingId == booking.Id && bs.SlotId == slot.Id);

        bookingSlot.Should().BeNull();

        return this;
    }
}
