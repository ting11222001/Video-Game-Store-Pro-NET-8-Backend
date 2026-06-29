using GameStore.Api.Data;
using GameStore.Api.Models;
using static GameStore.Api.Features.Games.UpdateGame.UpdateGameDtos;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", (Guid id, UpdateGameDto gameDto, GameStoreContext dbContext) =>
        {
            Game? existingGame = dbContext.Games.Find(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = gameDto.Name;
            existingGame.GenreId = gameDto.GenreId;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;

            dbContext.SaveChanges();

            return Results.NoContent();
        })
        .WithParameterValidation();
    }
}
