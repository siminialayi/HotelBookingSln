using FluentResults;
using MediatR;

namespace HotelBooking.Application.Hotels.Queries.FindHotel;

/// <summary>
/// MediatR Query to find a hotel by its name.
/// IRequest<Result<T>> ensures we handle success (HotelDto) or failure explicitly.
/// </summary>
public record FindHotelQuery(string Name) : IRequest<Result<HotelDto>>;