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
- `Pages/Users/Create.cshtml` + `Pages/Users/Create.cshtml.cs`

## Current flow
1. Request hits Razor Page endpoint
2. `PageModel` uses injected service
3. Service executes MongoDB operation
4. Data is returned to the Razor Page view

## Notes for future development
- Keep business/data access logic in `Services/`, not in page markup
- Keep project progress updated in `docs/PROJECT_STAGES.md`
- Update this file when new architectural decisions are introduced
