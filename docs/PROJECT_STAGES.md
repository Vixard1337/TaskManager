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

## Stage 9 — Done/Undo action on tasks
- Extended `Pages/Tasks/Index.cshtml.cs` with `OnPostToggleDoneAsync(string id)` handler
- Added inline `Done/Undo` action button in `Pages/Tasks/Index.cshtml`
- Reused `TaskService.SetDoneAsync(...)` to update completion status in MongoDB

## Stage 10 — Tag filtering on tasks list
- Extended `Pages/Tasks/Index.cshtml.cs` with GET filter support (`Tag`)
- Added filtering logic:
  - all tasks when filter is empty
  - `TaskService.GetByTagAsync(...)` when tag is provided
- Added filter UI in `Pages/Tasks/Index.cshtml` with:
  - tag input
  - `Filter` and `Clear` actions
  - active filter indicator
- Preserved selected filter after `Done/Undo` action

## Stage 11 — Documentation assets (diagrams and screenshots)
- Added `docs/DIAGRAMS.md` with:
  - class diagram draft (User–TaskItem relation)
  - use case diagram draft (core user actions)
- Added `docs/SCREENSHOTS.md` with:
  - screenshot checklist for report section 5
  - naming convention for exported images

## Next planned stages
1. Capture screenshots and export diagrams as images for final report
