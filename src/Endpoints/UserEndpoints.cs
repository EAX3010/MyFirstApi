using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyFirstApi.Models;
using MyFirstApi.Repositories;

namespace MyFirstApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", GetAllUsers);
        app.MapGet("/users/{id}", GetUser);
        app.MapPost("/users", CreateUser);
        app.MapPut("/users/{id}", UpdateUser);
        app.MapDelete("/users/{id}", DeleteUser);
    }

    private static IResult GetAllUsers(UserRepository repo)
    {
        return Results.Ok(repo.GetAll());
    }

    private static IResult GetUser(int id, UserRepository repo)
    {
        var user = repo.Get(id);
        return user is null ? Results.NotFound() : Results.Ok(user);
    }

    private static IResult CreateUser(User user, UserRepository repo)
    {
        if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            return Results.BadRequest("Name and Email are required.");

        var created = repo.Create(user);
        return Results.Created($"/users/{created.Id}", created);
    }

    private static IResult UpdateUser(int id, User updatedUser, UserRepository repo)
    {
        if (!repo.Exists(id)) return Results.NotFound();

        if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
            return Results.BadRequest("Name and Email are required.");

        repo.Update(id, updatedUser);
        return Results.Ok(updatedUser);
    }

    private static IResult DeleteUser(int id, UserRepository repo)
    {
        if (!repo.Exists(id)) return Results.NotFound();

        repo.Delete(id);
        return Results.NoContent();
    }
}