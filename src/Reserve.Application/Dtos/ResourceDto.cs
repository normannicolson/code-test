public class ResourceDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? HotelId { get; set; }

    public string? HotelName { get; set; }
}