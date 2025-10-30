using HotelBooking.Application.Interfaces.RepositoryInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class BookingRepository(HotelBookingDbContext dbContext) : RepositoryBase<Booking>(dbContext), IBookingRepository
    {
    public async Task<Booking?> GetByBookingNumberAsync(string bookingNumber)
        {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber);
        }

    // Business Rule Check: Double Booking / Overlapping
    public async Task<bool> IsRoomDoubleBookedAsync(Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
        var hasOverlap = await DbSet.AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .AnyAsync(b =>
                checkInDate < b.CheckOutDate &&
                b.CheckInDate < checkOutDate);

        return hasOverlap;
        }

    // This is where the EF Core logic resides.
    public async Task<Booking?> GetDetailsByBookingNumberAsync(string bookingNumber)
        {
        // Used Include to fetch the related Room and Hotel data for DTO mapping.
        return await DbSet
            .AsNoTracking()
            .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
            .FirstOrDefaultAsync(b => b.BookingNumber == bookingNumber);
        }
    }

// Two time periods [A, B] and [C, D] overlap if:
// (A < D) AND (C < B)
// A=New CheckIn, B=New CheckOut, C=Existing CheckIn, D=Existing CheckOut