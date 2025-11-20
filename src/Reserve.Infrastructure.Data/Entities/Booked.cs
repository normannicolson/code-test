using System;

namespace Reserve.Infrastructure.Data.Entities;

public class Booked
{
    public Guid BookingId { get; set; }

    public Guid SlotId { get; set; }

    public Guid ResourceId { get; set; }

    // public virtual Booking Booking { get; set; }

    // public virtual Slot Slot { get; set; }

    // public virtual Resource Resource { get; set; }
}
