using Reserve.Application.Commands.Data;
using Reserve.Application.Results;

namespace Reserve.Application.CommandHandlers.Data;

public interface IDatabaseSeedDataCommandHandler
{
    Task<Result<bool>> Handle(DatabaseSeedDataCommand command, CancellationToken token);
}