using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;

namespace Reserve.Application.QueryHandlers;

public interface IHotelSearchQueryHandler
{
    Task<Result<IEnumerable<HotelDto>>> Handle(HotelSearchQuery query, CancellationToken token);
}
