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

### Adding Server-Side Validation

If I only sent this at POST:

```
###
POST http://localhost:5065/games
Content-Type: application/json

{
    "genre": "Kids and Family",
    "price": 19.99,
    "releaseDate": "2011-11-18"
}
```

It will print this as the body is missing the `name` property:

```
HTTP/1.1 400 Bad Request
Connection: close
Content-Type: text/plain; charset=utf-8
Date: Sat, 06 Jun 2026 03:39:43 GMT
Server: Kestrel
Transfer-Encoding: chunked

Microsoft.AspNetCore.Http.BadHttpRequestException: Failed to read parameter "Game game" from the request body as JSON.
```

This is as expected but the response is not ideal.

Then, if I sent this which makes the `name` property as an empty string:

```
###
POST http://localhost:5065/games
Content-Type: application/json

{
    "name": "",
    "genre": "Kids and Family",
    "price": 19.99,
    "releaseDate": "2011-11-18"
}
```

It prints this which is bad as it's still giving us `201`:

```
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 06 Jun 2026 03:41:28 GMT
Server: Kestrel
Location: http://localhost:5065/games/8d1b1b80-c2a3-4c1f-8a4a-059f7f9d0572
Transfer-Encoding: chunked

{
  "id": "8d1b1b80-c2a3-4c1f-8a4a-059f7f9d0572",
  "name": "",
  "genre": "Kids and Family",
  "price": 19.99,
  "releaseDate": "2011-11-18"
}
```

#### First fix

In `Program.cs` I can do this:

```csharp
// POST /games
app.MapPost("/games", (Game game) =>
{
    if (string.IsNullOrEmpty(game.Name))
    {
        return Results.BadRequest("Game name is required.");
    }

    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new { id = game.Id },
        game
    );
});

```

This give me:

```
HTTP/1.1 400 Bad Request
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 06 Jun 2026 03:45:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

"Game name is required."
```

But this means I will need to validate each property of the sent object myself.

#### Use Data Annotation in `Game.cs` instead

Add `[Required]`:

```csharp
using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Models;

public class Game
{
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public required string Genre { get; set; }
    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }
}
```

So no object with an empty `Name` can come into my API.

But this is not enough as it's still printing:

```
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 06 Jun 2026 03:49:57 GMT
Server: Kestrel
Location: http://localhost:5065/games/cd048f08-1f57-49a7-b982-71f36b70c585
Transfer-Encoding: chunked

{
  "id": "cd048f08-1f57-49a7-b982-71f36b70c585",
  "name": "",
  "genre": "Kids and Family",
  "price": 19.99,
  "releaseDate": "2011-11-18"
}
```

Turns out - I'm using minimal Web API and it doesn't have much under the hood so it won't enforce the data annotations.

I need to one extra thing called `endpoint filter`.

- This endpoint filter mechanism will help enforce those data annotations before letting the request get into my endpoints.
- I can use a library so I don't need to write this endpoint filter manually. It's on NuGet [here](https://nuget.org/).

Search `minimalapis.extensions`. Find `MinimalApis.Extensions`. Install by:

```
dotnet add package MinimalApis.Extensions --version 0.11.0
```

Make sure the terminal is at `GameStore.Api` which is where the `.csproj` file is at.

In terminal:

```bash
Backend/src/GameStore.Api (main)
$ dotnet add package MinimalApis.Extensions --version 0.11.0
  Determining projects to restore...
  Writing C:\Users\Li-Ting\AppData\Local\Temp\tmp1fk1bz.tmp
info : X.509 certificate chain validation will use the default trust store selected by .NET for code signing.
...
```

Then, double check in `.csproj` to see this newly added package reference:

```
 <ItemGroup>
    <PackageReference Include="MinimalApis.Extensions" Version="0.11.0" />
  </ItemGroup>
```

So now I can add this `WithParameterValidation`, so that the data annotations in the data model `Game.cs` will be checked before allowing any requests to actually jump into my endpoints:

```csharp
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
```

Now, test the endpoint again:

```
###
POST http://localhost:5065/games
Content-Type: application/json

{
    "name": "",
    "genre": "Kids and Family",
    "price": 19.99,
    "releaseDate": "2011-11-18"
}
```

It will print this well-formed problem json object instead of just exception trace:

```
HTTP/1.1 400 Bad Request
Connection: close
Content-Type: application/problem+json
Date: Sat, 06 Jun 2026 04:08:51 GMT
Server: Kestrel
Transfer-Encoding: chunked

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": [
      "The Name field is required."
    ]
  }
}
```

### Implementing the PUT endpoint

I can start writing the PUT method like the below - it's convention to let the PUT method return `204 No Content`:

```csharp
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
```

Test the PUT endpoint:

```
###
PUT http://localhost:5065/games/ff9f5f37-b80e-49ce-b484-f5154dcaabc4
Content-Type: application/json

{
    "name": "Street Fighter II Turbo",
    "genre": "Fighting",
    "price": 9.99,
    "releaseDate": "1992-07-15"
}
```

It prints this as expected:

```
HTTP/1.1 204 No Content
Connection: close
Date: Sat, 06 Jun 2026 09:53:45 GMT
Server: Kestrel

```

It works for now.

However, this is not thread safe in production where there will be multiple requests into my API i.e. updating the same record at the same time:

```csharp
    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
```

I can use `ConcurrentBag` to replace the in-memory list `List` in the `Program.cs`:

```csharp
ConcurrentBag<Game> games = [
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
```

But that's just a note to know. I'm still using `List` for now.

Also, test the PUT method wiht a non-existing id, and it prints this as expected:

```
HTTP/1.1 404 Not Found
Content-Length: 0
Connection: close
Date: Sat, 06 Jun 2026 09:58:14 GMT
Server: Kestrel
```

### Implementing the DELETE endpoint

Add this in `Program.cs`:

```csharp
// DELETE /games/{id}
app.MapDelete("/games/{id}", (Guid id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});
```

Test by deleting the Minecraft game:

```
###
DELETE http://localhost:5065/games/583a7003-1113-4877-8a68-24f15ea233df
```

It prints this as expected:

```
HTTP/1.1 204 No Content
Connection: close
Date: Sat, 06 Jun 2026 10:03:45 GMT
Server: Kestrel

```

Also, double check with the Get All request that the Minecraft game was gone:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sat, 06 Jun 2026 10:04:00 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "6bb607c2-17e2-4657-bd01-ad8dfd3457cc",
    "name": "Street Fighter II",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "1992-07-15"
  },
  {
    "id": "30a8703d-40a0-4c87-bada-c57d17a38903",
    "name": "Final Fantasy XIV",
    "genre": "Roleplaying",
    "price": 59.99,
    "releaseDate": "2010-09-30"
  },
  {
    "id": "0640fbe5-5efb-4444-be9a-d23ce3022e43",
    "name": "FIFA 23",
    "genre": "Sports",
    "price": 69.99,
    "releaseDate": "2022-09-27"
  }
]
```

### Knowledge check so far

#### Which statement best reflects the relationship between APIs and REST?

##### Answer

An API provides a set of functions to clients, and REST imposes conditions on how these functions should be presented and interacted with.

##### Explain

An API (Application Programming Interface) is a set of definitions and protocols for building and integrating application software. It defines the methods and data formats for communicating with the service from an application.

REST (Representational State Transfer), on the other hand, is an architectural style that defines a set of constraints for creating Web services. RESTful services enable interacting parties to communicate over the Web using the standard HTTP protocol.

The principles of REST guide the design of the architecture for APIs, focusing on stateless communication, resource-based URLs, and the use of HTTP methods to perform operations.

##### Analogy

Think of it this way:

API is like a waiter at a restaurant. It takes your order (request) and brings back your food (response). It's the messenger between you and the kitchen (the system).

REST is like the restaurant's rules for how the waiter should behave. For example:

- Always speak clearly (use standard HTTP methods like GET, POST, DELETE)
- Don't remember previous customers (stateless, each request stands alone)
- Refer to items by name on the menu (use resource based URLs like `/orders/123`)

So the relationship is: an API is the thing that lets two systems talk to each other. REST is a set of rules that tells you how to design that API well.

A REST API simply means an API that follows those REST rules.

##### The core REST rules using the restaurant analogy

1. Stateless (The waiter has amnesia)
   Every time you call the waiter, they have no memory of your last visit or even your last order. Each request must include all the information needed. You can't say "the same as before." You must always give the full order.

2. Resource based URLs (Everything on the menu has a name)
   Every "thing" in the system has its own address. For example:

`/customers/42` is customer number 42
`/orders/99` is order number 99

You don't say "go get that thing." You point to it by name.

3. HTTP methods (The type of request you make)
   Instead of saying everything in words, you use standard actions:

- GET = "Can I see the menu?" (read something)
- POST = "I'd like to order something new" (create something)
- PUT = "I want to change my whole order" (replace something)
- PATCH = "Just swap the drink" (update part of something)
- DELETE = "Cancel my order" (remove something)

4. Uniform interface (Every waiter works the same way)
   No matter which waiter serves you, they all follow the same process. This makes the system predictable and easy to work with.

5. Client and server are separate (You and the kitchen don't need to know each other)
   You only talk to the waiter. You don't need to know how the kitchen works. The kitchen doesn't need to know who you are. They just handle orders and send food out.

6. Cacheable (Popular orders are remembered at the counter)
   If 100 people order the same thing, the kitchen doesn't need to cook it fresh 100 times. The result can be stored temporarily and reused. This makes things faster.

## Using Data Transfer Objects

### Expanding the data model

Create a new data model class for `Genre`.

In `Program.cs` create a in-memory list of Genres but hardcoding the Guid.

By doing this in PowerShell:

```
PS C:\Projects\Video-Game-Store-Pro> [guid]::NewGuid()

