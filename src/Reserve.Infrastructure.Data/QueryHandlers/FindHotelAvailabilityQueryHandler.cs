using System.Threading.Tasks;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Core.Entities;
using Reserve.Infrastructure.Data;

namespace Reserve.Infrastructure.Data.QueryHandlers;

public sealed class FindHotelAvailabilityQueryHandler : IFindHotelAvailabilityQueryHandler
{
    IContext dbContext;

    public FindHotelAvailabilityQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<RoomDto>> Handle(FindHotelAvailabilityQuery query, CancellationToken token)
    {
        var availableRooms = await dbContext.FindAvailableRooms(
            query.Start,
            query.End,
            query.NumberOfPeople,
            token);

        var roomDtos = availableRooms
            .Select(r =>
            {
                var dto = new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    HotelId = r.HotelId,
                    HotelName = r.Hotel?.Name
                };
                return dto;
            })
            .ToList();

        return await Task.FromResult(roomDtos);
    }
}
