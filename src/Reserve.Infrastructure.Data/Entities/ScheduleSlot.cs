using System;

namespace Reserve.Infrastructure.Data.Entities;

public class ScheduleSlot
{
    public Guid Id { get; set; }

    public Guid ScheduleId { get; set; }

    public Guid SlotId { get; set; }

    public virtual Schedule Schedule { get; set; }

    public virtual Slot Slot { get; set; }
}
