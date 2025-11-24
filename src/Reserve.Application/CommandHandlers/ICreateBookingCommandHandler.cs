using Reserve.Application.Commands;
using Reserve.Application.Results;

namespace Reserve.Application.CommandHandlers;

public interface ICreateBookingCommandHandler
{
    Task<Result<Guid>> Handle(CreateBookingCommand command, CancellationToken token);
}
