using Microsoft.AspNetCore.Http.Json;
using MyFirstApi.Models;
using MyFirstApi.Repositories;
using MyFirstApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddSingleton<UserRepository>();

var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<AuthMiddleware>();

app.MapGet("/users", (UserRepository repo) => Results.Ok(repo.GetAll()));

app.MapGet("/users/{id}", (int id, UserRepository repo) =>
{
    var user = repo.Get(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
});

app.MapPost("/users", (User user, UserRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
        return Results.BadRequest("Name and Email are required.");

    var created = repo.Create(user);
    return Results.Created($"/users/{created.Id}", created);
});

app.MapPut("/users/{id}", (int id, User updatedUser, UserRepository repo) =>
{
    if (!repo.Exists(id)) return Results.NotFound();

    if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
        return Results.BadRequest("Name and Email are required.");

    repo.Update(id, updatedUser);
    return Results.Ok(updatedUser);
});

app.MapDelete("/users/{id}", (int id, UserRepository repo) =>
{
    if (!repo.Exists(id)) return Results.NotFound();

    repo.Delete(id);
    return Results.NoContent();
});

app.Run();
