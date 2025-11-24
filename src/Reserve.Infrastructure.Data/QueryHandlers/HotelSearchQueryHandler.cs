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

    public async Task<IEnumerable<HotelDto>> Handle(HotelSearchQuery query, CancellationToken token)
    {
        var hotels = await dbContext.Hotels
            .Where(h => h.Name.Contains(query.Name))
            .Select(r => new HotelDto
            {
                Id = r.Id,
                Name = r.Name
            })
            .ToListAsync(token);

        return hotels;
    }
}
