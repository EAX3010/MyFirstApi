namespace MyFirstApi.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    public LoggingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"[{DateTime.Now}] Request: {context.Request.Method} {context.Request.Path}");
        await _next(context);
    }
}
