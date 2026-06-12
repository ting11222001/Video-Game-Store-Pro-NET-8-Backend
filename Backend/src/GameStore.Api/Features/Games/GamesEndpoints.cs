using GameStore.Api.Data;
using GameStore.Api.Features.Games.CreateGame;
using GameStore.Api.Features.Games.DeleteGame;
using GameStore.Api.Features.Games.GetGame;
using GameStore.Api.Features.Games.GetGames;
using GameStore.Api.Features.Games.UpdateGame;

namespace GameStore.Api.Features.Games;

public static class GamesEndpoints
{
    public static void MapGames (
        this IEndpointRouteBuilder app,
        GameStoreData data
    )
    {
        // Route group
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGetGames(data);

        // GET /games/{id}
        group.MapGetGame(data);

        // POST /games
        group.MapCreateGame(data);

        // PUT /games/{id}
        group.MapUpdateGame(data);

        // DELETE /games/{id}
        group.MapDeleteGame(data);
    }
}
