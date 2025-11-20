using System;
using System.Collections.Generic;

namespace Reserve.Infrastructure.Data.Entities;

public class Slot
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; } 

    public virtual ICollection<ScheduleSlot> ScheduleSlots { get; set; }

    public virtual ICollection<BookingSlot> BookingSlots { get; set; }
}
