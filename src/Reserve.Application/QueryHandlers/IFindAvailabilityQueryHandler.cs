using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IFindAvailabilityHandler
{
    Task<IEnumerable<ResourceDto>> Handle(FindAvailabilityQuery query, CancellationToken token);
}