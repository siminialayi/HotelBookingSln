using HotelBooking.Application.Interfaces.RepositoryInterfaces;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class RepositoryBase<T>(HotelBookingDbContext dbContext) : IRepository<T> where T : class
    {
    protected readonly HotelBookingDbContext DbContext = dbContext;
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id)
        {
        return await DbSet.FindAsync(id);
        }

    public async Task<IReadOnlyList<T>> GetAllAsync()
        {
        return await DbSet.AsNoTracking().ToListAsync();
        }

    public async Task AddAsync(T entity)
        {
        await DbSet.AddAsync(entity);
        }

    // Simple Update and Delete 
    public void Update(T entity)
        {
        DbSet.Update(entity);
        }

    public void Delete(T entity)
        {
        DbSet.Remove(entity);
        }
    }