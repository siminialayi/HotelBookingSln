using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.RepositoryInterfaces;
using HotelBooking.Infrastructure.Persistence;

namespace HotelBooking.Infrastructure.Repositories;

public class UnitOfWork(HotelBookingDbContext dbContext) : IUnitOfWork, IDisposable
    {
    private readonly HotelBookingDbContext _dbContext = dbContext;

    // Lazily instantiated repositories
    private IBookingRepository? _bookingRepository;
    private IRoomRepository? _roomRepository;
    private IHotelRepository? _hotelRepository;

    //implementations
    public IBookingRepository Bookings => _bookingRepository ??= new BookingRepository(_dbContext);
    public IRoomRepository Rooms => _roomRepository ??= new RoomRepository(_dbContext);
    public IHotelRepository Hotels => _hotelRepository ??= new HotelRepository(_dbContext); 

    public async Task<int> CompleteAsync()
        {
        return await _dbContext.SaveChangesAsync();
        }

    // Implement IDisposable pattern for clean resource management
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
        {
        if (!_disposed)
            {
            if (disposing)
                {
                _dbContext.Dispose();
                }
            _disposed = true;
            }
        }

    public void Dispose()
        {
        Dispose(true);
        GC.SuppressFinalize(this);
        }
    }

