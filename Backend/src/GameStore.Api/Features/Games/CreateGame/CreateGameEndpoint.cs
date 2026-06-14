using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Models;
using static GameStore.Api.Features.Games.CreateGame.CreateGameDtos;

namespace GameStore.Api.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", (CreateGameDto gameDto, GameStoreData data, GameDataLogger logger) =>
        {
            Genre? genre = data.GetGenre(gameDto.GenreId);
            if (genre is null)
            {
                return Results.BadRequest("Invalid genre ID.");
            }

            Game game = new Game
            {
                Name = gameDto.Name,
                Genre = genre,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                Description = gameDto.Description
            };

            data.AddGame(game);

            // Call the logger
            logger.PrintGames();
            
            return Results.CreatedAtRoute(
                EndpointName.GetGame,
                new { id = game.Id },
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.Genre.Id,
                    game.Price,
                    game.ReleaseDate,
                    game.Description
                )
            );
        })
        .WithParameterValidation();
    }
}
