using Reserve.Application.Commands;

namespace Reserve.Application.CommandHandlers;

public interface IAddRoomCommandHandler
{
    Task<bool> Handle(AddRoomCommand command, CancellationToken token);
}
