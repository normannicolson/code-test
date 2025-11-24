using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;
using Reserve.Infrastructure.Data;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class GetHotelBookingsQueryHandler : IGetHotelBookingsQueryHandler
{
    IContext dbContext;

    public GetHotelBookingsQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<BookingDto>>> Handle(GetHotelBookingsQuery query, CancellationToken token)
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

        return new SuccessResult<IEnumerable<BookingDto>>(bookings);
    }
}
