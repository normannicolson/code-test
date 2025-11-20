using System;

namespace Reserve.Infrastructure.Data.Entities;

public class BookingSlot
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public Guid SlotId { get; set; }

    public virtual Booking Booking { get; set; }

    public virtual Slot Slot { get; set; }
}
