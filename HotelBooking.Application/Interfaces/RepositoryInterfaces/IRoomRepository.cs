using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.RepositoryInterfaces;
public interface IRoomRepository : IRepository<Room>
    {
    // Specific query for "Find available rooms" business functionality.
    Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate, int requiredCapacity);

    // Query for checking capacity before booking
    Task<int> GetRoomCapacityAsync(Guid roomId);
    }