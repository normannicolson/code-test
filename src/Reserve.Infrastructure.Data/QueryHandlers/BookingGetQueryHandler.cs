using System.Threading.Tasks;
using Reserve.Application.Queries;
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

    public async Task<BookingDto> Handle(BookingGetQuery query, CancellationToken token)
    {
        var booking = await dbContext
            .GetBooking(query.Id, token);

        var model = new BookingDto
        {
            Id = booking.Id,
            Name = booking.Name
        };

        return model;
    }
}