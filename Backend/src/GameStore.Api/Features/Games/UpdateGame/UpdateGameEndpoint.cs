using GameStore.Api.Data;
using GameStore.Api.Models;
using static GameStore.Api.Features.Games.UpdateGame.UpdateGameDtos;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(
        this IEndpointRouteBuilder app, 
        GameStoreData data)
    {
        app.MapPut("/{id}", (Guid id, UpdateGameDto gameDto) =>
        {
            Game? existingGame = data.GetGame(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            Genre? genre = data.GetGenre(gameDto.GenreId);

            if (genre is null)
            {
                return Results.BadRequest("Invalid genre ID.");
            }

            existingGame.Name = gameDto.Name;
            existingGame.Genre = genre;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;

            return Results.NoContent();
        })
        .WithParameterValidation();
    }
}
