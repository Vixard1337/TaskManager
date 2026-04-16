# TaskManager

TaskManager is a simple `ASP.NET Core Razor Pages` application for managing tasks and users with a `MongoDB` backend.

## Project goal

The project is designed as a student-friendly task management system that can be extended with full CRUD features, task assignment, status tracking, and tag-based filtering.

## Tech stack

- `.NET 10`
- `ASP.NET Core Razor Pages`
- `MongoDB`
- `MongoDB.Driver`

## Current setup

The project currently includes:

- Added `MongoDb` section in `appsettings.json`
- Added strongly typed settings class: `Configuration/MongoSettings.cs`
- Registered MongoDB settings and `IMongoClient` in `Program.cs`
- Added `MongoDB.Driver` package reference
- Added domain models: `Models/User.cs`, `Models/TaskItem.cs`
- Added MongoDB services: `Services/UserService.cs`, `Services/TaskService.cs`
- Added Razor Pages for users:
  - `Pages/Users/Index` (list users)
  - `Pages/Users/Create` (create user)
- Added Razor Page for tasks:
  - `Pages/Tasks/Index` (list tasks)
- Updated navigation in `Pages/Shared/_Layout.cshtml`

## How to run

1. Start MongoDB locally (default: `mongodb://localhost:27017`).
2. Verify MongoDB settings in `appsettings.json`.
3. Run the application:

`dotnet run`

4. Open the app in browser and use:
   - `/Users/Index`
   - `/Tasks/Index`

## MongoDB configuration

Configuration is stored in `appsettings.json`:

- `MongoDb:ConnectionString`
- `MongoDb:DatabaseName`
- `MongoDb:UsersCollectionName`
- `MongoDb:TasksCollectionName`

Default local connection:

`mongodb://localhost:27017`

## Next steps

1. Add `Tasks/Create` page with user assignment
2. Add `Tasks/Edit` and `Tasks/Delete`
3. Add `Done/Undo` action for tasks
4. Add tag filtering on tasks list
