using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces.RepositoryInterfaces
    {
    public interface IHotelRepository : IRepository<Hotel>
        {
        // Specific query for "Find a hotel based on its name"
        Task<Hotel?> GetByNameAsync(string name);
        }
    }