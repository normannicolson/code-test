using Microsoft.EntityFrameworkCore;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class HotelGetAllQueryHandler : IHotelGetAllQueryHandler
{
    private readonly IContext dbContext;

    public HotelGetAllQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<HotelDto>> Handle(HotelGetAllQuery query, CancellationToken token)
    {
        var hotels = await dbContext.Hotels.ToListAsync(token);

        return hotels.Select(h =>
        {
            var dto = new HotelDto
            {
                Id = h.Id,
                Name = h.Name
            };
            return dto;
        });
    }
}
