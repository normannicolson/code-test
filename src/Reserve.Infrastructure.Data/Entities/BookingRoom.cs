using System;

namespace Reserve.Infrastructure.Data.Entities;

public class BookingRoom
{
    public required Guid Id { get; set; }

    public required Guid BookingId { get; set; }

    public required Guid RoomId { get; set; }

    public virtual required Booking Booking { get; set; }

    public virtual required Room Room { get; set; }
}
