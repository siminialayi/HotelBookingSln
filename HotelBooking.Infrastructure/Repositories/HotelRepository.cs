using HotelBooking.Application.Interfaces.RepositoryInterfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class HotelRepository(HotelBookingDbContext dbContext) : RepositoryBase<Hotel>(dbContext), IHotelRepository
    {
    public async Task<Hotel?> GetByNameAsync(string name)
        {
        // Case-insensitive search using database collation
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(h => EF.Functions.Like(h.Name, name));
        }
    }