using System;

namespace Reserve.Infrastructure.Data.Entities;

public class ScheduleSlot
{
    public required Guid Id { get; set; }

    public required Guid ScheduleId { get; set; }

    public required Guid SlotId { get; set; }

    public virtual required Schedule Schedule { get; set; }

    public virtual required Slot Slot { get; set; }
}
