using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;

namespace Reserve.Application.QueryHandlers;

public interface IGetHotelBookingsQueryHandler
{
    Task<Result<IEnumerable<BookingDto>>> Handle(GetHotelBookingsQuery query, CancellationToken token);
}