Guid
----
d8a77400-e414-4e5a-a695-c3ec90ce6177
```

Then paste the generated Guid into the list now:

```csharp
List<Genre> genres = [
    new Genre { Id = new Guid("d8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Fighting" },
    new Genre { Id = new Guid("b8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Roleplaying" },
    new Genre { Id = new Guid("c8a77400-e414-4e5a-a695-c3ec90ce6177"), Name = "Sports" },
    new Genre { Id = new Guid("d8a77400-e414-4e5a-a695-c3ec90ce6178"), Name = "Racing" }
];
```

Test it in the `gamestore.http`:

```
GET http://localhost:5065/games
```

I can see genre is returning both id and name, but what if for my future home catalog page that I only need the name, not the id?

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 07 Jun 2026 12:44:52 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "d6ec98db-31ba-4621-bd46-02ef5dac1111",
    "name": "Street Fighter II",
    "genre": {
      "id": "d8a77400-e414-4e5a-a695-c3ec90ce6177",
      "name": "Fighting"
    },
    "price": 19.99,
    "releaseDate": "1992-07-15",
    "description": "The classic fighting game that defined the genre."
  },
  ...
]
```

I also don't necessarily need the description of the games as what if I need to list hundres of games then that will be too much.

I will also need to fix what objects to POST:

```
###
POST http://localhost:5065/games
Content-Type: application/json

{
  "name": "Minecraft",
  "genre": "Kids and Family", -> here should just use the Genre id to show the relation
  "price": 19.99,
  "releaseDate": "2011-11-18"
}
```

So I need a way to define what's exposed to the client.

### Understanding Data Transfer Objects

A Data Transfer Object (DTO) is an object that carries data between processes or applications.

In the context of a REST API, a DTO can be considered a contract between the client and server.

Always return a specific shape of data to the client.

### Using DTOs with GET requests

In `Program.cs`, the get specific game currently looks like this:

```csharp
// GET /games/{id}
app.MapGet("games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);
```

Create a `Record` type for this DTO:

```csharp
public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description
);
```

Records are immutable - once it's created, it cannot be changed anymore. It makes our life easier when setting the properties too, like we can use one line instead of setting up any constructors, or getters, or setters like `Class`.

Then, replace the returned `game` in `GET /games/{id}` endpoint:

```csharp
// GET /games/{id}
app.MapGet("games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
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
```

Then, all update the `GET /games` endpoint - basically change each `game` in this `games` list into the `GameSummaryDto`:

```csharp
// Old: GET /games
app.MapGet("/games", () => games);

// New: GET /games
app.MapGet("/games", () => games.Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre.Name, // note: it's a string now, so return the name of the Genre
    game.Price,
    game.ReleaseDate
)));

public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
```

Then, test it and I should see the `genre` property is returning a string, regardless now it's been changed to a composite property as client doesn't need to know how we store things internally.

Then, add a new endpoint to get all genres:

```csharp
// GET /genres
app.MapGet("/genres", () => genres.Select(
    genre => new GenreDto(genre.Id, genre.Name)));

// DTO for returning genre information
public record GenreDto(Guid Id, string Name);
```

Test it and it prints as expected:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 09 Jun 2026 00:14:27 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "d8a77400-e414-4e5a-a695-c3ec90ce6177",
    "name": "Fighting"
  },
  {
    "id": "b8a77400-e414-4e5a-a695-c3ec90ce6177",
    "name": "Roleplaying"
  },
  {
    "id": "c8a77400-e414-4e5a-a695-c3ec90ce6177",
    "name": "Sports"
  },
  {
    "id": "d8a77400-e414-4e5a-a695-c3ec90ce6178",
    "name": "Racing"
  }
]
```

### Using DTOs with POST requests

Make sure the data annotations e.g. required, string length, etc. are copied from the Models into the Dtos.

This helps us check if the input parameters are fine.

Then, add association to the Genre - make sure the Genre exists in our database first before continuing.

`CreateGameDto` is the way how we're going to store the data internally.

But when responding back to client, we use `GameDetailsDto`.

Changes:

```csharp
// Old: POST /games
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

