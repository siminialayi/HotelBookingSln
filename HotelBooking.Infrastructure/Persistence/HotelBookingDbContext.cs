using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence;

public class HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : DbContext(options)
    {

    // DbSet properties for our core entities
    public DbSet<Hotel> Hotels { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        // Hotel Configuration
        modelBuilder.Entity<Hotel>().HasKey(h => h.Id);
        modelBuilder.Entity<Hotel>().Property(h => h.Name).IsRequired().HasMaxLength(100);

        // Room Configuration
        modelBuilder.Entity<Room>().HasKey(r => r.Id);
        modelBuilder.Entity<Room>().Property(r => r.RoomNumber).IsRequired().HasMaxLength(10);
        modelBuilder.Entity<Room>().Property(r => r.Capacity).IsRequired();
        modelBuilder.Entity<Room>().Property(r => r.Type).HasConversion<string>(); // Store enum as string

        // Relationship: Hotel has many Rooms
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId);

        // Booking Configuration
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);
        modelBuilder.Entity<Booking>().Property(b => b.BookingNumber).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Booking>().Property(b => b.CheckInDate).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.CheckOutDate).IsRequired();

        // Business Rule Enforcement: Booking numbers must be unique
        modelBuilder.Entity<Booking>().HasIndex(b => b.BookingNumber).IsUnique();

        // Relationship: Room has many Bookings
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId);

        // General: Check Out Date must be after Check In Date (Database-level constraint)
        modelBuilder.Entity<Booking>().ToTable(t => t.HasCheckConstraint("CK_Booking_Dates", "[CheckOutDate] > [CheckInDate]"));

        base.OnModelCreating(modelBuilder);
        }
    }