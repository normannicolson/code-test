using System;
using System.Collections.Generic;

namespace Reserve.Core.Entities;

public class Booking : IEntity
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; } 

    public IList<Resource> Resources { get; set; }

    public IList<Slot> Slots { get; set; }

    public Booking(
        Guid id,
        long version,
        string name, 
        DateTimeOffset start,
        DateTimeOffset end)
    {
        this.Id = id;
        this.Version = version;
        this.Name = name;
        this.Start = start;
        this.End = end;
        this.Resources = [];
        this.Slots = [];
    }
}
