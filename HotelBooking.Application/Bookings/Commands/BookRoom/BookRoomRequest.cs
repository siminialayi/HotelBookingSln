namespace HotelBooking.Application.Bookings.Commands.BookRoom;

/// <summary>
/// DTO representing the input payload for creating a new booking.
/// </summary>
public class BookRoomRequest
    {
    // The Room ID the user intends to book.
    public Guid RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    // Must be after CheckInDate. Business Rule: Any booking must not require 
    // guests to change rooms (enforced by a single date range).
    public DateTime CheckOutDate { get; set; }

    // Business Rule: A room cannot be occupied by more people than its capacity.
    // This will be validated against the Room entity's capacity.
    public int NumberOfGuests { get; set; }

    public string GuestName { get; set; } = string.Empty;
    }