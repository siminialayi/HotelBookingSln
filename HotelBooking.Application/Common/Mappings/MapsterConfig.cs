// /HotelBookings.Application/Common/Mappings/MapsterConfig.cs
using HotelBooking.Application.Bookings.Queries.FindBookingDetails;
using HotelBooking.Application.Hotels.Queries.FindHotel;
using HotelBooking.Application.Rooms.Queries.FindAvailableRooms;
using HotelBooking.Domain.Entities;
using Mapster;

namespace HotelBooking.Application.Common.Mappings;

public static class MapsterConfig
    {
    public static void ConfigureMappings()
        {
       
        // Hotel Mapping 
        TypeAdapterConfig<Hotel, HotelDto>.NewConfig()
            .Map(dest => dest.Id, src => src.Id) // Explicitly map GUID
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Address, src => src.Address);

        // Room Mapping 
        TypeAdapterConfig<Room, AvailableRoomDto>.NewConfig()
            .Map(dest => dest.RoomId, src => src.Id) // Rename 'Id' to 'RoomId' for external contract clarity
            .Map(dest => dest.MaxCapacity, src => src.Capacity) // Map 'Capacity' to 'MaxCapacity'
            .Map(dest => dest.RoomNumber, src => src.RoomNumber)
            .Map(dest => dest.Type, src => src.Type)
            .Ignore(dest => dest.HotelName);

        // Booking Mapping
        // Configuration for mapping when all necessary entities (Room, Hotel) 
        // are included via EF Core .Include() in the query.
        TypeAdapterConfig<Booking, BookingDetailsDto>.NewConfig()
            .Map(dest => dest.BookingNumber, src => src.BookingNumber)
            .Map(dest => dest.CheckInDate, src => src.CheckInDate)
            .Map(dest => dest.CheckOutDate, src => src.CheckOutDate)
            .Map(dest => dest.NumberOfGuests, src => src.NumberOfGuests)
            .Map(dest => dest.RoomNumber, src => src.Room.RoomNumber) // Accessing navigation property
            .Map(dest => dest.RoomType, src => src.Room.Type) // Accessing nested enum
            .Map(dest => dest.HotelName, src => src.Room.Hotel.Name); // Deep nesting access
        }
    }