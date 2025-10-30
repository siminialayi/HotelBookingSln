namespace HotelBooking.API.Middleware;

public class CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger) : IMiddleware
    {
    private const string HeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
            {
            correlationId = Guid.NewGuid().ToString("N");
            context.Request.Headers[HeaderName] = correlationId;

            logger.LogDebug("Generated new Correlation ID: {CorrelationId}", correlationId);
            }

        // Echo to response
        context.Response.Headers[HeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object>
            {
                { "CorrelationId", correlationId }
            }))
            {
            await next(context);
            }
        }
    }