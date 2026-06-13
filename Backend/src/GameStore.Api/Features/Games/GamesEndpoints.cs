using GameStore.Api.Data;
using GameStore.Api.Features.Games.CreateGame;
using GameStore.Api.Features.Games.DeleteGame;
using GameStore.Api.Features.Games.GetGame;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.UpdateGame;

namespace GameStore.Api.Features.Games;

public static class GamesEndpoints
{
    public static void MapGames (this IEndpointRouteBuilder app)
    {
        // Route group
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGetGames();

        // GET /games/{id}
        group.MapGetGame();

        // POST /games
        group.MapCreateGame();

        // PUT /games/{id}
        group.MapUpdateGame();

        // DELETE /games/{id}
        group.MapDeleteGame();
    }
}
