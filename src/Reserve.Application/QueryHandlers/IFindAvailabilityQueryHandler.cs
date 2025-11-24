using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;

namespace Reserve.Application.QueryHandlers;

public interface IFindAvailabilityQueryHandler
{
    Task<Result<IEnumerable<RoomDto>>> Handle(FindAvailabilityQuery query, CancellationToken token);
}