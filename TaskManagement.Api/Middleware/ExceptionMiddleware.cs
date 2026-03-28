using System.Net;
using System.Text.Json;

namespace TaskManagement.Api.Middleware;

// Global Exception Middleware — catches ALL unhandled errors in one place
// No need for try/catch in every controller
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Let the request continue normally
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, "An unexpected error occurred.");

            // Return a clean JSON error response
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = "An internal server error occurred. Please try again later.",
            detail = exception.Message  // Remove this line in production
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
