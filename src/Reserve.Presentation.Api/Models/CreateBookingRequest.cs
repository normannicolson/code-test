namespace Reserve.Presentation.Api.Models;

public record CreateBookingRequest(string Name, Guid RoomId, DateTimeOffset From, DateTimeOffset To);
