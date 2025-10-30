using FluentResults;
using HotelBooking.Application.Interfaces;
using Mapster;
using MediatR;

namespace HotelBooking.Application.Rooms.Queries.FindAvailableRooms;

public class FindAvailableRoomsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindAvailableRoomsQuery, Result<IReadOnlyList<AvailableRoomDto>>>
    {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<AvailableRoomDto>>> Handle(FindAvailableRoomsQuery request,CancellationToken cancellationToken)
        {
        //  Basic Input Validation 
        if (request.CheckInDate >= request.CheckOutDate)
            {
            return Result.Fail("Check-out date must be strictly after the check-in date.");
            }

        // If we only have Deluxe rooms (max 4 capacity), we must fail fast if guests > 4.
        const int MaxHotelRoomCapacity = 4;
        if (request.NumberOfGuests > MaxHotelRoomCapacity)
            {
            return Result.Fail($"Maximum capacity per room is {MaxHotelRoomCapacity}. Please reduce the number of guests.");
            }

        // Async Database Call 
        var rooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(
            request.CheckInDate,
            request.CheckOutDate,
            request.NumberOfGuests);

        // Mapping
        var roomDtos = rooms.Adapt<IReadOnlyList<AvailableRoomDto>>();

        // Returning an empty list is a success (no error), just means no rooms are available.
        return Result.Ok(roomDtos);
        }
    }