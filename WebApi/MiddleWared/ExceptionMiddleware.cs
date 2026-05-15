using System.Text.Json;

namespace WebApi.MiddleWared;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ArgumentException ex)
        {
            _logger.LogWarning(ex.Message);
            context.Response.StatusCode  = 400;

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                new
                {
                    Error = ex.Message
                }
            ));
        }
    }
}
