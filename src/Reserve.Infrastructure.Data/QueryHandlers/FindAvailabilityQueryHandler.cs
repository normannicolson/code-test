using System.Threading.Tasks;
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

    public FindAvailabilityQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
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
