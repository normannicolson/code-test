using Reserve.Application.Commands;
using Reserve.Application.Results;

namespace Reserve.Application.CommandHandlers;

public interface IAddRoomCommandHandler
{
    Task<Result<bool>> Handle(AddRoomCommand command, CancellationToken token);
}
