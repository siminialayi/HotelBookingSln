using FluentResults;
using MediatR;

namespace HotelBooking.Application.Rooms.Queries.FindAvailableRooms;

/// <summary>
/// MediatR Query to find rooms available between two dates for a specific number of guests.
/// </summary>
public record FindAvailableRoomsQuery(
    DateTime CheckInDate,
    DateTime CheckOutDate,
    int NumberOfGuests) : IRequest<Result<IReadOnlyList<AvailableRoomDto>>>;