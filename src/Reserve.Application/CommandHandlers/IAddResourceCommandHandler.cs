using Reserve.Application.Commands;

namespace Reserve.Application.CommandHandlers;

public interface IAddResourceCommandHandler
{
    Task<bool> Handle(AddResourceCommand command, CancellationToken token);
}