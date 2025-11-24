using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IFindHotelAvailabilityQueryHandler
{
    Task<IEnumerable<RoomDto>> Handle(FindHotelAvailabilityQuery query, CancellationToken token);
}