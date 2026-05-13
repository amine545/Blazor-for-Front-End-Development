# EventEase

Blazor Server app for browsing, registering, and tracking attendance for events.

## Run

```bash
dotnet run
```

App: http://localhost:5080

## Features

- **Event Card** component (`Components/Shared/EventCard.razor`) with two-way data binding (`@bind`, `EventCallback<EventModel>`).
- **Routing** between Events list (`/`), Event Details (`/events/{id}`), Registration (`/register`, `/register/{id}`), Attendance (`/attendance`), and Sign in (`/signin`).
- **Registration Form** with `DataAnnotations` validation (required, email, phone, terms acceptance).
- **State management** via scoped `UserSessionService` exposing `OnChange` event.
- **Attendance Tracker** keyed by event with present/absent toggle and counts.
- **Performance**: `<Virtualize>` for event list, scoped services, minimal re-renders, input validation.

## Project structure

```
EventEase/
  Components/
    Layout/        MainLayout, NavMenu
    Pages/         Home, EventDetails, Register, Attendance, SignIn, Error
    Shared/        EventCard
    App.razor, Routes.razor, _Imports.razor
  Models/          EventModel, RegistrationModel, Attendance
  Services/        EventService, UserSessionService, AttendanceService
  wwwroot/         app.css, bootstrap loader
  Program.cs       DI + endpoints
```

## Copilot assistance summary

### Activity 1 - Foundation
- **Event Card**: Generated initial markup, fields, and `EventCallback<EventModel>` binding pattern.
- **Routing**: Suggested `@page` directives with route constraints and `NavLink` matching.
- **Validation**: Authored `DataAnnotations` attributes (`[Required]`, `[EmailAddress]`, `[Range]` for boolean accept-terms).
- **State management**: Proposed the singleton/scoped split for `EventService` vs `UserSessionService` and the `OnChange` pattern.
- **Attendance Tracker**: Drafted toggle logic and reactive count rendering.

### Activity 2 - Debug & Optimize
- **Event Card validation**: Refactored to use `EditForm` + `EditContext` + `DataAnnotationsValidator` so invalid/empty inputs are caught before propagating; added `ValidityChanged` callback so parents can disable Save.
- **Routing**: Added a `NotFound` template with `PageTitle`, friendly 404 markup, and a guarded "event not found" view in `EventDetails` to handle invalid ids gracefully.
- **Performance**:
  - Seeded 200 events to verify large-dataset rendering.
  - `<Virtualize>` with `OverscanCount` and `Placeholder` template avoids full-list rendering.
  - Search input is debounced (200 ms) to prevent excessive re-filtering.
  - Service mutations are guarded with `lock` for thread-safe access in a Blazor Server scenario.
  - Components implement `IDisposable` to release subscriptions and cancellation tokens.

### Activity 3 - Advanced features & Deployment
- **Registration Form**: Full `DataAnnotations` validation (name, email, phone, terms acceptance) with `<ValidationSummary>` and per-field messages; rejects unknown event ids; auto-fills name/email from the session.
- **User Session Tracker** (`/session`): tracks login time, last activity, live session duration, page views and registered events; reactive UI via `OnChange` event and a 1-second timer.
- **Attendance Tracker**: real-time present count and attendance-rate progress bar; promoted to a singleton so attendance is shared across users.
- **Production readiness**:
  - `Dockerfile` (multi-stage build on `mcr.microsoft.com/dotnet/aspnet:8.0`).
  - Response compression enabled.
  - Global exception handler (`/Error`) and HSTS outside Development.
  - `.gitignore` / `.dockerignore` exclude build artefacts.

## Deploy

```bash
docker build -t eventease .
docker run -p 8080:8080 eventease
```
