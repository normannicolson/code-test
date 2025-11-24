using Reserve.Application.CommandHandlers;
using Reserve.Application.Commands;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data.CommandHandlers;

public sealed class CreateBookingCommandHandler : ICreateBookingCommandHandler
{
    private readonly Context context;

    public CreateBookingCommandHandler(Context context)
    {
        this.context = context;
    }

    public async Task<Guid> Handle(CreateBookingCommand command, CancellationToken token)
    {
        var bookingId = new Guid(); 
        return bookingId;
    }
}
