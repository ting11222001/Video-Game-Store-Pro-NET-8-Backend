using System.ComponentModel.DataAnnotations;
using GameStore.Api.Data;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

GameStoreData data = new();

// GET /games
app.MapGet("/games", () => data.GetGames().Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre.Name,
    game.Price,
    game.ReleaseDate
)));

// GET /games/{id}
app.MapGet("games/{id}", (Guid id) =>
{
    Game? game = data.GetGame(id);
    return game is null ? Results.NotFound() : Results.Ok(
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
.WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto gameDto) =>
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
    return Results.CreatedAtRoute(
        GetGameEndpointName,
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

// PUT /games/{id}
app.MapPut("/games/{id}", (Guid id, UpdateGameDto gameDto) =>
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

// DELETE /games/{id}
app.MapDelete("/games/{id}", (Guid id) =>
{
    data.RemoveGame(id);
    return Results.NoContent();
});

// GET /genres
app.MapGet("/genres", () => data.GetGenres().Select(
    genre => new GenreDto(genre.Id, genre.Name)));

app.Run();

// DTO for returning game details without exposing the Genre object
public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description
);

// DTO for returning game summaries in the list endpoint
public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);

// DTO for creating a new game, which includes GenreId instead of the full Genre object
public record CreateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100, ErrorMessage = "Price must be between 1 and 100.")] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
);

// DTO for updating an existing game, which also includes GenreId instead of the full Genre object
public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100, ErrorMessage = "Price must be between 1 and 100.")] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
);

// DTO for returning genre information
public record GenreDto(Guid Id, string Name);