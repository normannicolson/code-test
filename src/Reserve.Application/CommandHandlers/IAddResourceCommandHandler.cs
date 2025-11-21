using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IAddResourceCommandHandler
{
    Task<bool> Handle(AddResourceCommand command, CancellationToken token);
}