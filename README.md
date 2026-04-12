# TaskManager

TaskManager is a simple `ASP.NET Core Razor Pages` application for managing tasks and users, prepared for a NoSQL approach with `MongoDB`.

## Project goal

The project is designed as a student-friendly task management system that can be extended with full CRUD features, task assignment, status tracking, and tag-based filtering.

## Tech stack

- `.NET 10`
- `ASP.NET Core Razor Pages`
- `MongoDB`
- `MongoDB.Driver`

## Current setup

This commit prepares the project for MongoDB integration:

- Added `MongoDb` section in `appsettings.json`
- Added strongly typed settings class: `Configuration/MongoSettings.cs`
- Registered MongoDB settings and `IMongoClient` in `Program.cs`
- Added `MongoDB.Driver` package reference

## MongoDB configuration

Configuration is stored in `appsettings.json`:

- `MongoDb:ConnectionString`
- `MongoDb:DatabaseName`
- `MongoDb:UsersCollectionName`
- `MongoDb:TasksCollectionName`

Default local connection:

`mongodb://localhost:27017`

## Next steps

1. Add domain models (`User`, `TaskItem`)
2. Add services/repositories for MongoDB collections
3. Build Razor Pages for task/user CRUD
4. Add filtering by tags and task status updates
