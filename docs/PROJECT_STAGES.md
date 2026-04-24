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

## Stage 12 — User display names on tasks list
- Updated `Pages/Tasks/Index.cshtml.cs` to load users via `UserService`
- Added user name lookup and display helper (`GetUserDisplayName(...)`)
- Replaced technical `UserId` column with readable full name on `Pages/Tasks/Index.cshtml`

## Stage 13 — DataAnnotations-based model validation
- Added validation attributes in models:
  - `Models/User.cs` (`[Required]`, `[StringLength]`)
  - `Models/TaskItem.cs` (`[Required]`, `[StringLength]`)
- Simplified validation in page models by relying on `ModelState`:
  - `Pages/Users/Create.cshtml.cs`
  - `Pages/Tasks/Create.cshtml.cs`
  - `Pages/Tasks/Edit.cshtml.cs`

## Stage 14 — Extended filtering on tasks list
- Extended `Pages/Tasks/Index.cshtml.cs` with additional GET filters:
  - `Status` (`all` / `done` / `notdone`)
  - `Title` search
- Updated filter UI on `Pages/Tasks/Index.cshtml`:
  - tag filter
  - title search
  - status selector
  - active filter badges
- Preserved all active filters after `Done/Undo`

## Stage 15 — Success feedback after actions
- Added success alert rendering in `Pages/Shared/_Layout.cshtml` based on `TempData["SuccessMessage"]`
- Added `[TempData] SuccessMessage` to action page models:
  - `Pages/Users/Create.cshtml.cs`
  - `Pages/Tasks/Create.cshtml.cs`
  - `Pages/Tasks/Edit.cshtml.cs`
  - `Pages/Tasks/Delete.cshtml.cs`
  - `Pages/Tasks/Index.cshtml.cs` (`Done/Undo`)
- Added success messages after create/edit/delete/toggle actions

## Stage 16 — Sorting on tasks list
- Extended `Pages/Tasks/Index.cshtml.cs` with GET sort support (`Sort`)
- Added sorting logic:
  - `status` (default: not done first, then title)
  - `titleasc`
  - `titledesc`
- Added sort selector in `Pages/Tasks/Index.cshtml`
- Preserved sort selection after `Done/Undo` action

## Stage 17 — UI polish and modernized layout
- Refreshed global layout in `Pages/Shared/_Layout.cshtml`:
  - dark gradient navbar
  - cleaner container spacing
  - styled global success alert
  - refined footer
- Reworked global styling in `wwwroot/css/site.css`:
  - modern color palette
  - elevated content surface
  - improved tables/forms/buttons
  - updated hover/focus states
- Updated `Pages/Index.cshtml` to a dashboard-style home page with quick navigation cards

## Stage 18 — Docker Compose for MongoDB
- Added `docker-compose.yml` with local MongoDB service (`mongo:7`)
- Added persistent volume (`mongo_data`) for database storage
- Updated `README.md` with:
  - Docker Compose startup/shutdown commands
  - MongoDB Compass connection guide

## Stage 19 — Layout overlap fix and action clarity
- Removed old scaffolded styles from `Pages/Shared/_Layout.cshtml.css` that were overriding the modern theme
- Eliminated absolute footer behavior causing content/footer overlap
- Improved task action readability in `Pages/Tasks/Index.cshtml`:
  - clearer spacing for action buttons
  - `Delete...` label to emphasize confirmation step
  - inline explanation of `Done/Undo` vs `Delete`

## Stage 20 — Admin capabilities extension
- Added cookie-based admin authentication and login page:
  - `Pages/Account/Login.cshtml`
  - `Pages/Account/Login.cshtml.cs`
  - protected app pages require authentication
- Added full user management pages:
  - `Pages/Users/Edit.cshtml` + `.cshtml.cs`
  - `Pages/Users/Delete.cshtml` + `.cshtml.cs`
- Extended task list filtering/sorting behavior:
  - filter by user
  - case-insensitive tag matching
  - additional user-based sorting (`userasc`, `userdesc`)
- Added internal scroll container for task table (`table-scroll-container`) to avoid full page scrolling
- Updated app naming as admin-facing panel in layout and tasks/users views

## Stage 21 — Hardened admin authentication (Mongo-based)
- Replaced plain appsettings login validation with MongoDB-backed admin accounts (`AdminUsers` collection)
- Added `Models/AdminUser.cs` for admin credentials/lockout metadata
- Added `Services/AdminAuthService.cs` with:
  - `PasswordHasher`-based credential verification
  - failed login counter
  - temporary lockout after multiple failed attempts
  - role/claims-compatible sign-in result
- Added bootstrap admin seeding on startup:
  - `Configuration/AdminBootstrapSettings.cs`
  - startup seed call in `Program.cs`
- Migrated configuration:
  - removed `AdminAuth`
  - added `AdminBootstrap` and `MongoDb:AdminUsersCollectionName`

## Next planned stages
1. Final pre-submission step: capture screenshots and export diagrams as images for the report
