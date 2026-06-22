using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// VERSION 1: DbContext
var connString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connString);

// REGISTER SERVICES HERE
builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddSingleton<GameStoreData>();

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.Run();
