namespace HotelBooking.Domain.Entities;

public class Booking
    {
    public Guid Id { get; set; }

    // Business Rule: Booking numbers must be unique.
    public string BookingNumber { get; set; } = null!;

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    // Constraint: A booking must not require guests to change rooms. 
    // Enforced by the single RoomId association.
    public Guid RoomId { get; set; }
    public int NumberOfGuests { get; set; } // Constraint: Capacity must be respected.

    // Navigation property
    public Room Room { get; set; } = null!;
    }