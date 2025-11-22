using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IHotelGetAllQueryHandler
{
    Task<IEnumerable<HotelDto>> Handle(HotelGetAllQuery query, CancellationToken token);
}
