using System;

namespace Reserve.Application.Queries;

public sealed class FindAvailabilityQuery
{
    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public FindAvailabilityQuery(
        DateTimeOffset start,
        DateTimeOffset end)
    {
        this.Start = start;
        this.End = end;
    }
}