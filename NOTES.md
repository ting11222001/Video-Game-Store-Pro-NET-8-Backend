# NOTES

## Getting started with ASP.NET Core

### Creating a Web project

Use command line to create a dotnet project:

```bash
$ cd MY_PROJECT_NAME/Backend/src

$ dotnet new web -n GameStore.Api
The template "ASP.NET Core Empty" was created successfully.

Processing post-creation actions...
Restoring C:\Users\Li-Ting\Documents\Projects\Video-Game-Store-Pro\Backend\src\GameStore.Api\GameStore.Api.csproj:
  Determining projects to restore...
  Restored C:\Users\Li-Ting\Documents\Projects\Video-Game-Store-Pro\Backend\src\GameStore.Api\GameStore.Api.csproj (in 82 ms).
Restore succeeded.
```

### Exploring the Web project

Key files:

- `launchSettings.json`: Profiles section has http and https and their port numbers. `"ASPNETCORE_ENVIRONMENT": "Development"` will look at `appsettings.Development.json`.
- `appsettings.json`: can set local environment settings.
- `Program.cs`: the entry point of the app.

### Building a Web project

Use the dotnet CLI:

```bash
$cd Backend/src/GameStore.Api/
$dotnet build

  Restored C:\Users\Li-Ting\Documents\Projects\Video-Game-Store-Pro\Backend\src\GameStore.Api\GameStore.Api.csproj
  (in 67 ms).
  GameStore.Api -> C:\Users\Li-Ting\Documents\Projects\Video-Game-Store-Pro\Backend\src\GameStore.Api\bin\Debug\net
  8.0\GameStore.Api.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:07.26
```

Just make sure the `dotnet build` finds a `.csproj` to build - a `.dll` file will show up, and the bin and the obj folders will contain temporal files too.

### Running and debugging a Web project

Go to `Program.cs` and hit F5. Select C# > Default Configuration.

It will launch the web app at the localhost and port number as set in the `launchSettings.json`.

At `http://localhost:5065/`, I should see:

```
Hello World!
```

Or use .NET CLI to run the project. `cd Backend/src/GameStore.Api`:

```bash
$ dotnet run

Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5065
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\Li-Ting\Documents\Projects\Video-Game-Store-Pro\Backend\src\GameStore.Api
```

### Using the REST Client extension

Create `gamestore.http` file at the root of `Backend` folder.

Write:

```
GET http://localhost:5065
```

Then, run `dotnet run`.

Once the app is up, click `Send Request`.

The `REST Client` extension will open a new panel in VS code:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: text/plain; charset=utf-8
Date: Thu, 04 Jun 2026 03:08:03 GMT
Server: Kestrel
Transfer-Encoding: chunked

Hello World!
```

As expected from the current `Program.cs`:

```csharp
app.MapGet("/", () => "Hello World!");
```

### Knowledge check for this section

- What is ASP.NET Core? It's a web development framework for building web apps on the .NET platform.
- What does the WebApplication class encapsulate in ASP.NET Core projects? All app's resources including an HTTP server, middleware components, logging, DI services, and configuration. 
  - The WebApplication acts as a host that encapsulates all essential resources for the application. 
  - This includes not only middleware components and logging but also an HTTP server implementation, dependency injection (DI) services, and configuration settings, making it a comprehensive setup for an ASP.NET Core project.
- In ASP.NET Core projects, where is the application URL for local development specified?
  - In the launchSettings.json file under the "applicationUrl" setting for each profile.
  - This allows developers to configure and test their applications locally with specific URLs, including setting different ports or even hostnames if needed, directly from their development environment without altering the application's code. 

## Building a REST API with ASP.NET Core

### What is a REST API?

Acronym breakdown:
- REpresentational
- State
- Transfer

The 6 principles:
- Stateless
- Client-Server
- Uniform interface
- Layered system
- Cacheable
- Code on demand

A set of guiding principles that impose conditions on how an API should work.

So the client (e.g. the apps on our device) can reach and interact with the data on the server computer in the internet cloud.

### Interacting with REST APIs

How to Identify Resources in a REST API?

A resource is any object, document or thing that the API can receive from or send to clients.

A URL has three parts:
```
Protocol > Domain > Resource
```

The full URL is called a Uniform Resource Identifier (URI):
```
http://example.com/games
```

Other examples of resources: songs, users, posts, etc.

My project's Games REST API will be doing:
```
GET /games
GET /games/1
POST /games
PUT /games/1
DELETE /games/1
```

### Adding the data model

In `Game.cs` add these properties:
```csharp
using System;

