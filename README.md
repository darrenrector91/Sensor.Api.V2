# Sensor.Api

ASP.NET Core backend API for collecting, storing, and serving generalized measurements from sensor controllers.

`Sensor.Api` is the backend for a controller/sensor/measurement platform. It receives measurements from controller devices, stores them in PostgreSQL, and exposes API endpoints for dashboards, administrative tooling, and device integration.

The current hardware implementation uses an ESP32 controller with an SHT35 temperature/humidity sensor, but the backend is intentionally designed to support many controller types, sensor types, and measurement types over time.

## Architecture

The system is built around this hierarchy:

```text
Controller
└── Sensor
    └── Measurement
```

A controller represents a physical device or data source, such as an ESP32.

A sensor represents a device attached to or associated with a controller, such as an SHT35.

A measurement represents a single reported value from a sensor, such as temperature, humidity, soil moisture, voltage, lux, water level, or battery percentage.

The project has intentionally moved away from a hardcoded temperature/humidity reading model. Measurements are stored generically so that new sensor data types can be added without changing the database schema.

## Current Status

The backend foundation is operational and stabilized.

The current working flow is:

```text
ESP32/SHT35
    └── POST measurement
        └── SensorMeasurements table
            └── DashboardRepository
                └── Dashboard API endpoint
```

The ESP32 firmware has been updated and verified to post generalized measurements successfully.

The SHT35 currently submits temperature and humidity as separate measurement rows for the same sensor.

Example rows:

```text
Temperature | 19.44 | C
Humidity    | 56.08 | %
```

## Technology Stack

- C# / .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Dapper
- Npgsql
- Swagger
- Raspberry Pi deployment target
- ESP32 controller firmware
- SHT35 as the current first sensor
- Angular frontend planned next
- PlatformIO for ESP32 firmware
- Arduino framework for ESP32 firmware

## Solution Structure

```text
Sensor.Api/
├── Sensor.Api.Core/
├── Sensor.Api.Data/
│   ├── Enums/
│   ├── QueryResults/
│   ├── Repositories/
│   │   └── Interfaces/
│   ├── Sql/
│   │   └── Schema/
│   │       └── CreateSensorSchema.sql
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

This project handles PostgreSQL connections, Dapper queries, repositories, repository interfaces, SQL schema files, and query result models.

Repositories currently in use:

```text
ControllerRepository
SensorRepository
SensorMeasurementRepository
DashboardRepository
```

Repository interfaces currently in use:

```text
IControllerRepository
ISensorRepository
ISensorMeasurementRepository
IDashboardRepository
```

`QueryResults` is used for repository-specific return models. Query result models should use the `QR` suffix for clarity.

### Sensor.Api.Web

ASP.NET Core API layer.

This project contains startup configuration, controllers, API-facing models, and services used by controller devices, dashboard clients, and future administration screens.

## Database Approach

This project does not use Entity Framework Core for active data access.

Database access is handled through Dapper and Npgsql. Database tables, schema changes, and SQL are managed manually.

Current PostgreSQL database:

```text
Database: sensor_data
User:     sensor_api
```

When connecting locally on the Raspberry Pi, use TCP localhost authentication:

```bash
psql -h 127.0.0.1 -U sensor_api -d sensor_data
```

Avoid this form unless peer authentication is intentionally configured:

```bash
psql -U sensor_api -d sensor_data
```

## Current Database Tables

Active tables:

```text
Controllers
Sensors
SensorMeasurements
```

Legacy table that may still exist:

```text
SensorReadings
```

Abandoned direction:

```text
SensorReadingsV2
```

## SensorMeasurements Table

The generalized measurement table uses this structure:

```text
Id
SensorId
MeasurementType
Value
Unit
CreatedUtc
```

Temperature and humidity are not dedicated columns. They are stored as separate generalized measurement records.

Example:

```text
SensorId | MeasurementType | Value | Unit | CreatedUtc
1        | Temperature     | 22.6  | C    | ...
1        | Humidity        | 49.1  | %    | ...
```

## Current API Endpoints

### Status

```http
GET /api/status
GET /api/status/database
```

### Controllers

```http
GET /api/controllers
```

### Sensors

```http
GET /api/controllers/{controllerId}/sensors
```

### Measurements

```http
POST /api/sensors/{sensorId}/measurements
GET /api/sensors/{sensorId}/measurements
GET /api/sensors/{sensorId}/measurements/latest
```

### Dashboard

```http
GET /api/dashboard/measurements
```

## Measurement Ingestion

Controller devices post one measurement per request.

Endpoint:

```http
POST /api/sensors/{sensorId}/measurements
```

Temperature example:

```json
{
  "measurementType": "Temperature",
  "value": "22.6",
  "unit": "C"
}
```

Humidity example:

```json
{
  "measurementType": "Humidity",
  "value": "49.1",
  "unit": "%"
}
```

The current ESP32/SHT35 firmware sends two requests per reporting cycle:

```text
Temperature
Humidity
```

## ESP32 Firmware Integration

The ESP32 firmware has been updated from the old reading endpoint to the current generalized measurement endpoint.

Old endpoint, no longer used:

```http
POST /api/readings
```

Old payload shape, no longer used:

```json
{
  "deviceId": "esp32-01",
  "temperatureC": 22.6,
  "humidityPercent": 49.1
}
```

Current endpoint:

```http
POST /api/sensors/{sensorId}/measurements
```

Current firmware behavior:

- Reads temperature and humidity from the SHT35.
- Posts temperature as one measurement.
- Posts humidity as one measurement.
- Uses a hardcoded `sensorId` for now.
- Reports once per hour in the current production-like setting.

Current firmware interval:

```cpp
const unsigned long postIntervalMs = 3600000;
```

Current temporary limitation:

```cpp
const int sensorId = 1;
```

Hardcoding the database sensor ID is acceptable for the first working version, but should eventually be replaced by a provisioning or registration flow.

## Networking Notes

During development, the ESP32 was reachable at:

```text
192.168.4.34
```

The local development API was reached from the ESP32 at:

```text
http://192.168.4.28:5278
```

The ESP32 status endpoint confirmed successful posting:

```json
{
  "lastApiError": "",
  "lastApiResponseCode": 200,
  "lastApiResponseBody": "{\"id\":128,\"status\":\"created\"}"
}
```

Example PlatformIO OTA settings:

```ini
upload_protocol = espota
upload_port = 192.168.4.34

