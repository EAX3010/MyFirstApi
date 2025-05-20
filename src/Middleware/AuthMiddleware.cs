namespace MyFirstApi.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    public AuthMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("X-Api-Key"))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing API Key.");
            return;
        }

        var apiKey = context.Request.Headers["X-Api-Key"];
        if (apiKey != "secret-key") // Replace with real logic
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        await _next(context);
    }
}
