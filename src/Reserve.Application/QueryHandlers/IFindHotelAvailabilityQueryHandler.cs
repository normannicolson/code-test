using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;

namespace Reserve.Application.QueryHandlers;

public interface IFindHotelAvailabilityQueryHandler
{
    Task<Result<IEnumerable<RoomDto>>> Handle(FindHotelAvailabilityQuery query, CancellationToken token);
}