// New: POST /games
app.MapPost("/games", (CreateGameDto gameDto) =>
{
    Genre? genre = genres.Find(g => g.Id == gameDto.GenreId);
    if (genre is null)
    {
        return Results.BadRequest("Invalid genre ID.");
    }

    // Saving the data into database
    Game game = new Game
    {
        Id = Guid.NewGuid(),
        Name = gameDto.Name,
        Genre = genre,
        Price = gameDto.Price,
        ReleaseDate = gameDto.ReleaseDate,
        Description = gameDto.Description
    };

    games.Add(game);
    return Results.CreatedAtRoute(
        GetGameEndpointName,
        new { id = game.Id },
        new GameDetailsDto( // return back to client so only need to return back GenreId, not the whole Genre obj
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
```

Test it - take an existing GenreId and paste it into the POST endpoint:

```
Old:
###
POST http://localhost:5065/games
Content-Type: application/json

{
    "name": "Minecraft",
    "genre": "Kids and Family",
    "price": 19.99,
    "releaseDate": "2011-11-18"
}

New:
###
POST http://localhost:5065/games
Content-Type: application/json

{
    "name": "Minecraft",
    "genreId": "b8a77400-e414-4e5a-a695-c3ec90ce6177",
    "price": 19.99,
    "releaseDate": "2011-11-18",
    "description": "A sandbox game that allows players to build and explore virtual worlds made of blocks."
}
```

It prints this as expected:

```
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 09 Jun 2026 00:42:09 GMT
Server: Kestrel
Location: http://localhost:5065/games/cc939d7e-417e-4a6e-a103-e790b9b4cecf
Transfer-Encoding: chunked

{
  "id": "cc939d7e-417e-4a6e-a103-e790b9b4cecf",
  "name": "Minecraft",
  "genreId": "b8a77400-e414-4e5a-a695-c3ec90ce6177",
  "price": 19.99,
  "releaseDate": "2011-11-18",
  "description": "A sandbox game that allows players to build and explore virtual worlds made of blocks."
}
```

And the get all endpoints will return a list with this new game - note that it won't return the description info:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 09 Jun 2026 00:44:31 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "1db743b4-9850-42e6-a4a9-108f03def383",
    "name": "Street Fighter II",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "1992-07-15"
  },
...
  {
    "id": "cc939d7e-417e-4a6e-a103-e790b9b4cecf",
    "name": "Minecraft",
    "genre": "Roleplaying",
    "price": 19.99,
    "releaseDate": "2011-11-18"
  }
]
```

### Using DTOs with PUT requests

#### Test

Before changing the first game it was like this in the Get All endpoint:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 09 Jun 2026 01:04:00 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "fd9162bb-7231-43d4-903b-733fd2773e27",
    "name": "Street Fighter II",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "1992-07-15"
  },
  ...
]
```

Get a genre id that I have:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 09 Jun 2026 01:06:25 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "d8a77400-e414-4e5a-a695-c3ec90ce6177",
    "name": "Fighting"
  },
  ...
]
```

Replace with the correct game id and genre id:

```
Old:
###
PUT http://localhost:5065/games/ff9f5f37-b80e-49ce-b484-f5154dcaabc4
Content-Type: application/json

{
    "name": "Street Fighter II Turbo",
    "genre": "Fighting",
    "price": 9.99,
    "releaseDate": "1992-07-15"
}

New:
###
PUT http://localhost:5065/games/fd9162bb-7231-43d4-903b-733fd2773e27
Content-Type: application/json

{
    "name": "Street Fighter II Turbo",
    "genreId": "d8a77400-e414-4e5a-a695-c3ec90ce6177",
    "price": 9.99,
    "releaseDate": "1992-07-15",
    "description": "The most iconic fighting game of all time!"
}
```

After running PUT endpoint, it shows updated successfully:

```
HTTP/1.1 204 No Content
Connection: close
Date: Tue, 09 Jun 2026 01:05:03 GMT
Server: Kestrel

```

Then, check again with get all endpoint again. I can see it's updated correctly with the correct genre and name:

```
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Tue, 09 Jun 2026 01:05:28 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "fd9162bb-7231-43d4-903b-733fd2773e27",
    "name": "Street Fighter II Turbo",
    "genre": "Fighting",
    "price": 9.99,
    "releaseDate": "1992-07-15"
  },
  ...
]
```

Finally, as we now we use DTO to define the inputs into our endpoints, I can remove those data annotations from my Models, just keep those rules on DTOs.

Cleaned `Game` Models:

```csharp
namespace GameStore.Api.Models;

public class Game
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public required Genre Genre { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public required string Description { get; set; }
}
```

Now just rely on the Dtos and the rules there. For example:

```csharp
public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100, ErrorMessage = "Price must be between 1 and 100.")] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
);
```

### Knowledge check

#### In a REST API, what is the primary purpose of using a Data Transfer Object (DTO)?

A Data Transfer Object (DTO) serves as a contract in a REST API by defining the expected structure and content of data exchanged between the client and the server.

This contract ensures consistency and compatibility, even if the underlying database models change. By using DTOs, the API can adapt to changes in database structure without breaking the client, as only the DTO structure (the "contract") remains consistent in client-server communication.

This approach helps maintain a stable interface for clients, enhancing the API’s reliability and flexibility.

#### Why are record types preferred for defining DTOs in .NET applications?

Record types in .NET are preferred for defining DTOs because they are immutable by default. Once their properties are set, usually at the time of creation, they cannot be changed.

This immutability is perfect for DTOs as they are meant to carry data from one point to another without the need for modification, ensuring data consistency and integrity as it moves across different layers of an application or between applications.

#### When implementing a POST /games endpoint, why should a GameDetailsDto be returned in the 201 Created response instead of the Game model?

Returning a GameDetailsDto in the response instead of the Game model helps prevent exposing the internal structure and potentially sensitive details of the Game object to the client.

The DTO only includes the data necessary for the client, ensuring that implementation details or any fields that should remain private are not inadvertently exposed.

## Vertical Slice Architecture

### Encapsulating the data

Remember the temp data lists, games and genres in the `Program.cs`.

Create `GameStoreData.cs` to move the endpoints from `Program.cs` to there.

`genres` is now encapsulated in the `GameStoreData` class and it's `readonly`.

```csharp
public class GameStoreData
{
    private readonly List<Genre> genres = [
      ...
    ]
}
```

Using `IEnumerable` to let the returned games be able to be iterated over, but not modified by insert, delete, etc.

```csharp
public IEnumerable<Game> GetGames() => games;

// same as using block body:
public IEnumerable<Game> GetGames() {
  return games;
}
```

#### Explanation - Field initialisation order in C#

- `genres` is initialised inline (at the field declaration)

```csharp
private readonly List<Genre> genres = [ ... ]; // inline, no dependency
```

- `games` is initialised inside the constructor

```csharp
private readonly List<Game> games;  // declared only

public GameStoreData()
{
    games = [ ... Genre = genres[0] ... ];   // assigned in constructor, after genres is ready
}
```

This is because each Game object references `genres[0]`, `genres[3]`, etc. Those index lookups only work if `genres` is already populated. By the time the constructor body runs, the inline field `genres` is already ready, so it's safe to use.

### Using the encapsulated data

Start to replace the below in `Program.cs`.

These are the same:
```csharp
GameStoreData data = new();

// GameStoreData data = new GameStoreData();

// GET /games
app.MapGet("/games", () => data.GetGames().Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre.Name,
    game.Price,
    game.ReleaseDate
)));
```
etc.

#### Explanation - `.Select()` in LINQ

`.Select()` is a LINQ method in C#. LINQ is a built-in set of tools for working with collections of data, like lists or arrays.

Think of `.Select()` like a factory conveyor belt. You put items in one end, and each item gets transformed into something new at the other end. The original list stays the same. You get a new list of transformed items.

In your code, `data.GetGenres()` returns a list of raw Genre objects from your data source. `.Select()` then loops over each one and converts it into a `GenreDto` object instead.

```csharp
// For each `genre` in the list, create a new GenreDto using its Id and Name.
.Select(genre => new GenreDto(genre.Id, genre.Name))
```

A `Dto` stands for Data Transfer Object. It is a stripped-down version of your data model. You use it to control what gets sent back to the client. In this case, instead of sending the full `Genre` object (which might have extra fields you don't want to expose), you only send `Id` and `Name`.

The `genre => new GenreDto(...)` part is a lambda. It is a small, inline function that says: "given one genre, do this thing."

### Introduction to Vertical Slice Architecture

To organise all the endpoints in `Program.cs` better, we're going to use this Vertical Slice Architecture.

#### Structuring code the old way
Layers:
```
Presentation Layer
Business Logic Layer
Data Access Layer
Database
```
Create Game files per layer:
```
Presentation: CreateGameDto.cs (request), GameDetailsDto.cs (response), GamesController.cs
Business Logic: Game.cs, GamesService.cs, IGamesRepository.cs
Data Access: GamesRepository.cs
```

Note: "Too many things to change across too many places"

#### Structuring code around slices

- Codebase is divided into independent features (slices)
- Each slice contains everything needed for a specific feature

So, structuring a slice like this:
```
Input -> Handler -> Output
(CreateGameDto -> CreateGameEndpoint -> GameDetailsDto)
```

### Adding the first vertical slice

Create `Features/Games/GetGames` folder.

Each feature is one folder. E.g. one for retrieving the full list of games.

- `GetGamesDto`: this holds all the related DTOs of this feature. Moved from `Program.cs`.
- `GetGamesEndpoint`: this holds all the related endpoints. Moved from `Program.cs`.


#### Why making `GetGamesEndpoint` static

Key points:

- It is a language design rule, not a technical limitation.
- A `static` class cannot be instantiated or inherited, so it is just a plain holder for methods.
- It signals to other developers that the class has no state and no side effects.
- The rule keeps extension methods easy to find and reason about.

#### Why using `this IEndpointRouteBuilder app` in `public static void MapGetGames(this IEndpointRouteBuilder app, GameStoreData data)`

First of all, when hovering over `MapGet()`, it shows this code hint:
```
(extension) RouteHandlerBuilder IEndpointRouteBuilder.MapGet
```

So `MapGet` is one of the extension methods from `IEndpointRouteBuilder`.

`IEndpointRouteBuilder` is Microsoft's type. app is an object of that type. Microsoft did not give it a `MapGetGames` method.

So the tutorial adds one:
```csharp
public static void MapGetGames(this IEndpointRouteBuilder app, GameStoreData data)
```

This says: "add `MapGetGames` to `IEndpointRouteBuilder`, even though we do not own that type."
Now you can write:
```csharp
app.MapGetGames(data);
```
As if Microsoft put it there themselves.

##### A simpler example

Say you have a `string` and you want a method called `Shout` that adds exclamation marks.

```csharp
string name = "hello";
name.Shout(); // you want this to work
```

`string` is Microsoft's class. You cannot add `Shout` to it. But with an extension method you can:
```csharp
public static class StringExtensions
{
    public static string Shout(this string text)
    {
        return text + "!!!";
    }
}
```

Now `name.Shout()` works. You did not touch Microsoft's `string` class at all.

### Adding the next slices

A feature to get a game by id and another feature to create a brand new game.

Create `Constants/EndpointName`:
```csharp
public static class EndpointName
{
    public const string GetGame = nameof(GetGame); // so I don't need to have a hardcoded string here
}
```

#### Why using a `static` class to store the constant values

Think of a regular class like a cookie cutter. You use it to make individual cookies (objects). Each cookie is separate.

A static class is different. It is not a cookie cutter. It is more like a notice board. The notice board exists on its own. You do not make copies of it. You just walk up to it and read what is there.

Because `EndpointName` is `static`, you refer to it directly by its name, like reading a sign on a wall:
```csharp
EndpointName.GetGame
// "Go to the EndpointName notice board. Read the GetGame sign."
```

The `const` or `static` keyword on the values inside also means they belong to the class itself, not to any object.

#### When to use `static`

Ask yourself: does this need "state"?

`State` means data stored inside an object that can change over time. For example:

```csharp
public class Counter
{
    private int _count = 0; // this is state

    public void Increment() => _count++;
    public int GetCount() => _count;
}
```

Each `Counter` instance has its own `_count`. You need to instantiate it because each one is independent.

When to use `static`?

Use `static` when the method or class is just a function. It takes input, does work, returns output. No stored data needed.

```csharp
public static class MathHelper
{
    public static int Add(int a, int b) => a + b;
}
```

`Add` doesn't need to remember anything between calls. No reason to create an object.

Your `GetGameEndpoint` is the same idea. It's just wiring up a route. No data stored inside it. So `static` makes sense.

So:
- When Needs to store and manage its own data: Instance (non-static)
- When Needs to be injected as a dependency: Instance (non-static)
- Just doing a job, no memory needed: static
- Shared utility or helper functions: static


Why does OOP work this way?

OOP was designed around modelling real world things that have identity and state. A BankAccount has a balance. A User has a name. These make sense as objects.

But not everything in a program is a "thing". Some code is just behaviour. Extension methods, helpers, and mappers are behaviour. Forcing them into objects with state would be artificial.

So C# gives you static as an escape hatch. It says "this is just a function that lives inside a class for organisation, not a blueprint for objects."

#### Used `Record` class for DTOs

A `record` is a shorthand C# type that is immutable, easy to compare, and ideal for carrying data between layers.

Key points:

- Immutable by default. Data cannot be changed after creation.
- Less boilerplate. One line replaces many lines of a class.
- Value equality built in. Two records with the same data are considered equal.
- Plays nicely with ASP.NET Core's JSON serialiser.

For example:
```csharp
// Record DTO (positional syntax)
public record UserDto(int Id, string Name);

// Regular class equivalent (much more code)
public class UserDto
{
    public int Id { get; init; }
    public string Name { get; init; }

    public UserDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
```


### Adding the final slices

Add update a game and delete a game slices. Also, get all genres slice.

#### Nested Record Types and Endpoint Parameter Binding

You pass the inner record `CreateGameDto` because that is the actual data shape, and the outer `CreateGameDtos` is just a container for organising it.

`CreateGameDtos` is the outer container. `CreateGameDto` is the actual shape of the data you want to receive.

Think of it like a box (`CreateGameDtos`) that holds a form (`CreateGameDto`). When someone submits data to your endpoint, you want the filled-in form, not the whole box.

```csharp
public record class CreateGameDtos       // <-- the box (outer container)
{
    public record CreateGameDto(...)     // <-- the form inside the box (the actual data shape)
    { ... }
}
```

### Using route groups

So now I have this in `Program.cs`:
```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

GameStoreData data = new();

// GET /games
app.MapGetGames(data);

// GET /games/{id}
app.MapGetGame(data);

// POST /games
app.MapCreateGame(data);

// PUT /games/{id}
app.MapUpdateGame(data);

// DELETE /games/{id}
app.MapDeleteGame(data);

// GET /genres
app.MapGetGenres(data);

app.Run();
```

But these endpoints can get really long, so I need a new extension method class called `GamesEndpoints`.

And with the `group` I can make the url even easier:
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

GameStoreData data = new();

app.MapGames(data);
app.MapGenres(data);

app.Run();

// GamesEndpoints.cs
public static class GamesEndpoints
{
  public static void MapGames (
      this IEndpointRouteBuilder app,
      GameStoreData data
  )
  {
    // Route group
    var group = app.MapGroup("/games");

    group.MapGetGames(data);

// GetGamesEndpoint.cs
public static class GetGamesEndpoint
{
  public static void MapGetGames(this IEndpointRouteBuilder app, GameStoreData data)
  {
    // GET /games
    app.MapGet("/", () => data.GetGames().Select(game => new GameSummaryDto(
```


#### Why I can write `app.MapGames(data);` instead of `GamesEndpoints.MapGames(app, data);` in `Program.cs`

The key is the `this` keyword in the first parameter.
```csharp
public static void MapGames(this IEndpointRouteBuilder app, GameStoreData data)
```

When you add `this` before a parameter, C# treats that method as belonging to that type. So instead of calling it like a regular static method:
```csharp
GamesEndpoints.MapGames(app, data);  // "Hey GamesEndpoints class, run MapGames"
```

You can call it as if `app` owns the method:
```csharp
app.MapGames(data);  // "Hey app, run MapGames"
```

Both lines do the exact same thing. The compiler converts `app.MapGames(data)` into `GamesEndpoints.MapGames(app, data)` behind the scenes.

Also, `app` in `Program.cs` is a `WebApplication`. That type implements `IEndpointRouteBuilder`. Your extension method targets `IEndpointRouteBuilder`. So `app` qualifies.

### Knowledge check

#### What is one of the main advantages of using Vertical Slice Architecture over n-tier architecture?

It allows each feature to be implemented within a single cohesive unit, reducing unnecessary abstraction.

#### What is a primary benefit of using extension methods in C#?

They allow new methods to be added to existing types without modifying their original type.

Extension methods provide a way to "extend" a class by adding new methods without altering the class's original code or structure. This is especially useful for built-in or third-party classes that can't be changed directly.

In this module, MapGetGame is added to IEndpointRouteBuilder to add routing functionality in a modular way. It looks like a native method of IEndpointRouteBuilder, promoting cleaner and more readable code without modifying the original class.

#### What is one of the main benefits of using route groups in ASP.NET Core Minimal APIs?

They reduce redundancy by applying a common route prefix and settings to multiple endpoints.

## Dependency Injection

### Understanding dependency injection

#### What is a Dependency?

`MyService` → LogThis("foo") → `MyLogger` (DEPENDENCY)

```csharp
public MyService()
{
    var logger = new MyLogger();
    logger.LogThis("I'm Ready!");
}
```

↓

```csharp
public MyService()
{
    var writter = new MyFileWritter("output.log");
    var logger = new MyLogger(writter);
    logger.LogThis("I'm Ready!");
}
```

> this is known as dependency injection because when `MyLogger` now needs a new dependency e.g. be passed in a `MyFileWriter` instance i.e. `output.log`, `MyService` needs to be updated accordingly.

**Problems**
- `MyService` is tightly coupled to the Logger dependency. Any changes to `MyLogger` require changes to `MyService`.
- MyService needs to know how to construct and configure the `MyLogger` dependency.
- It's hard to test `MyService` since the `MyLogger` dependency cannot be mocked or stubbed.

#### What is Dependency Injection?

`MyService` → LogThis("foo") → `MyLogger` (DEPENDENCY)

It means `MyService` has `MyLogger` as a dependency so it can output the log using `LogThis`.

```csharp
public MyService(MyLogger logger)
{
    logger.LogThis("I'm Ready!");
}
```

**Service Container**
- `MyLogger` → Register → `IServiceProvider`
- `Another Dependency` → Register → `IServiceProvider`
- `IServiceProvider` resolves, constructs and injects dependencies when there is a HTTP Request and it will constructs `MyService` and `MyService` dependencies together.


**Benefits**
- `MyService` won't be affected by changes to its dependencies.
- `MyService` doesn't need to know how to construct or configure its dependencies.
- Dependencies can also be injected as parameters to minimal API endpoints.
- Opens the door to using Dependency Inversion.

#### Using Dependency Inversion

**The Dependency Inversion Principle**
> "Code should depend on abstractions as opposed to concrete implementations."

**Diagram**
- `MyService` ~~depends on~~ `MyLogger` ❌
- `MyService` depends on `ILogger` (abstraction)
- `MyLogger`, `CloudLogger`, `ConsoleLogger`, `DBLogger` all implement `ILogger`

```csharp
public MyService(ILogger logger)
{
    logger.LogThis("I'm Ready!");
}
```

**Benefits**
- The logger dependency can be swapped out for a different implementation without modifying `MyService`.
- It's easier to test `MyService` since the logger dependency can be mocked or stubbed.
- Code is cleaner, easier to modify and easier to reuse.

### Understanding service lifetimes

#### When should instances be created?

Your Web App receives HTTP Requests and has two services:
- **MyService** (uses MyLogger)
- **AnotherService** (uses MyLogger)

The **IServiceProvider** is responsible for resolving, constructing, and injecting dependencies.

**MyLogger** is registered with the **IServiceProvider**.

When a service needs **MyLogger**, the IServiceProvider decides:
- Create new MyLogger instance?
- Reuse same MyLogger instance?

#### The Transient Service Lifetime

**MyLogger** is registered using `AddTransient<MyLogger>()`.

The **IServiceProvider** resolves, constructs, and injects a **new MyLogger instance every time** it is requested:
- **MyService** gets its own MyLogger instance (per HTTP Request)
- The second HTTP Request gets another new MyLogger instance
- **AnotherService** gets its own separate MyLogger instance

> Transient lifetime services are created each time they are requested from the service container (IServiceProvider)

#### The Scoped Service Lifetime

**MyLogger** is registered using `AddScoped<MyLogger>()`.

The **IServiceProvider** resolves, constructs, and injects **one MyLogger instance per HTTP Request**, shared within that request:
- **HTTP Request 1:** Both **MyService** and **AnotherService** share the **same** MyLogger instance
- **HTTP Request 2:** A **new** MyLogger instance is created and shared across services for that request

> Scoped lifetime services are created once per HTTP request and reused within that request.

#### The Singleton Service Lifetime

**MyLogger** is registered using `AddSingleton<MyLogger>()`.

The **IServiceProvider** resolves, constructs, and injects **one MyLogger instance for the entire application lifetime**:
- **HTTP Request 1:** Both **MyService** and **AnotherService** share the **same** MyLogger instance
- **HTTP Request 2:** Both services still use that **same** MyLogger instance

> Singleton lifetime services are created the first time they are requested and reused across the application lifetime.

### Using transient services

Start by replacing the `GameStoreData` to be registered in a service container in `Program.cs`.

Instead of passing `data` layer by layer:
```csharp
// Program.cs
GameStoreData data = new();
app.MapGames(data);

// GamesEndpoints.cs
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
        ...
    }
}

// GetGamesEndpoint.cs
public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app, GameStoreData data)
    {
        app.MapGet("/", () => data.GetGames().Select(game => new GameSummaryDto(
            game.Id,
            game.Name,
            game.Genre.Name,
            game.Price,
            game.ReleaseDate
        )));
    }
}
```

I can register a service container with `GameStoreData` in it (note that this service container needs to be registered before the `app`, i.e. the web application builder is built), and removed the passing-down `data`. At the final level, in `app.MapGet` I can directly say I need a `GameStoreData data` in the delegate function parameter, and .NET will be smart enough to find it in the service container:
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// REGISTER SERVICES HERE
builder.Services.AddTransient<GameStoreData>();

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.Run();

// GamesEndpoints.cs
public static class GamesEndpoints
{
    public static void MapGames (this IEndpointRouteBuilder app)
    {
        // Route group
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGetGames();
        ...
    }
}

// GetGamesEndpoint.cs
public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", (GameStoreData data) => data.GetGames().Select(game => new GameSummaryDto(
            game.Id,
            game.Name,
            game.Genre.Name,
            game.Price,
            game.ReleaseDate
        )));
    }
}
```

But remember that transient service means there will be new instances created per HTTP request?

So if I test the apis by creating a new game which will be successfully created and added to the `GameStoreData`, but when I run `get games` endpoint, the newly added game will not be shown, and all the `game` record Id keeps being updated when the `get games` endpoint is triggered.

I can put a breakpint in `GameStoreData.cs` the `games` line, and then keep clicking on `get games` send request to see how each request will trigger a new `GameStoreData` object to be constructed.

### Using scoped services

Create `src/Data/GameDataLogger` and create a method to print all the `games` in the games collection in the `GameStoreData`.

Putting `GameStoreData` in the `GameDataLogger` constructor, we can receive it from dependency injection. And use `Quick fix` to make those `data` and `logger` into primary constructors so that we can use them across this class without having to assign them to a private variable first.

```csharp
// old
public class GameDataLogger
{
    public GameDataLogger(GameStoreData data, ILogger<GameDataLogger> logger)
    {
        
    }
}

// new
public class GameDataLogger(GameStoreData data, ILogger<GameDataLogger> logger)
{
  ...
}
```

And, add `PringGames()` as the method in `GameDataLogger`.

Then, register this `GameDataLogger` to a service container in `Program.cs`. Use `AddTransient` for now.

Use this `GameDataLogger` in `CreateGameEndpoint`, right after `data.AddGame(game);`.

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddTransient<GameStoreData>();

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.Run();


// CreateGameEndpoint.cs

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

```

When testing the `POST` api (as I just added the `GameDataLogger` in `CreateGameEndpoint`), somehow this newly added game won't show in the terminal log.

```bash
// The new game is created
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 14 Jun 2026 12:57:44 GMT
Server: Kestrel
Location: http://localhost:5065/games/086e103a-006f-4095-94c1-3614d1ee1097
Transfer-Encoding: chunked

{
  "id": "086e103a-006f-4095-94c1-3614d1ee1097",
  "name": "Minecraft",
  "genreId": "4e179397-c3f1-45ec-a271-c26f07ff64f3",
  "price": 19.99,
  "releaseDate": "2011-11-18",
  "description": "A sandbox game that allows players to build and explore virtual worlds made of blocks."
}

// but in the terminal it's not added to the `data` collection:
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: a9a30300-6fe3-4e74-ab25-a0190e5935cb | Game Name: Street Fighter II
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: 34c59dcc-ae29-4ec7-b54d-05ae91b6434f | Game Name: Final Fantasy XIV
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: b30f4e10-7aab-40e7-aad7-19c72b8fb7d5 | Game Name: FIFA 23
```

Because `GameDataLogger` is getting another `GameStoreData` instance (I used `transient` in `Program.cs`):
```csharp
builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddTransient<GameStoreData>();
```

So even though I called the logger after the new game is added, the POST endpoint's `GameStoreData` here is going to be different from the `GameDataLogger`'s data:
```csharp
// In CreateGameEndpoint.cs, this `GameStoreData` instance is going to be different from the one injected into logger when the logger is constructed:
data.AddGame(game);

logger.PrintGames();

// In GameDataLogger.cs, when the logger is constructed in GameDataLogger, it's getting another GameStoreData instance:
public class GameDataLogger(GameStoreData data, ILogger<GameDataLogger> logger)
```

Try changing the `GameStoreData` to `AddScoped` in `CreateGameEndpoint`, so it means that the POST endpoint itself should receive one instance of `GameStoreData`, and also anything from its body of request should also receive the same `GameStoreData`:
```csharp
// In Program.cs
builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddScoped<GameStoreData>();
```

So test the POST endpoint again - the newly created game is now logged successfully in the terminal:
```bash
HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 14 Jun 2026 13:13:54 GMT
Server: Kestrel
Location: http://localhost:5065/games/ee29a508-05c2-4e80-aba5-db94fafa5fb8
Transfer-Encoding: chunked

{
  "id": "ee29a508-05c2-4e80-aba5-db94fafa5fb8",
  "name": "Minecraft",
  "genreId": "4e179397-c3f1-45ec-a271-c26f07ff64f3",
  "price": 19.99,
  "releaseDate": "2011-11-18",
  "description": "A sandbox game that allows players to build and explore virtual worlds made of blocks."
}

info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: fbe46154-bc9c-4ebc-9671-f3c1678f3485 | Game Name: Street Fighter II
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: 4a653976-eb1a-4cbf-873c-b7ce4e15e7a9 | Game Name: Final Fantasy XIV
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: 63b978de-97a5-4762-a7a7-6c30c9653d4a | Game Name: FIFA 23
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: ee29a508-05c2-4e80-aba5-db94fafa5fb8 | Game Name: Minecraft
```

But remember after this `POST` request is done, any future requests will receive a brand new `GameStoreData` instance again.

Like if I run `GetGames` endpoint, then I will see a new `GameStoreData` instance is constructed which will not have my newly added game:
```bash
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Sun, 14 Jun 2026 13:16:52 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "22b271f1-811b-43be-a5b0-ac4d372a55a3",
    "name": "Street Fighter II",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "1992-07-15"
  },
  {
    "id": "ca5d7ecf-1e15-477c-ac3e-05a444ff7be6",
    "name": "Final Fantasy XIV",
    "genre": "Roleplaying",
    "price": 59.99,
    "releaseDate": "2010-09-30"
  },
  {
    "id": "5b6173f7-7283-4fde-8a82-66a7f6e00d10",
    "name": "FIFA 23",
    "genre": "Sports",
    "price": 69.99,
    "releaseDate": "2022-09-27"
  }
]
```

I can add breakpoints over `CreateGameEndpoint`, `GameDataLogger` and `GameStoreData` and hit F5 to see when they're being called after I click `Send Request` on POST and GET Games endpoints in `gamestore.http`.

### Using singleton services

So now make `GameStoreData` as the one and only instance across the entire lifetime of the application for every requests.
```csharp
// Program.cs
builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddSingleton<GameStoreData>();
```

Test with the POST endpoint - the new game is created successfully:
```bash
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: 37c12262-1d51-49eb-a4ae-ecc3ee75bf49 | Game Name: Street Fighter II
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: 83a09aa9-4f20-4632-8f34-ad556cb0db04 | Game Name: Final Fantasy XIV
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: e2814923-6741-4ef3-b497-ff1e2fc21669 | Game Name: FIFA 23
info: GameStore.Api.Data.GameDataLogger[0]
      Game Id: a3888e23-48a0-40a9-a872-5100b8140ab3 | Game Name: Minecraft

HTTP/1.1 201 Created
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Jun 2026 14:12:17 GMT
Server: Kestrel
Location: http://localhost:5065/games/a3888e23-48a0-40a9-a872-5100b8140ab3
Transfer-Encoding: chunked

{
  "id": "a3888e23-48a0-40a9-a872-5100b8140ab3",
  "name": "Minecraft",
  "genreId": "4e179397-c3f1-45ec-a271-c26f07ff64f3",
  "price": 19.99,
  "releaseDate": "2011-11-18",
  "description": "A sandbox game that allows players to build and explore virtual worlds made of blocks."
}
```

And then test GET games endpoint - the GameStoreData is now having the new game i.e. it's not being reconstructed:
```bash
HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8
Date: Mon, 15 Jun 2026 14:13:10 GMT
Server: Kestrel
Transfer-Encoding: chunked

[
  {
    "id": "37c12262-1d51-49eb-a4ae-ecc3ee75bf49",
    "name": "Street Fighter II",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "1992-07-15"
  },
  {
    "id": "83a09aa9-4f20-4632-8f34-ad556cb0db04",
    "name": "Final Fantasy XIV",
    "genre": "Roleplaying",
    "price": 59.99,
    "releaseDate": "2010-09-30"
  },
  {
    "id": "e2814923-6741-4ef3-b497-ff1e2fc21669",
    "name": "FIFA 23",
    "genre": "Sports",
    "price": 69.99,
    "releaseDate": "2022-09-27"
  },
  {
    "id": "a3888e23-48a0-40a9-a872-5100b8140ab3",
    "name": "Minecraft",
    "genre": "Fighting",
    "price": 19.99,
    "releaseDate": "2011-11-18"
  }
]
```

#### Interesting thing to note

I can technically write even though the newly created game will not show up in GET games endpoint:
```csharp
builder.Services.AddSingleton<GameDataLogger>(); // singleton can depend on a transient service
builder.Services.AddTransient<GameStoreData>();
```

but not this because when a singleton is constructed, there is no scope, so I can't have a singleton obejct expecting to consume a scope service when there's no scope:
```csharp
builder.Services.AddSingleton<GameDataLogger>(); // singleton cannot depend on a scoped service
builder.Services.AddScoped<GameStoreData>();
```

but I can do this:
```csharp
builder.Services.AddScoped<GameDataLogger>(); // scope can depend on a singleton service
builder.Services.AddSingleton<GameStoreData>();
```

##### Singleton depending on Transient (works, but with a catch)

A singleton is created once for the whole application. A transient service is created fresh every time it's requested.

When `GameDataLogger` (singleton) asks for a `GameStoreData` (transient), .NET creates one instance of `GameStoreData` and gives it to `GameDataLogger`. `GameDataLogger` then holds onto that instance forever, because it's a singleton.

The problem: every other part of your app that asks for `GameStoreData` normally gets a brand new copy. But `GameDataLogger` is stuck with the one copy it got at startup. So if `GameStoreData` holds an in-memory list of games, the copy inside `GameDataLogger` never sees new games added later. That's why a new game won't show up if you check it through `GameDataLogger`.

It compiles and runs without errors, but it's a logic bug waiting to happen.

##### Singleton depending on Scoped (throws an error)

A scoped service is meant to live for one request (in a web app, one HTTP request gets one instance).

A singleton is built once, when the app starts. At that point, there's no HTTP request happening yet. So there's no "scope" for the scoped service to belong to.

.NET checks for this and throws an exception at runtime (or at startup if you enable validation), with a message like "Cannot consume scoped service from singleton". It refuses to let this happen because it would be unclear which request's data the singleton should be using.

##### Scoped depending on Singleton (works fine)

This is the safe direction. Each request creates a new `GameDataLogger` (scoped). When it asks for `GameStoreData` (singleton), it just gets the one shared instance that already exists.

There's no timing problem here, the singleton already exists before any request starts, so it's always available.

##### Simple rule to remember

Think of lifetimes as having a "width": Singleton is widest (whole app lifetime), Scoped is medium (one request), Transient is narrowest (one usage).

A service can safely depend on something with an equal or wider lifetime, but not a narrower one. Singleton → Scoped is "wide depending on narrow", which breaks. Scoped → Singleton is "narrow depending on wide", which is fine.

### Knowledge check

#### What is the main purpose of Dependency Injection (DI) in ASP.NET Core?

Dependency Injection allows a class to rely on dependencies without managing their creation or configuration. Instead, dependencies are provided to the class, which simplifies code by removing the need for direct instantiation.

#### In ASP.NET Core, which service lifetime should you use when you want an instance of a service to be created every time it is requested?

n ASP.NET Core, the dependency injection container provides three different lifetimes for services:

Singleton lifetime services are created the first time they are requested or when the application starts and then every subsequent request will use the same instance. If your application requires a singleton behavior, you would use this lifetime.

Scoped lifetime services are created once per client request (connection). This is particularly useful for things like entity framework contexts that are meant to be used per operation or per request.

Transient lifetime services are created each time they are requested from the service container. This lifetime works best for lightweight, stateless services because a new instance is provided to every controller and every service that requires it.

Therefore, the correct answer is Transient because it ensures a new instance is created every time the service is requested.

#### Why can't a scoped service be injected into a singleton in ASP.NET Core?

Scoped services depend on the request lifecycle, which does not exist for singletons.

## Entity Framework and the Configuration System

### Introduction to Entity Framework Core

#### The Need For Object-Relational Mapping (O/RM)

Problems:
- Need to learn new language
- Need a lot of additional data-access code
- Error prone
- Need to manually keep C# models in sync with DB tables

Flow:
1. Translate Web API request to SQL query
2. Send SQL query to database server
3. Read resulting database rows
4. Translate database rows to Web API response

#### What is Object-Relational Mapping (O/RM)?

A technique for converting data between a relational database and an object-oriented program

#### What is Entity Framework Core?

A lightweight, extensible, open source and cross-platform object-relational mapper for .NET

Benefits:
- No need to learn a new language
- Minimal data-access code (LINQ)
- Tooling to keep C# models in sync with DB tables
- Change tracking

Flow:
1. REST API sends C# code to Entity Framework Core
2. Entity Framework Core sends SQL statements to Database
3. Database returns resulting data to Entity Framework Core
4. Entity Framework Core returns resulting objects to REST API

### Preparing the data model for EF Core

Before adding EF Core database support, add some changes to the data model e.g. adding relationship between Game and Genre.

We need a foreign key from Game into Genre.

So whenever we create a Game, it has to have a valid Genre that actually exists in the database.

In `Game.cs`:
```csharp
// old
public required Genre Genre { get; set; } // even though this is enough for EF core to infer the relationship, it's better to do the below

// new
public Genre? Genre { get; set; } // nullable: for performance, we may not want to load the genres along with the games
public Guid GenreId { get; set; } // FK: add this explicit foreign key, so EF core will automatically populate Genre for the Game
```

Then `GetGameEndpoint` will have a warning:
```csharp
// old
game.Genre.Id, // warning: 'Genre' may be null here.

// new
game.GenreId,
```

In `GetGamesEndpoint` we can use null forgiveness operator because in `GameStoreData` we always assign a Genre to a Game.
```csharp
// old
game.Genre.Name,

// new
game.Genre!.Name,
```

Also, update `GameStoreData` to add the new `GenreId` field to the `Game`:
```csharp
    public GameStoreData()
    {
        games =
        [
            new Game {
                Id = Guid.NewGuid(),
                Name = "Street Fighter II",
                Genre = genres[0],
                GenreId = genres[0].Id,
                ...
```

Update `CreateGameEndpoint` to add the new `GenreId` field to the `Game`:
```csharp
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
                GenreId = genre.Id,
                ...
```

Update `UpdateGameEndpoint` to add the new `GenreId` field to the `existingGame`:
```csharp
// add
existingGame.GenreId = genre.Id;
```

### Creating the DBContext