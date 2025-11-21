using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands.Data;

namespace Reserve.Infrastructure.Data.Data;

public class DatabaseSeedDataCommandHandler : IDatabaseSeedDataCommandHandler
{
    public Task<bool> Handle(DatabaseSeedDataCommand command, CancellationToken token)
    {
        return Task.FromResult(true);
    }
}