using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;

namespace Reserve.Application.QueryHandlers;

public interface IHotelGetAllQueryHandler
{
    Task<Result<IEnumerable<HotelDto>>> Handle(HotelGetAllQuery query, CancellationToken token);
}
