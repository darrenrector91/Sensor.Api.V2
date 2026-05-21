# Sensor.Api

ASP.NET Core backend API for the ESP32/SHT35 sensor dashboard.

This API receives environmental sensor readings from ESP32-based controller devices, stores the data in PostgreSQL, and provides endpoints for the Angular dashboard to display controller, sensor, temperature, humidity, and status information.

## Project Purpose

`Sensor.Api` is the backend service for the sensor dashboard system. It is designed to support a controller/sensor structure where each controller can own one or more sensors, and each sensor can report environmental readings over time.

The project is built to keep the backend clean, maintainable, and easy to expand as the system grows.

## Technology Stack

- C# / .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Dapper
- Npgsql
- Raspberry Pi deployment target
- ESP32/SHT35 sensor input
- Angular frontend consumer

## Solution Structure

```text
Sensor.Api/
├── Sensor.Api.Core/
├── Sensor.Api.Data/
│   ├── Enums/
│   ├── QueryResults/
│   ├── Repositories/
│   │   └── Interfaces/
│   ├── ISensorDbContext.cs
│   └── SensorDbContext.cs
├── Sensor.Api.Web/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   │   └── Interfaces/
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
└── Sensor.Api.slnx
```

## Project Layers

### Sensor.Api.Core

Shared code that is not specific to the web API or database layer.

This project is intended for common utilities, constants, shared interfaces, and reusable logic.

### Sensor.Api.Data

Database access layer.

This project handles PostgreSQL connections, Dapper queries, repositories, repository interfaces, and query result models.

`QueryResults` is used for repository-specific return models. Query result models should use the `QR` suffix for clarity.

Examples:

```text
ControllerDashboardQR.cs
SensorReadingQR.cs
SensorStatusQR.cs
```

### Sensor.Api.Web

The ASP.NET Core API project.

This project contains API startup configuration, controllers, API-facing models, and services used by the Angular frontend and ESP32 devices.

## Database Approach

This project does not use Entity Framework Core.

Database access is handled through Dapper and Npgsql. Database tables, schema changes, and SQL are managed manually.

## Development Workflow

The intended workflow is:

```text
Local IDE development
Commit to Git branch
Push to GitHub
Deploy/pull on Raspberry Pi
Restart API service
```

The Raspberry Pi should be treated as the deployment target, not the primary development environment.

## Build

From the solution root:

```bash
dotnet build
```

## Run Locally

From the solution root:

```bash
dotnet run --project Sensor.Api.Web
```

## Configuration

The API expects a PostgreSQL connection string named `Default`.

Example:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=sensor_data;Username=sensor_api;Password=your_password"
  },
  "AllowedHosts": "*"
}
```

Production secrets should not be committed to source control.

## Planned API Areas

- API status endpoint
- Controller device endpoints
- Sensor device endpoints
- Sensor reading ingestion
- Dashboard data endpoints
- Latest reading endpoints
- Historical reading endpoints

## Current Goal

The current development focus is establishing a clean backend foundation using a multi-project .NET structure and Dapper-based PostgreSQL access before adding the full controller/sensor data model.
