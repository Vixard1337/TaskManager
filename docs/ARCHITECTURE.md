# Architecture Overview

## Project type
- `ASP.NET Core Razor Pages` application
- Data source: `MongoDB`
- Target framework: `.NET 10`

## High-level structure
- `Configuration/` — typed configuration classes
- `Models/` — domain/data models
- `Services/` — MongoDB data access logic
- `Pages/` — Razor Pages UI (`.cshtml` + `PageModel`)

## Design patterns used

### 1. Dependency Injection (DI)
Services are registered in `Program.cs` and injected where needed.

Examples:
- `IMongoClient`
- `UserService`
- `TaskService`

### 2. Options Pattern
MongoDB configuration is mapped from `appsettings.json` into a strongly typed class.

Files:
- `Configuration/MongoSettings.cs`
- `Program.cs` (`Configure<MongoSettings>(...)`)

### 3. Service Layer
Database operations are encapsulated in dedicated service classes.

Files:
- `Services/UserService.cs`
- `Services/TaskService.cs`

Benefits:
- separation of concerns
- reusable data access logic
- cleaner page models

### 4. Razor Pages PageModel pattern
Each page keeps UI markup in `.cshtml` and request logic in `.cshtml.cs`.

Examples:
- `Pages/Tasks/Index.cshtml` + `Pages/Tasks/Index.cshtml.cs`
- `Pages/Tasks/Create.cshtml` + `Pages/Tasks/Create.cshtml.cs`
- `Pages/Tasks/Edit.cshtml` + `Pages/Tasks/Edit.cshtml.cs`
- `Pages/Tasks/Delete.cshtml` + `Pages/Tasks/Delete.cshtml.cs`
- `Pages/Users/Create.cshtml` + `Pages/Users/Create.cshtml.cs`

### 5. DataAnnotations validation
Input constraints are declared in model classes and evaluated through `ModelState` in page handlers.

Examples:
- `Models/User.cs`
- `Models/TaskItem.cs`

## Current flow
1. Request hits Razor Page endpoint
2. `PageModel` uses injected service
3. Service executes MongoDB operation
4. Data is returned to the Razor Page view

## Write flow example (task creation)
1. User opens `Pages/Tasks/Create`
2. `CreateModel` loads users via `UserService`
3. User submits form (`Title`, `Description`, `UserId`, `TagsInput`)
4. `CreateModel` validates input and parses tags
5. `TaskService` writes the new `TaskItem` to MongoDB
6. Request is redirected to `Pages/Tasks/Index`

## Update/Delete flow example (task maintenance)
1. User clicks `Edit` or `Delete` on `Pages/Tasks/Index`
2. `PageModel` loads current task by `id` from `TaskService`
3. Edit path: user updates values and submits form
4. `TaskService.UpdateAsync(...)` saves updated document in MongoDB
5. Delete path: user confirms removal
6. `TaskService.DeleteAsync(...)` removes document from MongoDB

## Status toggle flow example (Done/Undo)
1. User clicks `Done` or `Undo` on `Pages/Tasks/Index`
2. `IndexModel.OnPostToggleDoneAsync(string id)` loads task by `id`
3. `TaskService.SetDoneAsync(...)` updates `IsDone` in MongoDB
4. Request is redirected to refreshed `Pages/Tasks/Index`

## Read flow example (extended filtering)
1. User submits GET filters on `Pages/Tasks/Index` (`Tag`, `Title`, `Status`)
2. `IndexModel` loads base data set (all tasks or by tag)
3. Additional in-memory filters are applied (`Title`, `Status`)
4. User display names are resolved through `UserService` lookup
5. Filtered list is rendered in the same index view

## Notes for future development
- Keep business/data access logic in `Services/`, not in page markup
- Keep project progress updated in `docs/PROJECT_STAGES.md`
- Update this file when new architectural decisions are introduced

## Documentation artifacts
- `docs/DIAGRAMS.md` — draft class and use case diagrams (Mermaid)
- `docs/SCREENSHOTS.md` — checklist and naming convention for report screenshots
