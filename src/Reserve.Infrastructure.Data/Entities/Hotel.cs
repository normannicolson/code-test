using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

public class Hotel
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