upload_flags =
  --host_ip=192.168.4.28
  --auth=stevie
  --timeout=90
```

For Raspberry Pi deployment, the API should run on the Pi and the ESP32 `baseApiUrl` should point to the Pi's reachable LAN address.

## Running Locally

From the solution root:

```bash
dotnet run --project Sensor.Api.Web --no-launch-profile --urls http://0.0.0.0:5278
```

Using `0.0.0.0` allows the API to listen on all network interfaces, which is required for controller devices to reach it over the LAN.

Verify local access:

```bash
curl -v http://localhost:5278/api/status
```

Verify LAN access:

```bash
curl -v http://<machine-lan-ip>:5278/api/status
```

Example:

```bash
curl -v http://192.168.4.28:5278/api/status
```

## Running on Raspberry Pi

Backend project path:

```text
~/projects/Sensor.Api
```

Swagger URL when running locally on the host:

```text
http://localhost:5278/swagger
```

For direct testing:

```bash
cd ~/projects/Sensor.Api
dotnet run --project Sensor.Api.Web --no-launch-profile --urls http://0.0.0.0:5278
```

Verify the API is listening:

```bash
ss -ltnp | grep 5278
```

A good result should show the API listening on all interfaces:

```text
0.0.0.0:5278
```

or:

```text
*:5278
```

If it only listens on `127.0.0.1`, external devices such as the ESP32 will not be able to reach it.

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

## Build

From the solution root:

```bash
dotnet build
```

## Git Workflow

Typical workflow:

```text
Create feature branch
Build and test
Push branch
Merge to main
Pull main
Delete feature branch
```

The ESP32 generalized measurement work used this branch name:

```bash
feature/esp32-generalized-measurement-posts
```

Suggested commit message:

```bash
git commit -m "Update ESP32 to post generalized sensor measurements"
```

## Completed Work

- Refactored backend away from the old hardcoded `SensorReadings` temperature/humidity model.
- Standardized architecture on `Controller -> Sensor -> Measurement`.
- Added generalized `SensorMeasurements` storage.
- Added measurement ingestion endpoint.
- Added latest measurement endpoint.
- Added dashboard measurement endpoint.
- Verified POST measurement flow through Swagger/API.
- Updated ESP32 firmware to post generalized measurements.
- Verified ESP32 OTA update path.
- Verified SHT35 reads are working.
- Verified ESP32 posts reach the API successfully.
- Verified rows are inserted into PostgreSQL.

## Remaining Cleanup Items

- Remove old `SensorReadings` table if it is no longer needed.
- Remove old Entity Framework migration artifacts if they are no longer relevant.
- Remove stale references to `/api/readings`.
- Remove stale references to `deviceId` where the new model should use sensor, controller, or provisioning identifiers.
- Decide whether hourly reporting is the final production interval.
- Decide when to replace hardcoded `sensorId` values in firmware.

## Recommended Next Steps

### 1. Raspberry Pi API Deployment

Move from local Mac-hosted development back to the Raspberry Pi deployment target.

The Pi-hosted API should listen on:

```text
http://0.0.0.0:5278
```

Then update the ESP32 firmware so `baseApiUrl` points to the Pi's reachable LAN IP.

### 2. Angular Dashboard

Build the Angular dashboard against:

```http
GET /api/dashboard/measurements
```

Initial UI goals:

- Display latest measurements.
- Group measurements by controller and sensor.
- Show measurement type, value, unit, and timestamp.
- Add simple refresh or polling.
- Add charting later.

### 3. Admin UI

Add Angular forms for:

- Creating controllers.
- Creating sensors.
- Pairing sensors to controllers.
- Viewing assigned sensor IDs.

### 4. Provisioning Improvement

The current ESP32 firmware uses a hardcoded database `sensorId`.

A better long-term design would be:

```text
Controller has a stable controller code
Sensor has a stable local sensor code
API resolves those codes to database IDs
```

Possible future payload shape:

```json
{
  "controllerCode": "esp32-greenhouse-01",
  "sensorCode": "sht35-main",
  "measurementType": "Temperature",
  "value": "22.6",
  "unit": "C"
}
```

This would avoid hardcoding database IDs in firmware.

### 5. Historical Data Endpoints

Add pagination and filtering for historical data.

Potential filters:

- Sensor ID
- Controller ID
- Measurement type
- Start UTC
- End UTC
- Page size
- Page number

### 6. Real Sensor Visualization

Add charts for temperature and humidity first.

Later chart candidates:

- Soil moisture
- pH
- voltage
- lux
- water level
- battery percentage

## Design Notes

The most important design decision is the move from:

```text
Sensor -> Reading
```

to:

```text
Sensor -> Measurement
```

The old model was simpler for one SHT35 sensor, but it would not scale cleanly as new measurement types were added.

The generalized model is the right foundation for a growing sensor platform because new measurement types can be added as data instead of schema changes.
