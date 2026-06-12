# Video-Game-Store-Pro (.NET 8)

Building a ASP.NET Core (.NET 8) web application backend for a video game store in VS code.

This is a more polished version of this project: [here](https://github.com/ting11222001/Video-Game-Store)

## Table of Contents

- [Demo](#demo)
- [Getting Started](#getting-started)
- [Key Concepts](#key-concepts)

## Demo

In progress.

## Getting Started

Clone the repo.

Check if the .NET SDK is installed:

```
dotnet --list-sdks
```

To run up the app, run up the backend:

```
cd GameStore.Api
dotnet run
```

Make sure the local url of the backend is updated into frontend's `vite.config.ts`.

Then, run up frontend:

```
cd GameStore.React
npm run dev
```

## Key Concepts

- REST API Design
- Data Transfer Objects (DTOs)
- CRUD endpoints
- extension methods
- route groups
- Handling invalid inputs
- Entity Framework Core
- Defining the data model
- Using the ASP.NET Core configuration system
- Generating the database and seeding data
- Understanding dependency injection and service lifetimes
- Saving new entities to the database
- Mapping entities to DTOs
- Querying, updating, and deleting entities from the database
- Using the asynchronous programming model
- API integration with the frontend
