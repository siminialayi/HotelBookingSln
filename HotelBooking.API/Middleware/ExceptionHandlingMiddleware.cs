using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;

namespace HotelBooking.API.Middleware;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, ProblemDetailsFactory problemDetailsFactory, IHostEnvironment env) : IExceptionHandler
    {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
        // If a response is already being sent, let the default pipeline continue
        if (httpContext.Response.HasStarted)
            {
            logger.LogWarning("Response started before exception could be handled.");
            return false;
            }

        var status = MapStatusCode(exception);
        var title = GetTitle(status);

        // Log with full details; payload to client remains safe
        logger.LogError(exception, "Unhandled exception. StatusCode={StatusCode}", status);


        // Detail message varies by environment, showing full exception only in Development
        var detail = env.IsDevelopment()
            ? exception.ToString()
            : "An unexpected error occurred. Please try again later.";

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        var correlationId = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? traceId;

        httpContext.Response.Clear();
        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/problem+json";

        var problem = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode: status,
            title: title,
            detail: detail,
            instance: httpContext.Request.Path
        );

        problem.Extensions["traceId"] = traceId;
        problem.Extensions["correlationId"] = correlationId;

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
        }

    private static int MapStatusCode(Exception ex) =>
        ex switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                InvalidOperationException => StatusCodes.Status409Conflict,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
                };

    private static string GetTitle(int status) =>
        status switch
            {
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status409Conflict => "Conflict",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                _ => "Error"
                };
    }