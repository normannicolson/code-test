using System.Threading.Tasks;
using Reserve.Application.Queries;
using Reserve.Core.Entities;
using Reserve.Infrastructure.Data;

namespace Reserve.Application.QueryHandlers;

public sealed class FindAvailabilityQueryHandler : IFindAvailabilityQueryHandler
{
    IContext dbContext;
    
    public FindAvailabilityQueryHandler(IContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IEnumerable<ResourceDto>> Handle(FindAvailabilityQuery query, CancellationToken token)
    {
        var availableResources = await dbContext.FindAvailableResources(query.Start, query.End, token);

        var resourceDtos = availableResources
            .Select(r => new ResourceDto
            {
                Id = r.Id,
                Name = r.Name,
                HotelId = r.HotelId,
                HotelName = r.Hotel?.Name
            })
            .ToList();

        return await Task.FromResult(resourceDtos);
    }
}