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

        var bookingId = Guid.NewGuid();

        return new SuccessResult<Guid>(bookingId);
    }
}
