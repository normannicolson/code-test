using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IFindAvailabilityQueryHandler
{
    Task<IEnumerable<ResourceDto>> Handle(FindAvailabilityQuery query, CancellationToken token);
}