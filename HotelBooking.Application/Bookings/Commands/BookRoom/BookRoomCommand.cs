using MediatR;
using FluentResults;

namespace HotelBooking.Application.Bookings.Commands.BookRoom;

/// <summary>
/// Command for initiating a new room booking. 
/// IRequest<Result<string>> indicates a successful result returns the unique Booking Number (string).
/// </summary>
public record BookRoomCommand : IRequest<Result<string>>
    {
    // These properties map directly from the BookRoomRequest DTO 
    public Guid RoomId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public int NumberOfGuests { get; init; }
    public string GuestName { get; init; } = string.Empty;
    }