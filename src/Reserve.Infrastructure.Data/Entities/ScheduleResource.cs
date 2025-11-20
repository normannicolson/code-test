using System;

namespace Reserve.Infrastructure.Data.Entities;

public class ScheduleResource
{
    public Guid Id { get; set; }

    public Guid ScheduleId { get; set; }

    public Guid ResourceId { get; set; }

    public virtual Schedule Schedule { get; set; }

    public virtual Resource Resource { get; set; }
}
