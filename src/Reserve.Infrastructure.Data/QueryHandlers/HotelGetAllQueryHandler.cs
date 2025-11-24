using Microsoft.EntityFrameworkCore;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class HotelGetAllQueryHandler : IHotelGetAllQueryHandler
{
    private readonly IContext dbContext;

    public HotelGetAllQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<HotelDto>>> Handle(HotelGetAllQuery query, CancellationToken token)
    {
        var hotels = await dbContext
            .GetHotels()
            .Select(h =>
                new HotelDto
                {
                    Id = h.Id,
                    Name = h.Name
                }
            )
            .ToListAsync(token);

        return new SuccessResult<IEnumerable<HotelDto>>(hotels);
    }
}
