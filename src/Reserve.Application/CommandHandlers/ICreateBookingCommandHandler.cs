using Reserve.Application.Commands;

namespace Reserve.Application.CommandHandlers;

public interface ICreateBookingCommandHandler
{
    Task<Guid> Handle(CreateBookingCommand command, CancellationToken token);
}
