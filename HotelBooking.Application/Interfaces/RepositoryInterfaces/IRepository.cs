namespace HotelBooking.Application.Interfaces.RepositoryInterfaces;

public interface IRepository<T> where T : class
    {
    // All operations are ASYNC for non-blocking I/O
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
    }