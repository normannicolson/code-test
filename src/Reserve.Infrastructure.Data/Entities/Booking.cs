using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

public class Booking
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required DateTimeOffset Start { get; set; }

    public required DateTimeOffset End { get; set; }

    public virtual BookingRoom? BookingRoom { get; set; }

    public virtual ICollection<BookingSlot> BookingSlots { get; set; } = new List<BookingSlot>();
}
