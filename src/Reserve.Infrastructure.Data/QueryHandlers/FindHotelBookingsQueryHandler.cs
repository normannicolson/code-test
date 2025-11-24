using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Infrastructure.Data;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class FindHotelBookingsQueryHandler : IFindHotelBookingsQueryHandler
{
    IContext dbContext;

    public FindHotelBookingsQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<BookingDto>> Handle(FindHotelBookingsQuery query, CancellationToken token)
    {
        var bookings = await dbContext.BookingRooms
            .Where(br => br.Room.HotelId == query.HotelId)
            .Select(br => br.Booking)
            .Distinct()
            .Select(b => new BookingDto
            {
                Id = b.Id,
                Name = b.Name
            })
            .ToListAsync(token);

        return bookings;
    }
}
