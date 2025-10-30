using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.RepositoryInterfaces;

public interface IBookingRepository : IRepository<Booking>
    {
    Task<bool> IsRoomDoubleBookedAsync(Guid roomId, DateTime checkInDate, DateTime checkOutDate);
    Task<Booking?> GetByBookingNumberAsync(string bookingNumber);

    // Contract for the complex, projected read operation
    // Note: I return the raw Booking entity, letting the Application layer (Mapster) handle the DTO projection.
    Task<Booking?> GetDetailsByBookingNumberAsync(string bookingNumber);
    }