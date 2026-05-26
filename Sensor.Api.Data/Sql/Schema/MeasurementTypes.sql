CREATE TABLE IF NOT EXISTS public."MeasurementTypes"
(
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "DisplayName" TEXT NOT NULL,
    "DefaultUnit" TEXT NULL,
    "Icon" TEXT NOT NULL,
    "DisplayKind" TEXT NOT NULL,
    "Priority" INTEGER NOT NULL,
    "CssClass" TEXT NOT NULL,
    "AccentColor" TEXT NOT NULL,
    "Description" TEXT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedUtc" TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT "UQ_MeasurementTypes_Name" UNIQUE ("Name")
);