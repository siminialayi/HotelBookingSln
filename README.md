******************************************************************🚀 Hotel Booking API**************************************************************************


This document provides essential instructions for setting up, configuring, and running the Hotel Booking API, an enterprise-grade solution built on Clean Architecture principles.

******************************************************⚙️ 1. Technology Stack & Architecture**********************************************************************
This API adheres strictly to the Clean Architecture pattern, ensuring strong separation of concerns.

**Framework**: ASP.NET Core 8+
**Language**: C#
**Persistence**: Entity Framework (EF) Core with SQL Server LocalDB
**Patterns**: Command Query Responsibility Segregation (CQRS) via MediatR
**Validation**: Fluent Validation (enforced via MediatR Pipeline)
**Error Handling**: Fluent Results for business logic errors and RFC 7807 Problem Details for standardized API responses
**Observability**: Correlation ID pattern for tracing requests across the system

**Core layers:**

Domain
Application
Infrastructure
Presentation

**********************************************************🛠️ 2. Setup and Run************************************************************************************
**Prerequisites****

.NET 8 SDK (or later)
SQL Server LocalDB (usually installed with Visual Studio)

  **Steps**

****1. Clone the Repository**

git clone https://github.com/siminalayi/HotelBookingSln.git
cd HotelBookingSln

**2. Restore Dependencies**

dotnet restore

**3. Configure Connection String**
Update appsettings.json in HotelBookings.Presentation:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelBookings_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

**4. Run the Application**

dotnet run --project HotelBookings.Presentation
``
**5. Access Swagger**
Navigate to:
https://localhost:8081/swagger/index.html (Port may vary)


**********************************************************💾 3. Database Initialization (Seeding & Resetting)****************************************************
On first run, Program.cs automatically handles database initialization:

**Migration**

await context.Database.MigrateAsync();

**Seeding**

await DatabaseInitializer.SeedAsync(context);

**Seeded data includes**:

One hotel: Grand Hyatt Downtown
Six rooms: 2 Single, 2 Double, 2 Deluxe

**To reset the database for clean testing, uncomment this line in Program.cs before SeedAsync (for dev/test only):**

// await DatabaseInitializer.ResetDatabaseAsync(context);
``
****************************************************🔒 4. Observability and Error Handling************************************************************************
**Correlation ID**

A Correlation ID (X-Correlation-ID) is automatically generated or read from the request header by CorrelationIdMiddleware.
This ID is echoed in the response header and added to the logging context for end-to-end tracing.

**Global Exception Handling**

Unexpected exceptions are caught by IExceptionHandler.
Errors are converted into standardized 500 Internal Server Error responses compliant with RFC 7807, including traceId and correlationId.


**********************************************************************🧪 5. Testing the API Endpoints**********************************************************
Use Swagger UI to execute the following test plan against the seeded data:
**5.1.** Find Hotel

Endpoint: GET /api/Hotels
Action: Search for the seeded hotel using name=Grand Hyatt Downtown.
Expected: 200 OK with hotel details (case-insensitive search).

**5.2.** Find Available Rooms

Endpoint: GET /api/Rooms/available
Action: Search for rooms with:

checkIn=2025-11-10
checkOut=2025-11-12
guests=1

Expected: 200 OK with 6 available rooms.

**5.3.** Book a Room

Endpoint: POST /api/Bookings
Action: Book a room using a valid RoomId and dates.
Expected: 201 Created with a unique booking number.

**5.4.** Test Business Rules

Capacity Test: Try NumberOfGuests=5 for a room with capacity 2.
Double Booking Test: Use the same RoomId and dates as a previous booking.
Expected: 400 Bad Request with structured Problem Details.

**5.5.** Find Booking Details

Endpoint: GET /api/Bookings/{bookingNumber}
Action: Retrieve booking details using the saved booking number.
Expected: 200 OK with full booking details including RoomNumber and HotelName.

