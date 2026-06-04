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