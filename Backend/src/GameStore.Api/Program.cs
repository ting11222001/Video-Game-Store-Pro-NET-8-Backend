using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Genre> genres = [
    new Genre { Id = new Guid("d8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Fighting" },
    new Genre { Id = new Guid("b8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Roleplaying" },
    new Genre { Id = new Guid("c8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Sports" },
    new Genre { Id = new Guid("d8a77400-e414-4e5a-a695-c3ec90ce6178"), Name = "Racing" }
];

List<Game> games = [
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description = "The classic fighting game that defined the genre."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = genres[1],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30),
        Description = "An epic MMORPG with a rich story and vibrant world."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = genres[2],
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27),
        Description = "The latest installment in the popular soccer franchise."
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Gran Turismo 7",
        Genre = genres[3],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2022, 9, 27),
        Description = "The latest entry in the acclaimed racing series."
    }
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new { id = game.Id },
        game
    );
})
.WithParameterValidation();

// PUT /games/{id}
app.MapPut("/games/{id}", (Guid id, Game updatedGame) =>
{
    Game? existingGame = games.Find(game => game.Id == id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
})
.WithParameterValidation();

// DELETE /games/{id}
app.MapDelete("/games/{id}", (Guid id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();
