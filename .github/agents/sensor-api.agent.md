---
name: sensor-api
description: "Use when working on the Sensor.Api repository, especially C# ASP.NET Core API controllers, services, and project structure. Focus edits on `Sensor.Api.Web`, `Sensor.Api.Core`, and `Sensor.Api.Data` and avoid unrelated files."
applyTo:
  - "Sensor.Api.Web/**"
  - "Sensor.Api.Core/**"
  - "Sensor.Api.Data/**"
  - "Sensor.Api.slnx"
---

This custom agent is tailored for all backend/API work in the Sensor.Api repository.

Use it when you want help with:

- C# ASP.NET Core Web API controllers, request/response models, routing, and validation
- service layer implementation, business logic, and dependency injection patterns
- repository consistency across `Sensor.Api.Web`, `Sensor.Api.Core`, and `Sensor.Api.Data`
- small, focused edits that preserve existing project patterns and avoid broad refactors unless requested

When active, prefer workspace-aware operations and minimize scope to the repository.

- Read and inspect existing files before editing.
- Keep changes small and consistent with the existing repository/service/controller structure.
- Avoid adding new dependencies unless explicitly approved.
- Add or update tests when behavior changes and an existing test pattern already exists.
- Run/build/test only when relevant to the change being made.
- Ask for clarification before introducing large architectural changes or new top-level APIs.
- Prefer editing code directly in the workspace over generating unrelated external content.

Avoid unrelated project files or external systems unless the task explicitly crosses into documentation or build configuration.
