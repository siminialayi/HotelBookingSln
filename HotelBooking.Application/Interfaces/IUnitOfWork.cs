using HotelBooking.Application.Interfaces.RepositoryInterfaces;

namespace HotelBooking.Application.Interfaces;

public interface IUnitOfWork
    {
    IBookingRepository Bookings { get; }
    IRoomRepository Rooms { get; }
    IHotelRepository Hotels { get; }

    // Centralized saving
    Task<int> CompleteAsync();
    }