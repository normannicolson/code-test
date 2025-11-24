using System.Threading.Tasks;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.Results;
using Reserve.Core.Entities;
using Reserve.Infrastructure.Data;

namespace Reserve.Application.QueryHandlers;

public sealed class BookingGetQueryHandler : IBookingGetQueryHandler
{
    IContext dbContext;

    public BookingGetQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<BookingDto>> Handle(BookingGetQuery query, CancellationToken token)
    {
        var booking = await dbContext
            .GetBooking(query.Id, token);

        if (booking == null)
        {
            return new ErrorResult<BookingDto>("BOOKING_NOT_FOUND", $"Booking with ID {query.Id} was not found.");
        }

        var model = new BookingDto
        {
            Id = booking.Id,
            Name = booking.Name
        };

        return new SuccessResult<BookingDto>(model);
    }
}