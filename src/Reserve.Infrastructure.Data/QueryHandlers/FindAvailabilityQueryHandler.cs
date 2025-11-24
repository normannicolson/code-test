using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;
using Reserve.Core.Entities;
using Reserve.Infrastructure.Data;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class FindAvailabilityQueryHandler : IFindAvailabilityQueryHandler
{
    IContext dbContext;
    ILogger<FindAvailabilityQueryHandler> logger;

    public FindAvailabilityQueryHandler(IContext dbContext, ILogger<FindAvailabilityQueryHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<Result<IEnumerable<RoomDto>>> Handle(FindAvailabilityQuery query, CancellationToken token)
    {
        var availableRooms = await dbContext
            .FindAvailableRooms(
                query.Start,
                query.End,
                query.NumberOfPeople,
                token);

        var roomDtos = availableRooms
            .Select(r =>
                new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    HotelId = r.HotelId,
                    HotelName = r.Hotel.Name
                }
            )
            .ToList();

        return new SuccessResult<IEnumerable<RoomDto>>(roomDtos);
    }
}
