using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reserve.Core;

namespace Reserve.Infrastructure.Data.Entities;

public class Room
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    [StringLength(256)]
    public required DateTimeOffset Start { get; set; }

    public required DateTimeOffset End { get; set; }

    public Guid? HotelId { get; set; }

    public required RoomType RoomType { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual ICollection<ScheduleRoom> ScheduleRooms { get; set; } = new List<ScheduleRoom>();

    public virtual ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
}
