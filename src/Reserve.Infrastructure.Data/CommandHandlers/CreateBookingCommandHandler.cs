using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using Reserve.Application.CommandHandlers;
using Reserve.Application.Commands;
using Reserve.Application.Results;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data.CommandHandlers;

public sealed class CreateBookingCommandHandler : ICreateBookingCommandHandler
{
    private readonly Context context;

    public CreateBookingCommandHandler(Context context)
    {
        this.context = context;
    }

    public async Task<Result<Guid>> Handle(CreateBookingCommand command, CancellationToken token)
    {
        // Check if the room has any overlapping bookings
        var hasOverlappingBooking = await this.context.HasOverlappingBooking(
            command.Start,
            command.End,
            command.RoomId,
            token);

        if (hasOverlappingBooking)
        {
            return new ErrorResult<Guid>("ROOM_UNAVAILABLE", "The room is not available for the requested time period.");
        }

        // Find all slots for the booking period
        var slots = await this.context.FindRoomSlotsForPeriod(
            command.Start,
            command.End,
            command.RoomId,
            token);

        // Create the booking
        var bookingId = Guid.NewGuid();
        var booking = new Booking
        {
            Id = bookingId,
            Name = command.Name,
            Start = command.Start,
            End = command.End
        };
        this.context.Bookings.Add(booking);

        // Create the booking room association
        var bookingRoom = new BookingRoom
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            RoomId = command.RoomId,
            Booking = booking,
            Room = null!
        };
        this.context.BookingRooms.Add(bookingRoom);

        // Create booking slots for each slot in the period
        foreach (var slot in slots)
        {
            var bookingSlot = new BookingSlot
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                SlotId = slot.Id,
                Booking = booking,
                Slot = slot
            };
            this.context.BookingSlots.Add(bookingSlot);
        }

        await this.context.SaveChangesAsync(token);

        return new SuccessResult<Guid>(bookingId);
    }
}
