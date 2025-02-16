using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{

    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
        .WithParameterValidation();

        //GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => 
        {
            var games = await dbContext
            .Games
            .Include(game => game.Genre)
            .Select(game => game.ToGameSummaryDto())
            .AsNoTracking()
            .ToListAsync();

            return games;

        });

        //GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => 
            {
                var game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

            })
        .WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game.ToGameDetailsDto());
        });

        //PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(existingGame)
            .CurrentValues
            .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => 
        {
            var gameToDelete = dbContext.Games.Where(game => game.Id == id).ToList();
            dbContext.Games.RemoveRange(gameToDelete);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }

}