namespace GameStore.Api.Models;

public class Game
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Genre { get; set; }
    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }
}

```

#### `GUID` type

Set `GUID` type to give me unique identifiers without having to have a central coordinateor like a database to assign ids for me. This way in the future if I have a distributed system then each node doesn't have to talk to an external service first and just generate the unique identifier on its own. 

It's also for security - no malicious users can get what is the next number we're assigning to new resources, or what resources are actually available in my API.

It does take more space than an integer to store.

#### `decimal` type

Either float or double are based on a binary floating point arthmetic, which could introduce a tiny inaccuracies due to how decimal numbers are represented in binary format.

But decimal type is using fixed point type, and it's designed for finance and currency calculations, so overall it's just more accurate for `Price` related properties.

#### Test with in-memory games list

In `Program.cs`, note that I'm using `m` to make sure the program is getting the numbers as decimals:
```csharp
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Game> games = [
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = "Roleplaying",
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = "Sports",
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27)
    }
];

app.MapGet("/", () => "Hello World!");

app.Run();
```

#### Extension version issues

##### Re-installed C# Dev Kit to an older version

In `Video-Game-Store-Pro/Backend/src/GameStore.Api/Models`:
```csharp
dotnet new class -n Game
```

Turn out this way of creating the new class file doesn't give me the right namespace.

It should be:
```
namespace GameStore.Api.Models;
```

But somehow the CLI approach only gives me this which is not matching the file structure I have `Backend/src/GameStore.Api/Models`:
```
namespace GameStore.Api;
```

To avoid manually updating namespace myself in the future, I ended up reinstall the C# Dev Kit to the older version to be able to see Solution Explorer which will be the same as the tutorial (the latest version had changed it to C# Project Details).

Also, the `prop` this code hint didn't give me:
```
prop  →  public int MyProperty { get; set; }
```

So I ended up reinstalling to the old version `1.11.14`

### Implementing a GET ALL endpoint

```csharp
app.MapGet("/", () => "Hello World!");
// add request pipeline configurations in between here
app.Run();
```

On `MapGet`, use `ctrl + space`, it will give a list of methods like `MapDelete`, etc.

This is saying when the pattern is matched `/games`, then what should be done:
```csharp
app.MapGet("/games", () => games);
```

To test, go to `gamestore.http` and do:
```
GET http://localhost:5065/games
```

In terminal:
```bash
cd Backend/src/GameStore.Api
dotnet run
```

Then click `Send Request`, REST client will open a window to the right and show these games:
```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 06 Jun 2026 01:54:57 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "bc1afa4e-e553-41e9-ade0-54e3943a4166",
    "name": "Street Fighter II",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "1992-07-15"
  },
  {
    "id": "4ebde087-33b9-4cef-af83-dfd9a87761cc",
    "name": "Final Fantasy XIV",
    "genre": "Roleplaying",
    "price": 59.99,
    "releaseDate": "2010-09-30"
  },
  {
    "id": "52547d20-247a-4fa4-aa74-d944a0c97b08",
    "name": "FIFA 23",
    "genre": "Sports",
    "price": 69.99,
    "releaseDate": "2022-09-27"
  }
]
```

#### `Content-Type: application/json`

By default, .NET core will serialize the response into json format.

### Implementing a GET BY ID endpoint

#### Route Parameters and Guid Type in ASP.NET

One-liner: The name in curly braces is a placeholder you choose, and Guid is a data type for long unique IDs.

Key points:
- `{id}` in the URL pattern must match the parameter name in the method, exactly by name.
- `Guid` is a 128-bit unique identifier, commonly used as a database primary key.
- `ASP.NET` parses the URL value into the type you declare. Wrong format means no match.
- You can use other types too, like int or string, depending on your ID format.

Source/context:
User's code or question snippet:
```csharp
app.MapGet("games/{id}", (Guid id) => games.Find(game => game.Id == id));

