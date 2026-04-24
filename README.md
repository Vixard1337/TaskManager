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
  - `Pages/Users/Edit` (edit user)
  - `Pages/Users/Delete` (delete confirmation)
- Added Razor Page for tasks:
  - `Pages/Tasks/Index` (admin list + filter by tag mode/title+description/status/user + sorting + Done/Undo)
  - `Pages/Tasks/Create` (create task with user assignment and tags)
  - `Pages/Tasks/Edit` (edit task)
  - `Pages/Tasks/Delete` (delete confirmation)
- Modernized app layout and styling:
  - refreshed `Pages/Shared/_Layout.cshtml`
  - updated global theme in `wwwroot/css/site.css`
  - dashboard-style home page in `Pages/Index.cshtml`
  - scrollable task table container for large datasets
- Added simple admin authentication (cookie login):
  - `Pages/Account/Login`
  - protected pages require authentication
  - credentials validated against MongoDB collection `AdminUsers`
  - password hashing via `ASP.NET Core PasswordHasher`
  - lockout after multiple failed login attempts
  - secure cookie settings and sliding session expiration
  - auto password rehash and unique admin username index
- Added project documentation assets:
  - `docs/PROJECT_STAGES.md`
  - `docs/ARCHITECTURE.md`
  - `docs/DIAGRAMS.md`
  - `docs/SCREENSHOTS.md`
- Added consistency guard:
  - users assigned to tasks cannot be deleted until tasks are reassigned/removed

## How to run

1. Start MongoDB (recommended via Docker Compose):

`docker compose up -d`

2. Verify MongoDB settings in `appsettings.json`.
3. Run the application:

`dotnet run`

4. Open the app and sign in:

`/Account/Login`

Bootstrap credentials from `appsettings.json` (used to seed the first admin account):
- Username: `admin`
- Password: `admin123`

Config section:

`AdminBootstrap`

5. Use admin pages:
   - `/Users/Index`
   - `/Tasks/Index`

5. Optional: stop MongoDB container when finished:

`docker compose down`

## MongoDB Compass (connection)

1. Open `MongoDB Compass`.
2. Use connection string:

`mongodb://localhost:27017`

3. Click `Connect`.
4. Open database `TaskManagerDb` and collections:
   - `Users`
   - `Tasks`
   - `AdminUsers`

## MongoDB configuration

Configuration is stored in `appsettings.json`:

- `MongoDb:ConnectionString`
- `MongoDb:DatabaseName`
- `MongoDb:UsersCollectionName`
- `MongoDb:TasksCollectionName`

Default local connection:

`mongodb://localhost:27017`

## Next steps

1. Capture screenshots from implemented pages
2. Export class and use case diagrams for report
3. Final report assembly (sections 1–8)
