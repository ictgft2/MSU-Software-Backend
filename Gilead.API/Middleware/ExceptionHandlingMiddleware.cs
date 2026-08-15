using System.Data;
using Microsoft.Data.SqlClient;

namespace Gilead.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            logger.LogWarning("Request was cancelled by the client. TraceId: {TraceId}", context.TraceIdentifier);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = GetError(exception);

        logger.LogError(
            exception,
            "Unhandled exception while processing {Method} {Path}. TraceId: {TraceId}",
            context.Request.Method,
            context.Request.Path,
            context.TraceIdentifier);

        if (context.Response.HasStarted)
        {
            throw exception;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            Succeeded = false,
            Error = environment.IsDevelopment() ? exception.Message : error,
            StatusCode = statusCode,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static (int StatusCode, string Error) GetError(Exception exception) =>
        exception switch
        {
            BadHttpRequestException => (StatusCodes.Status400BadRequest, "Invalid request."),
            DataException => (StatusCodes.Status500InternalServerError, "A data mapping error occurred."),
            SqlException => (StatusCodes.Status500InternalServerError, "A database error occurred."),
            TimeoutException => (StatusCodes.Status503ServiceUnavailable, "The request timed out."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected server error occurred.")
        };
}
