
using FluentValidation;

namespace HotelBooking.Application.Bookings.Commands.BookRoom;

/// <summary>
/// Fluent Validator for the BookRoomCommand. 
/// Validates input data integrity before executing core business logic.
/// </summary>
public class BookRoomCommandValidator : AbstractValidator<BookRoomCommand>
    {
    public BookRoomCommandValidator()
        {
        // General Constraints (Dates, Guests, Room)
        RuleFor(x => x.RoomId)
            .NotEmpty()
            .WithMessage("Room ID is required for booking.");

        RuleFor(x => x.NumberOfGuests)
            .GreaterThan(0)
            .WithMessage("Booking must be for at least one person.");

        // Booking Numbers (Overlapping/Date Check) - Business Rule: Check-out must be strictly after Check-in
        RuleFor(x => x.CheckInDate)
            .LessThan(x => x.CheckOutDate)
            .WithMessage("Check-in date must be before the check-out date.")
            .GreaterThanOrEqualTo(DateTime.Today.Date)
            .WithMessage("Bookings cannot be made for past dates.");

        }
    }