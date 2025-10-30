namespace HotelBooking.Domain.Entities;

public class Hotel
    {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    // Navigation property for EF Core (One-to-many relationship)
    public ICollection<Room> Rooms { get; set; } = [];
    }