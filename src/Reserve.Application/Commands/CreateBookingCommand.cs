using System;
using System.Collections.Generic;

namespace Reserve.Application.Commands;

public sealed class CreateBookingCommand
{
    public string Name { get; }

    public Guid RoomId { get; }

    public DateTimeOffset StartDateTime { get; }

    public DateTimeOffset EndDateTime { get; }

    public CreateBookingCommand(
        string name,
        Guid roomId,
        DateTimeOffset startDateTime,
        DateTimeOffset endDateTime)
    {
        this.Name = name;
        this.RoomId = roomId;
        this.StartDateTime = startDateTime;
        this.EndDateTime = endDateTime;
    }
}
