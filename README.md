# Hotel Booking API

Enterprise-grade Hotel Booking API implemented with Clean Architecture principles.

## Technology stack & architecture

- Framework: ASP.NET Core 8+
- Language: C#
- Persistence: Entity Framework Core with SQL Server LocalDB
- Patterns: CQRS via MediatR
- Validation: FluentValidation (enforced via MediatR pipeline)
- Error handling: Fluent Results for business errors and RFC 7807 Problem Details for standardized API responses
- Observability: Correlation ID pattern for tracing requests

Core layers:
- Domain
- Application
- Infrastructure
- Presentation

## Prerequisites

- .NET 8 SDK (or later)
- SQL Server LocalDB (usually installed with Visual Studio)

## Quick start

1. Clone the repository

   git clone https://github.com/siminialayi/HotelBookingSln.git
   cd HotelBookingSln

2. Restore dependencies

   dotnet restore

3. Configure the connection string

   Update appsettings.json in HotelBookings.Presentation (or the environment-specific settings you use):

   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelBookings_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }

4. Run the application

   dotnet run --project HotelBookings.Presentation

   Optionally, to bind to a specific URL/port:

   dotnet run --project HotelBookings.Presentation --urls "https://localhost:8081"

5. Open Swagger

   Navigate to https://localhost:8081/swagger/index.html (port may vary if not set explicitly)

## Database initialization (migrations, seeding & reset)

On first run, Program.cs applies migrations and seeds data automatically:

- Migrations: await context.Database.MigrateAsync();
- Seeding: await DatabaseInitializer.SeedAsync(context);

Seeded data includes:
- One hotel: Grand Hyatt Downtown
- Six rooms: 2 Single, 2 Double, 2 Deluxe

To reset the database (development/testing only), uncomment the ResetDatabaseAsync call in Program.cs before the SeedAsync call:

// await DatabaseInitializer.ResetDatabaseAsync(context);

Be careful: resetting the database will delete and recreate the schema and seeded data.

## Observability and error handling

- Correlation ID: CorrelationIdMiddleware reads or generates an X-Correlation-ID for each request. The correlation ID is returned in response headers and included in logs for end-to-end tracing.
- Global exception handling: Unexpected exceptions are handled centrally and converted into Problem Details (RFC 7807) responses that include traceId and correlationId.

## Testing the API (recommended manual tests via Swagger)

Use Swagger UI to exercise the API using the seeded data:

1. Find hotel
   - GET /api/Hotels
   - Query: name=Grand Hyatt Downtown
   - Expected: 200 OK with hotel details (case-insensitive search)

2. Find available rooms
   - GET /api/Rooms/available
   - Query:
     - checkIn=2025-11-10
     - checkOut=2025-11-12
     - guests=1
   - Expected: 200 OK with 6 available rooms

3. Book a room
   - POST /api/Bookings
   - Provide a valid RoomId and dates
   - Expected: 201 Created with a unique booking number

4. Test business rules
   - Capacity test: try NumberOfGuests=5 for a room with capacity 2 (expect 400 Bad Request)
   - Double booking test: attempt to book the same room for overlapping dates (expect 400 Bad Request with Problem Details)

5. Find booking details
   - GET /api/Bookings/{bookingNumber}
   - Expected: 200 OK with full booking details (RoomNumber, HotelName, etc.)

## Contributing

Please open issues or pull requests against this repository. Follow existing code patterns and include tests where applicable.

## Notes

- The port used by the application may vary depending on how the ASP.NET Core host is configured or overridden by environment variables.
- Resetting the database is intended for development/testing only; do not use it in production.

If you want any additional sections (deployment, CI, example requests/responses, or environment variables), tell me what you'd like included and I will update the README.
