using System;
using System.Collections.Generic;

namespace Reserve.Core.Entities;

public class Slot : IEntity
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; }

    public Slot(
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
    }
}
