using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IHotelSearchQueryHandler
{
    Task<IEnumerable<HotelDto>> Handle(HotelSearchQuery query, CancellationToken token);
}
