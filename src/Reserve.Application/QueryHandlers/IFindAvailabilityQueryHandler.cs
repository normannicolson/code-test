using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IFindAvailabilityQueryHandler
{
    Task<IEnumerable<RoomDto>> Handle(FindAvailabilityQuery query, CancellationToken token);
}