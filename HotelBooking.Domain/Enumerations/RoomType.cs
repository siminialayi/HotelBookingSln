namespace HotelBooking.Domain.Enumerations;

/// <summary>
/// Defines the available room types and their default maximum occupancy.
/// 
/// </summary>
public enum RoomType
    {
    // Single room, max 1 occupant
    Single = 1,

    // Double room, max 2 occupants
    Double = 2,

    // Deluxe room, max 4 occupants 
    Deluxe = 4
    }