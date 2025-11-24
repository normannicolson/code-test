using Reserve.Application.Commands.Data;
using Reserve.Application.Results;

namespace Reserve.Application.CommandHandlers.Data;

public interface IDatabaseResetDataCommandHandler
{
    Task<Result<bool>> Handle(DatabaseResetDataCommand command, CancellationToken token);
}