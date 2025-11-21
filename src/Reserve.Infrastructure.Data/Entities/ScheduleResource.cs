using System;

namespace Reserve.Infrastructure.Data.Entities;

public class ScheduleResource
{
    public required Guid Id { get; set; }

    public required Guid ScheduleId { get; set; }

    public required Guid ResourceId { get; set; }

    public virtual required Schedule Schedule { get; set; }

    public virtual required Resource Resource { get; set; }
}
