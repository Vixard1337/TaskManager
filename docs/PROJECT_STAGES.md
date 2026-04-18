# Project Stages

This document tracks the project progress in chronological order.

## Stage 1 — Repository initialization
- Initialized Git repository
- Added initial project files
- First commit: `init`

## Stage 2 — MongoDB setup
- Added `MongoDb` section to `appsettings.json`
- Added `Configuration/MongoSettings.cs`
- Registered `IMongoClient` in `Program.cs`
- Added `MongoDB.Driver` package reference
- Added initial `README.md`

## Stage 3 — Domain and service layer
- Added models:
  - `Models/User.cs`
  - `Models/TaskItem.cs`
- Added services:
  - `Services/UserService.cs`
  - `Services/TaskService.cs`
- Registered services in DI container in `Program.cs`

## Stage 4 — Tasks list page
- Added:
  - `Pages/Tasks/Index.cshtml`
  - `Pages/Tasks/Index.cshtml.cs`
- Added `Tasks` navigation link in `Pages/Shared/_Layout.cshtml`

## Stage 5 — Users pages (MVP)
- Added:
  - `Pages/Users/Index.cshtml`
  - `Pages/Users/Index.cshtml.cs`
  - `Pages/Users/Create.cshtml`
  - `Pages/Users/Create.cshtml.cs`
- Added `Users` navigation link in `Pages/Shared/_Layout.cshtml`

## Stage 6 — Documentation refresh
- Updated `README.md` with:
  - current project status
  - run instructions
  - refreshed next steps

## Stage 7 — Task creation page
- Added:
  - `Pages/Tasks/Create.cshtml`
  - `Pages/Tasks/Create.cshtml.cs`
- Implemented task creation flow with:
  - user assignment (`UserId`)
  - comma-separated tags parsing
  - basic validation
- Updated `Pages/Tasks/Index.cshtml` with `Add task` button

## Stage 8 — Task edit and delete pages
- Added:
  - `Pages/Tasks/Edit.cshtml`
  - `Pages/Tasks/Edit.cshtml.cs`
  - `Pages/Tasks/Delete.cshtml`
  - `Pages/Tasks/Delete.cshtml.cs`
- Implemented task update flow with:
  - loading current task by `id`
  - editing title, description, tags, and assigned user
  - basic validation on submit
- Implemented task delete confirmation flow
- Updated `Pages/Tasks/Index.cshtml` with `Edit` and `Delete` actions

## Next planned stages
1. Add `Done/Undo` action on tasks
2. Add tag filtering on tasks list
3. Add screenshots and diagrams for project documentation
