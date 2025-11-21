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
        var booking = dbContext.Bookings
            .First(i => i.Id.Equals(Guid.Parse("2b4161ec-c329-4898-8c7e-3f7234ab9ec9")));

        var model = new BookingDto
        {
            Id = booking.Id,
            Name = booking.Name
        };

        return await Task.FromResult(model);
    }
}