

CREATE TABLE IF NOT EXISTS "Sensors"
(
    "Id" integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    "ControllerId" integer NOT NULL,
    "LocationId" integer NULL,
    "Name" text NOT NULL,
    "HardwareModel" text NOT NULL,
    "Description" text NOT NULL DEFAULT '',
    "CommunicationProtocol" text NOT NULL DEFAULT '',
    "Address" text NULL,
    "MeasurementIntervalSeconds" integer NOT NULL DEFAULT 300,
    "Notes" text NOT NULL DEFAULT '',
    "IsActive" boolean NOT NULL DEFAULT true,
    "CreatedUtc" timestamp without time zone NOT NULL DEFAULT NOW()
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

CREATE TABLE IF NOT EXISTS "SensorMeasurementTypes"
(
    "SensorId" integer NOT NULL,
    "MeasurementTypeId" integer NOT NULL,

    CONSTRAINT "PK_SensorMeasurementTypes"
        PRIMARY KEY ("SensorId", "MeasurementTypeId"),

    CONSTRAINT "FK_SensorMeasurementTypes_Sensors"
        FOREIGN KEY ("SensorId")
        REFERENCES "Sensors" ("Id")
        ON DELETE CASCADE,

    CONSTRAINT "FK_SensorMeasurementTypes_MeasurementTypes"
        FOREIGN KEY ("MeasurementTypeId")
        REFERENCES "MeasurementTypes" ("Id")
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Sensors_ControllerId"
    ON "Sensors" ("ControllerId");

CREATE INDEX IF NOT EXISTS "IX_SensorMeasurements_SensorId_CreatedUtc"
    ON "SensorMeasurements" ("SensorId", "CreatedUtc" DESC);