using FluentResults;
using HotelBooking.Application.Interfaces;
using Mapster; 
using MediatR;

namespace HotelBooking.Application.Hotels.Queries.FindHotel;

public class FindHotelQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FindHotelQuery, Result<HotelDto>>
    {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<HotelDto>> Handle(FindHotelQuery request, CancellationToken cancellationToken)
        {
        // Async Database Call
        var hotel = await _unitOfWork.Hotels.GetByNameAsync(request.Name);

        // Error Handling using FluentResult
        if (hotel == null)
            {
            return Result.Fail($"Hotel named '{request.Name}' was not found.");
            }

        // Mapster handles the projection (mapping Hotel -> HotelDto).
        var hotelDto = hotel.Adapt<HotelDto>();

        return Result.Ok(hotelDto);
        }
    }