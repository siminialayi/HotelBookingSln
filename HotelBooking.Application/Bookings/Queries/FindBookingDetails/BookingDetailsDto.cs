using HotelBooking.Domain.Enumerations;

namespace HotelBooking.Application.Bookings.Queries.FindBookingDetails;

/// <summary>
/// DTO for presenting the full details of a successful booking.
/// </summary>
public class BookingDetailsDto
    {
    // The unique, externally facing reference number.
    public string BookingNumber { get; set; } = string.Empty;

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    // Room information associated with the booking.
    public string RoomNumber { get; set; } = string.Empty;
    public RoomType RoomType { get; set; }
    public string HotelName { get; set; } = string.Empty;

    public int NumberOfGuests { get; set; }
    }