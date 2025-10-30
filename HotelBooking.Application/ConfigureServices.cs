using FluentValidation;
using HotelBooking.Application.Common.Behaviors;
using HotelBooking.Application.Common.Mappings;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelBooking.Application;

public static class ConfigureServices
    {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
        // Configure Mapster & Mapping Registration
        MapsterConfig.ConfigureMappings();
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, Mapper>();

        // Add MediatR and register handlers/requests
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Add Fluent Validation
        // This registers all validators defined in the current assembly.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register MediatR Pipeline Behaviors (for cross-cutting concerns)
        // Pipeline Behaviors are like middleware for MediatR.
        // The ValidationBehavior ensures the Fluent Validation runs BEFORE the handler.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
        }
    }