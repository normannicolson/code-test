using Microsoft.EntityFrameworkCore;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class HotelSearchQueryHandler : IHotelSearchQueryHandler
{
    private readonly IContext dbContext;

    public HotelSearchQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<HotelDto?> Handle(HotelSearchQuery query, CancellationToken token)
    {
        var hotel = await dbContext.Hotels
            .FirstOrDefaultAsync(h => h.Name.Contains(query.Name), token);

        if (hotel == null)
        {
            return null;
        }

        return new HotelDto
        {
            Id = hotel.Id,
            Name = hotel.Name
        };
    }
}
