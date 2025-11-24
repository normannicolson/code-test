using Reserve.Application.Dtos;
using Reserve.Application.Queries;

namespace Reserve.Application.QueryHandlers;

public interface IBookingGetQueryHandler
{
    Task<BookingDto> Handle(BookingGetQuery query, CancellationToken token);
}