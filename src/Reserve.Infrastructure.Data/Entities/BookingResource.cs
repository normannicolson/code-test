using System;

namespace Reserve.Infrastructure.Data.Entities;

public class BookingResource
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public Guid ResourceId { get; set; }

    public virtual Booking Booking { get; set; }

    public virtual Resource Resource { get; set; }
}
