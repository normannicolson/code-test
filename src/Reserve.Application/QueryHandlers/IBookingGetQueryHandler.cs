using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;

namespace Reserve.Application.QueryHandlers;

public interface IBookingGetQueryHandler
{
    Task<Result<BookingDto>> Handle(BookingGetQuery query, CancellationToken token);
}