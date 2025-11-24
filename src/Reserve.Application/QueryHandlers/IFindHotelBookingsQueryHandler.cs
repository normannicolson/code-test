using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IFindHotelBookingsQueryHandler
{
    Task<IEnumerable<BookingDto>> Handle(FindHotelBookingsQuery query, CancellationToken token);
}
