using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

public class Booking
{
    public Guid Id { get; set; }

    [StringLength(256)]
    public string Name { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; }

    public virtual BookingResource BookingResource { get; set; }

    public virtual ICollection<BookingSlot> BookingSlots { get; set; }
}
