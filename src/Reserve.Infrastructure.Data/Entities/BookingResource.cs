using System;

namespace Reserve.Infrastructure.Data.Entities;

public class BookingResource
{
    public required Guid Id { get; set; }

    public required Guid BookingId { get; set; }

    public required Guid ResourceId { get; set; }

    public virtual required Booking Booking { get; set; }

    public virtual required Resource Resource { get; set; }
}
