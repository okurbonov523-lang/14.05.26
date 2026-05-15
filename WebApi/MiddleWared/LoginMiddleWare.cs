using System.Diagnostics;

namespace WebApi.MiddleWared;

public class LoginMiddleWare
{
    private readonly RequestDelegate _next;

    public LoginMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        System.Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
        System.Console.WriteLine($"Request body: {context.Request.Body}");
        stopwatch.Start();

        await _next(context);

        stopwatch.Stop();
        System.Console.WriteLine($"Response body: {context.Response.StatusCode} processed in {stopwatch.ElapsedMilliseconds} ms");
    }

}
