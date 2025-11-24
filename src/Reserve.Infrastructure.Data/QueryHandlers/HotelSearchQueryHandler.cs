using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class HotelSearchQueryHandler : IHotelSearchQueryHandler
{
    private readonly IContext dbContext;
    private readonly ILogger<HotelSearchQueryHandler> logger;

    public HotelSearchQueryHandler(IContext dbContext, ILogger<HotelSearchQueryHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<Result<IEnumerable<HotelDto>>> Handle(HotelSearchQuery query, CancellationToken token)
    {
        var hotels = await dbContext
            .FindHotel(query.Name, token)
            .Select(r => new HotelDto
            {
                Id = r.Id,
                Name = r.Name
            })
            .ToListAsync(token);

        return new SuccessResult<IEnumerable<HotelDto>>(hotels);
    }
}
