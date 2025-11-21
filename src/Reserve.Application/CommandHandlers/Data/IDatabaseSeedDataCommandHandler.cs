using Reserve.Application.Commands.Data;

namespace Reserve.Application.CommandHandlers.Data;

public interface IDatabaseSeedDataCommandHandler
{
    Task<bool> Handle(DatabaseSeedDataCommand command, CancellationToken token);
}