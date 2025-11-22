using System;
using System.Collections.Generic;

namespace Reserve.Core.Entities;

public class Schedule : IEntity
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; } 

    public IList<Room> Rooms { get; set; }

    public IList<Slot> Slots { get; set; }

    public Schedule()
    {
        this.Id = Guid.NewGuid();
        this.Version = 0;
        this.Name = string.Empty;
        this.Start = DateTimeOffset.MinValue;
        this.End = DateTimeOffset.MinValue;
        this.Rooms = [];
        this.Slots = [];
    }

    public Schedule(
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
        this.Rooms = [];
        this.Slots = [];
    }
}