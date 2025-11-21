using Reserve.Application.Commands.Data;

namespace Reserve.Application.CommandHandlers.Data;

public interface IDatabaseResetDataCommandHandler
{
    Task<bool> Handle(DatabaseResetDataCommand command, CancellationToken token);
}