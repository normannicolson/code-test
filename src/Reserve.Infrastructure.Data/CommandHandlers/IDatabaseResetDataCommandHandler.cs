using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands.Data;

namespace Reserve.Infrastructure.Data.Data;

public class DatabaseResetDataCommandHandler : IDatabaseResetDataCommandHandler
{
    public Task<bool> Handle(DatabaseResetDataCommand command, CancellationToken token)
    {
        return Task.FromResult(true);
    }
}