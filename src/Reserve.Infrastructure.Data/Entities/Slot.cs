using System;
using System.Collections.Generic;

namespace Reserve.Infrastructure.Data.Entities;

public class Slot
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required DateTimeOffset Start { get; set; }

    public required DateTimeOffset End { get; set; } 

    public virtual ICollection<ScheduleSlot> ScheduleSlots { get; set; } = new List<ScheduleSlot>();

    public virtual ICollection<BookingSlot> BookingSlots { get; set; } = new List<BookingSlot>();
}
