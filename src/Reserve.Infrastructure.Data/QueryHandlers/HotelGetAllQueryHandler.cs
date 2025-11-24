using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class HotelGetAllQueryHandler : IHotelGetAllQueryHandler
{
    private readonly IContext dbContext;
    private readonly ILogger<HotelGetAllQueryHandler> logger;

    public HotelGetAllQueryHandler(IContext dbContext, ILogger<HotelGetAllQueryHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
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
