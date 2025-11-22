using System;

namespace Reserve.Core.Entities;

public class Hotel : IEntity
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string Name { get; set; }

    public Hotel(
        Guid id,
        long version,
        string name)
    {
        this.Id = id;
        this.Version = version;
        this.Name = name;
    }
}