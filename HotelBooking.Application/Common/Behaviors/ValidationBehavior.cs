// /HotelBookings.Application/Common/Behaviors/ValidationBehavior.cs
using MediatR;
using FluentValidation;
using FluentResults;
using System.Reflection;

namespace HotelBooking.Application.Common.Behaviors;

// TEACHING MOMENT: This behavior intercepts all MediatR requests (Commands)
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResultBase, new()
    {
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
        if (!_validators.Any())
            {
            return await next();
            }

        // Run all validators for the specific TRequest
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Aggregate validation failures
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            {
            // If validation fails, return a FluentResult failure immediately.
            var result = new TResponse();
            var errors = failures.Select(f => new Error(f.ErrorMessage)
                .WithMetadata("Property", f.PropertyName)
                .WithMetadata("ErrorCode", f.ErrorCode))
                .ToList();

            result.Reasons.AddRange(errors);

            return result; // Fail fast before hitting the Command Handler
            }

        return await next();
        }
    }