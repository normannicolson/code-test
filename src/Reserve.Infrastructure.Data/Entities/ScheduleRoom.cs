using System;

namespace Reserve.Infrastructure.Data.Entities;

public class ScheduleRoom
{
    public required Guid Id { get; set; }

    public required Guid ScheduleId { get; set; }

    public required Guid RoomId { get; set; }

    public virtual required Schedule Schedule { get; set; }

    public virtual required Room Room { get; set; }
}
