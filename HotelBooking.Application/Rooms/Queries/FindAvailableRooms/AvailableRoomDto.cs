using HotelBooking.Domain.Enumerations;

namespace HotelBooking.Application.Rooms.Queries.FindAvailableRooms;

/// <summary>
/// DTO for presenting available room details, hiding unnecessary internal data.
/// </summary>
public class AvailableRoomDto
    {
    public Guid RoomId { get; set; }

    // The friendly identifier for the room.
    public string RoomNumber { get; set; } = string.Empty;

    public RoomType Type { get; set; }

    // Crucial information for the user to determine if the room is suitable.
    public int MaxCapacity { get; set; }

    public string HotelName { get; set; } = string.Empty;
    }