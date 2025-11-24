namespace Reserve.Infrastructure.Data.Entities;

public class RoomType
{
    public required Reserve.Core.RoomType Id { get; set; }

    public required string DisplayName { get; set; }

    public required int MaxOccupancy { get; set; }
}
