using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Interfaces.RepositoryInterfaces;
using HotelBooking.Infrastructure.Persistence;
using HotelBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Infrastructure;

public static class ConfigureServices
    {
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
        {
        // Database Context Registration
        // Using SQL Server as the provider
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<HotelBookingDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register Unit of Work and Repositories 
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();

        return services;
        }
    }