using HotelBooking.Application.Interfaces.RepositoryInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class RoomRepository(HotelBookingDbContext dbContext) : RepositoryBase<Room>(dbContext), IRoomRepository
    {

    // Business Functionality: Find available rooms
    public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime checkInDate,
        DateTime checkOutDate,
        int requiredCapacity)
        {
        // Find the IDs of rooms that are already booked (Overlapping logic from BookingRepository)
        var bookedRoomIds = await DbContext.Bookings
            .Where(b =>
                checkInDate < b.CheckOutDate &&
                b.CheckInDate < checkOutDate)
            .Select(b => b.RoomId)
            .Distinct()
            .ToListAsync();

        // Find all rooms that meet capacity requirement AND are not in the booked list.
        var availableRooms = await DbSet.AsNoTracking()
            .Where(r =>
                r.Capacity >= requiredCapacity &&
                !bookedRoomIds.Contains(r.Id))
            .ToListAsync();

        return availableRooms;
        }

    // Query for capacity check
    public async Task<int> GetRoomCapacityAsync(Guid roomId)
        {
        return await DbSet.AsNoTracking()
            .Where(r => r.Id == roomId)
            .Select(r => r.Capacity)
            .FirstOrDefaultAsync();
        }
    }