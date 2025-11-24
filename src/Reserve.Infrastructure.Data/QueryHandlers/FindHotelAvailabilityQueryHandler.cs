using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Reserve.Application.Dtos;
using Reserve.Application.Queries;
using Reserve.Application.QueryHandlers;
using Reserve.Application.Results;
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

    public async Task<Result<IEnumerable<RoomDto>>> Handle(FindHotelAvailabilityQuery query, CancellationToken token)
    {
        var availableRooms = await dbContext
            .FindHotelAvailableRooms(
                query.Start,
                query.End,
                query.NumberOfPeople,
                query.HotelId,
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
            );

        return new SuccessResult<IEnumerable<RoomDto>>(roomDtos);
    }
}
