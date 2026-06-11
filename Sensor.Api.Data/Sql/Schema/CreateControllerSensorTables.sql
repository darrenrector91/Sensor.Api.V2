CREATE TABLE IF NOT EXISTS "Locations"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL DEFAULT '',
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "Controllers"
(
    "Id" SERIAL PRIMARY KEY,
    "ControllerKey" TEXT NOT NULL UNIQUE,
    "Name" TEXT NOT NULL,
    "LocationId" INTEGER NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),

    CONSTRAINT "FK_Controllers_Locations_LocationId"
        FOREIGN KEY ("LocationId")
        REFERENCES "Locations" ("Id")
);

CREATE TABLE IF NOT EXISTS "MeasurementTypes"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL UNIQUE,
    "DisplayName" TEXT NOT NULL,
    "DefaultUnit" TEXT NOT NULL,
    "Icon" TEXT NULL,
    "DisplayKind" TEXT NULL,
    "Priority" INTEGER NOT NULL DEFAULT 0,
    "CssClass" TEXT NULL,
    "AccentColor" TEXT NULL,
    "Description" TEXT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
    "Color" TEXT NULL,
    "DisplayStyle" TEXT NULL,
    "ChartGroup" TEXT NULL
);

CREATE TABLE IF NOT EXISTS "Sensors"
(
    "Id" SERIAL PRIMARY KEY,
    "ControllerId" INTEGER NOT NULL,
    "LocationId" INTEGER NULL,
    "Name" TEXT NOT NULL,
    "HardwareModel" TEXT NOT NULL,
    "Description" TEXT NOT NULL DEFAULT '',
    "CommunicationProtocol" TEXT NOT NULL DEFAULT '',
    "Address" TEXT NULL,
    "MeasurementIntervalSeconds" INTEGER NOT NULL DEFAULT 300,
    "Notes" TEXT NOT NULL DEFAULT '',
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),

    CONSTRAINT "FK_Sensors_Controllers_ControllerId"
        FOREIGN KEY ("ControllerId")
        REFERENCES "Controllers" ("Id"),

    CONSTRAINT "FK_Sensors_Locations_LocationId"
        FOREIGN KEY ("LocationId")
        REFERENCES "Locations" ("Id")
);

CREATE TABLE IF NOT EXISTS "SensorMeasurementTypes"
(
    "SensorId" INTEGER NOT NULL,
    "MeasurementTypeId" INTEGER NOT NULL,

    CONSTRAINT "PK_SensorMeasurementTypes"
        PRIMARY KEY ("SensorId", "MeasurementTypeId"),

    CONSTRAINT "FK_SensorMeasurementTypes_Sensors_SensorId"
        FOREIGN KEY ("SensorId")
        REFERENCES "Sensors" ("Id")
        ON DELETE CASCADE,

    CONSTRAINT "FK_SensorMeasurementTypes_MeasurementTypes_MeasurementTypeId"
        FOREIGN KEY ("MeasurementTypeId")
        REFERENCES "MeasurementTypes" ("Id")
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "SensorMeasurements"
(
    "Id" BIGSERIAL PRIMARY KEY,
    "SensorId" INTEGER NOT NULL,
    "MeasurementTypeId" INTEGER NOT NULL,
    "Value" NUMERIC(12, 4) NOT NULL,
    "Unit" TEXT NOT NULL,
    "CreatedUtc" TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),

    CONSTRAINT "FK_SensorMeasurements_Sensors_SensorId"
        FOREIGN KEY ("SensorId")
        REFERENCES "Sensors" ("Id"),

    CONSTRAINT "FK_SensorMeasurements_MeasurementTypes_MeasurementTypeId"
        FOREIGN KEY ("MeasurementTypeId")
        REFERENCES "MeasurementTypes" ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_Controllers_LocationId"
    ON "Controllers" ("LocationId");

CREATE INDEX IF NOT EXISTS "IX_Sensors_ControllerId"
    ON "Sensors" ("ControllerId");

CREATE INDEX IF NOT EXISTS "IX_Sensors_LocationId"
    ON "Sensors" ("LocationId");

CREATE INDEX IF NOT EXISTS "IX_SensorMeasurementTypes_MeasurementTypeId"
    ON "SensorMeasurementTypes" ("MeasurementTypeId");

CREATE INDEX IF NOT EXISTS "IX_SensorMeasurements_SensorId_CreatedUtc"
    ON "SensorMeasurements" ("SensorId", "CreatedUtc" DESC);

CREATE INDEX IF NOT EXISTS "IX_SensorMeasurements_MeasurementTypeId"
    ON "SensorMeasurements" ("MeasurementTypeId");