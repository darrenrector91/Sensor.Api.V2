CREATE TABLE IF NOT EXISTS "Controllers"
(
    "Id" SERIAL PRIMARY KEY,
    "ControllerKey" TEXT NOT NULL UNIQUE,
    "Name" TEXT NOT NULL,
    "Location" TEXT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "Sensors"
(
    "Id" SERIAL PRIMARY KEY,
    "ControllerId" INTEGER NOT NULL,
    "SensorKey" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "SensorType" TEXT NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),

    CONSTRAINT "FK_Sensors_Controllers_ControllerId"
        FOREIGN KEY ("ControllerId")
        REFERENCES "Controllers" ("Id"),

    CONSTRAINT "UQ_Sensors_ControllerId_SensorKey"
        UNIQUE ("ControllerId", "SensorKey")
);

CREATE TABLE IF NOT EXISTS "SensorMeasurements"
(
    "Id" BIGSERIAL PRIMARY KEY,
    "SensorId" INTEGER NOT NULL,
    "MeasurementType" TEXT NOT NULL,
    "Value" TEXT NOT NULL,
    "Unit" TEXT NOT NULL,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),

    CONSTRAINT "FK_SensorMeasurements_Sensors_SensorId"
        FOREIGN KEY ("SensorId")
        REFERENCES "Sensors" ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_Sensors_ControllerId"
    ON "Sensors" ("ControllerId");

CREATE INDEX IF NOT EXISTS "IX_SensorMeasurements_SensorId_CreatedUtc"
    ON "SensorMeasurements" ("SensorId", "CreatedUtc" DESC);