using Microsoft.AspNetCore.Http.Json;
using MyFirstApi.Endpoints;
using MyFirstApi.Middleware;
using MyFirstApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSingleton<UserRepository>();

var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<AuthMiddleware>();

// Map all user-related endpoints
app.MapUserEndpoints();

app.Run();
