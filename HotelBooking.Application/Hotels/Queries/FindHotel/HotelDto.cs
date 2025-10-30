namespace HotelBooking.Application.Hotels.Queries.FindHotel;

/// <summary>
/// DTO for presenting basic hotel information.
/// </summary>
public class HotelDto
    {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    }