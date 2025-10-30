using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enumerations;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBookings.Infrastructure.Persistence;

/// <summary>
/// Static class to handle database migrations and seed/reset initial data.
/// </summary>
public static class DatabaseInitializer
{

    public static async Task SeedAsync(HotelBookingDbContext context)
    {
        if (await context.Hotels.AnyAsync())
            {
            return; // Data already seeded
            }

        var grandHyatt = new Hotel
            {
            Id = Guid.NewGuid(),
            Name = "Grand Hyatt Downtown",
            Address = "123 Pepple St, Lagos, NGA"
            };

        var rooms = new List<Room>
    {
        // Two Single Rooms (Capacity 1)
        new Room { Id = Guid.NewGuid(), RoomNumber = "101", Type = RoomType.Single, Capacity = 1, HotelId = grandHyatt.Id },
        new Room { Id = Guid.NewGuid(), RoomNumber = "102", Type = RoomType.Single, Capacity = 1, HotelId = grandHyatt.Id },
        
        // Two Double Rooms (Capacity 2)
        new Room { Id = Guid.NewGuid(), RoomNumber = "201", Type = RoomType.Double, Capacity = 2, HotelId = grandHyatt.Id },
        new Room { Id = Guid.NewGuid(), RoomNumber = "202", Type = RoomType.Double, Capacity = 2, HotelId = grandHyatt.Id },
        
        // Two Deluxe Rooms (Capacity 4)
        new Room { Id = Guid.NewGuid(), RoomNumber = "301", Type = RoomType.Deluxe, Capacity = 4, HotelId = grandHyatt.Id },
        new Room { Id = Guid.NewGuid(), RoomNumber = "302", Type = RoomType.Deluxe, Capacity = 4, HotelId = grandHyatt.Id },
    };

        await context.Hotels.AddAsync(grandHyatt);
        await context.Rooms.AddRangeAsync(rooms);

        await context.SaveChangesAsync();
        }
    

    /// <summary>
    /// Removes all data from the database tables to prepare for clean seeding.
    /// This is strictly for development/testing environments.
    /// </summary>
    public static async Task ResetDatabaseAsync(HotelBookingDbContext context)
    {
        // Remove Bookings first due to foreign key constraints (FK: Booking -> Room)
        context.Bookings.RemoveRange(context.Bookings);

        // Remove Rooms next (FK: Room -> Hotel)
        context.Rooms.RemoveRange(context.Rooms);

        // Remove Hotels last
        context.Hotels.RemoveRange(context.Hotels);
        
        await context.SaveChangesAsync();
        

    }
}