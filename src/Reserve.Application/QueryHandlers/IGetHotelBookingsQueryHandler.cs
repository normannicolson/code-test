using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IGetHotelBookingsQueryHandler
{
    Task<IEnumerable<BookingDto>> Handle(GetHotelBookingsQuery query, CancellationToken token);
}
