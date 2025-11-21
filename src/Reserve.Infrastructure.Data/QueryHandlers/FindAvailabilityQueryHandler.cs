using System.Threading.Tasks;
using Reserve.Application.Queries;
using Reserve.Core.Entities;

namespace Reserve.Application.QueryHandlers;

public sealed class FindAvailabilityQueryHandler
{
    public FindAvailabilityQueryHandler()
    {
    }

    public async Task<ResourceDto> Handle(FindAvailabilityQuery query)
    {
        var model = new ResourceDto
        {
            Id = Guid.NewGuid(),
            Name = "Resource Name"
        };

        return await Task.FromResult(model);
    }
}