using HotelBooking.Application;
using HotelBooking.API.Middleware;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using HotelBookings.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// SERVICE REGISTRATION (Dependency Injection) 

// Framework Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clean Architecture Layer Registration
builder.Services.AddApplicationServices(); // MediatR, Mapster, FluentValidation, PipelineBehavior
builder.Services.AddInfrastructureServices(builder.Configuration); // EF Core, Repositories, UoW

// Observability and Error Handling Setup
// Register the Correlation ID Middleware
builder.Services.AddTransient<CorrelationIdMiddleware>();


// Register the IExceptionHandler implementation
          
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddProblemDetails(); // Required for the ProblemDetailsFactory injection

var app = builder.Build();

// PIPELINE CONFIGURATION (Middleware) 

// Database Initialization (Development/Testing setup)
using (var scope = app.Services.CreateScope())
    {
    var context = scope.ServiceProvider.GetRequiredService<HotelBookingDbContext>();

    //await DatabaseInitializer.ResetDatabaseAsync(context);

    await context.Database.MigrateAsync();
    await DatabaseInitializer.SeedAsync(context);
    }

if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI();
    }

// Correlation ID Middleware: Must run early to capture and inject the ID into logs/responses.
app.UseMiddleware<CorrelationIdMiddleware>();

// Global Exception Handler: Uses the IExceptionHandler service registered above.
app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();