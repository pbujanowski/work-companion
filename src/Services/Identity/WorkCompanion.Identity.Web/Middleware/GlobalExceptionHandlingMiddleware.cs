using System.Net;

namespace WorkCompanion.Identity.Web.Middleware;

/// <summary>
/// Global exception handling middleware that catches unhandled exceptions and returns consistent error responses
/// </summary>
public class GlobalExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    /// <summary>
    /// Invokes the middleware to handle the HTTP request and catch exceptions
    /// </summary>
    /// <param name="context">The HTTP context for the current request</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions by mapping them to appropriate HTTP status codes and logging
    /// </summary>
    /// <param name="context">The HTTP context for the current request</param>
    /// <param name="exception">The exception to handle</param>
    /// <returns>A task representing the asynchronous write operation</returns>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.TraceIdentifier;
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An error occurred processing your request";

        switch (exception)
        {
            case ArgumentNullException:
            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid request parameters";
                _logger.LogWarning(exception, "Validation error. CorrelationId: {CorrelationId}", correlationId);
                break;

            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "Resource not found";
                _logger.LogInformation(exception, "Resource not found. CorrelationId: {CorrelationId}", correlationId);
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Unauthorized access";
                _logger.LogWarning(exception, "Unauthorized access. CorrelationId: {CorrelationId}", correlationId);
                break;

            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid operation";
                _logger.LogWarning(exception, "Invalid operation. CorrelationId: {CorrelationId}", correlationId);
                break;

            case OperationCanceledException:
                statusCode = HttpStatusCode.RequestTimeout;
                message = "Request timeout";
                _logger.LogWarning(exception, "Operation cancelled. CorrelationId: {CorrelationId}", correlationId);
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred";
                _logger.LogError(exception, "Unhandled exception. CorrelationId: {CorrelationId}", correlationId);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            message,
            correlationId,
            timestamp = DateTime.UtcNow,
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}

/// <summary>
/// Extension methods for registering global exception handling
/// </summary>
/// <summary>
/// Extension methods for configuring global exception handling middleware
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Registers the global exception handling middleware in the application pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for method chaining</returns>
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
