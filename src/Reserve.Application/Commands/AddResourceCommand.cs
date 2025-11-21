using System;

namespace Reserve.Application.Queries;

public sealed class AddResourceCommand
{
    public Guid Id { get; }

    public string Name { get; }

    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public AddResourceCommand(
        Guid id,
        string name,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        this.Id = id;
        this.Name = name;
        this.Start = start;
        this.End = end;
    }
}