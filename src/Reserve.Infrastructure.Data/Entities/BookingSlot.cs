using System;

namespace Reserve.Infrastructure.Data.Entities;

public class BookingSlot
{
    public required Guid Id { get; set; }

    public required Guid BookingId { get; set; }

    public required Guid SlotId { get; set; }

    public virtual required Booking Booking { get; set; }

    public virtual required Slot Slot { get; set; }
}
