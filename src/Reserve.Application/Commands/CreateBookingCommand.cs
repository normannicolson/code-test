using System;
using System.Collections.Generic;

namespace Reserve.Application.Commands;

public sealed class CreateBookingCommand
{
    public string Name { get; }

    public Guid RoomId { get; }

    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public CreateBookingCommand(
        string name,
        Guid roomId,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        this.Name = name;
        this.RoomId = roomId;
        this.Start = start;
        this.End = end;
    }
}