// Renaming the parameter - both sides must match
app.MapGet("games/{gameId}", (Guid gameId) => games.Find(game => game.Id == gameId));
```

#### Test the endpoints in `gamestore.http`
To test the API in `gamestore.http`, each endpoint needs to be separated by `###` so that I can see `Send Request`:
```
GET http://localhost:5065/games

###
GET http://localhost:5065/games/3b5eea7e-1888-41c6-b2be-288c3d886cd3
```

After running up the app using `dotnet run`, I can send request to the get all endpoint to see the GUID of games from that session and then send another request to the get id endpoint using those temporary GUID.

#### What if the specific game doesn't exist

So update the `// GET /games/{id}` in `Program.cs`:
```csharp
// old
app.MapGet("games/{id}", (Guid id) => games.Find(game => game.Id == id));

// new
app.MapGet("games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
});
```

Note:
- `Results.NotFound()`: `Results` has well known status codes for REST APIs to use.
- `Results.Ok(game)`: I can't just return `game` which is of `Game` type, but I'm combining the results. So, use `Results.Ok(game)`.

#### Test the get id endpoint

Give it a non-existing id, then it responds as the below:
```
HTTP/1.1 404 Not Found
Content-Length: 0
Connection: close
Date: Sat, 06 Jun 2026 02:15:11 GMT
Server: Kestrel

```

Not confusing status code 200 with a null body ;)

### Implementing the POST endpoint

For the post endpoint, I want to receive the whole body of the `game` I'm creating.

.NET core will deserilaize the json body into C# object.

We want to return the route to the created `game`.

So instead of writing this:
```csharp
// POST /games
app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.Ok(game);
});
```

We write:
```csharp
const string GetGameEndpointeName = "GetGame";

// GET /games/{id}
app.MapGet("games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointeName);

// POST /games
app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointeName, new { id = game.Id }, game);
});
```

Test in `gamestore.http`:
```
###
POST http://localhost:5065/games
Content-Type: application/json

{
    "name": "Minecraft",
    "genre": "Kids and Family",
    "price": 19.99,
    "releaseDate": "2011-11-18"
}
```

It should respond:
```
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 06 Jun 2026 03:19:58 GMT
Server: Kestrel
Location: http://localhost:5065/games/ef74c558-94c6-4346-8fb9-15410cb8c96d
Transfer-Encoding: chunked

{
  "id": "ef74c558-94c6-4346-8fb9-15410cb8c96d",
  "name": "Minecraft",
  "genre": "Kids and Family",
  "price": 19.99,
  "releaseDate": "2011-11-18"
}
```

I can then use that `Location` to get that newly created resource, very convenient.

#### Anonymous Object Creation in C#

One-liner: `new { name = value }` creates a temporary, nameless object with named properties, without needing a class.

Key points:
- This is creating an object, not destructuring one. Destructuring takes things apart; this puts things together.
- The property names you write (like `id`) must match the route parameter names in the URL template.
- Anonymous objects are read-only. You cannot change their properties after creation.
- Common use: passing route values, quick projections in LINQ, and lightweight data grouping.
- Re`sults.CreatedAtRoute` needs to know which route parameters to fill in when building the URL. In this case, the named route expects an `id` parameter, so you pass `new { id = game.Id }` to say "use this game's ID as the `id` in the URL".

Why not just pass game.Id directly?
- Because `CreatedAtRoute` accepts an `object` and uses reflection to read its properties by name. It matches property names to route parameter names. Passing a plain `Guid` has no property names, so it would not know how to map it.

```csharp
// Route template: /games/{id}
// This tells CreatedAtRoute to fill {id} with game.Id
Results.CreatedAtRoute(
    GetGameEndpointeName,
    new { id = game.Id },  // <-- anonymous object, property "id" maps to {id} in route
    game
);
```