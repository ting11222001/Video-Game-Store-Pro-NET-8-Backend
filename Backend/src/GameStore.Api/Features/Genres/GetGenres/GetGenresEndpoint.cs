using GameStore.Api.Data;
using static GameStore.Api.Features.Genres.GetGenres.GetGenresDtos;

namespace GameStore.Api.Features.Genres.GetGenres;

public static class GetGenresEndpoint
{
    public static void MapGetGenres(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", (GameStoreData data) => data.GetGenres().Select(
            genre => new GenreDto(genre.Id, genre.Name)));
    }
}
