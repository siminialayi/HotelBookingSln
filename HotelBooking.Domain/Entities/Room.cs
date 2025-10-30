using HotelBooking.Domain.Enumerations;

namespace HotelBooking.Domain.Entities;

public class Room
    {
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = null!;
    public RoomType Type { get; set; }

    // We get the max capacity from the Type enum, but explicitly store it
    // for future flexibility if the rule changes per room.
    public int Capacity { get; set; }

    public Guid HotelId { get; set; }

    // Navigation properties
    public Hotel Hotel { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = [];
    }