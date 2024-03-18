using QueueManagementApi.Core;

namespace QueueManagementApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch (QueueApiException ex)
        {
            await HandleQueueApiException(context, ex);
        }
        catch (Exception ex)
        {
            // Handle the exception
            await HandleUnknownException(context, ex);
        }
    }

    private Task HandleQueueApiException(HttpContext context, QueueApiException queueApiException)
    {
        // Log the exception and related information
        _logger.LogError(queueApiException, "An exception occurred on endpoint '{Endpoint}' in controller '{Controller}'", context.Request.Path, context.Request.RouteValues["controller"]);

        // Return a plain text as message with http status 400
        context.Response.ContentType = "text/plain";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        return context.Response.WriteAsync(queueApiException.Message);
    }

    private Task HandleUnknownException(HttpContext context, Exception exception)
    {
        // Log the exception and related information
        _logger.LogError(exception, "An exception occurred on endpoint '{Endpoint}' in controller '{Controller}'", context.Request.Path, context.Request.RouteValues["controller"]);

        // Return a plain text as message with http status 500
        context.Response.ContentType = "text/plain";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
    }
}