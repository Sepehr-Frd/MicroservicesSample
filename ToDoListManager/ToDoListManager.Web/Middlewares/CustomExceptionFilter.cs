using System.Net;
using ToDoListManager.Common.Constants;
using ILogger = Serilog.ILogger;

namespace ToDoListManager.Web.Middlewares;

using ILogger = ILogger;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.Fatal(exception, "Unhandled Exception of Type {exceptionType} Thrown", exception.GetType());

            context.Response.ContentType = "text/plain";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(MessageConstants.InternalServerError);
        }
    }
}