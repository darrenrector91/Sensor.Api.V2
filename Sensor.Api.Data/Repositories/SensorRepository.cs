using Dapper;
using Sensor.Api.Core.Requests;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly IDbContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorRepository"/> class.
    /// </summary>
    /// <param name="databaseContext">The database context used to create connections.</param>
    public SensorRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Gets all sensors for a specific controller.
    /// </summary>
    /// <param name="controllerId">The controller identifier.</param>
    /// <returns>A read-only list of matching sensor query results.</returns>
    public async Task<IReadOnlyList<SensorQR>> GetSensorsByControllerIdAsync(int controllerId)
    {
        const string sql = """
            SELECT
                s."Id",
                s."ControllerId",
                s."LocationId",
                l."Name" AS "LocationName",
                s."Name",
                s."HardwareModel",
                s."Description",
                s."CommunicationProtocol",
                s."Address",
                s."MeasurementIntervalSeconds",
                s."Notes",
                s."IsActive",
                s."CreatedUtc"
            FROM "Sensors" s
            LEFT JOIN "Locations" l ON l."Id" = s."LocationId"
            WHERE s."ControllerId" = @ControllerId
            ORDER BY s."Name";
            """;

        using var connection = _databaseContext.CreateConnection();

        var sensors = await connection.QueryAsync<SensorQR>(sql, new
        {
            ControllerId = controllerId
        });

        return sensors.ToList();
    }

    /// <summary>
    /// Gets a sensor by its identifier.
    /// </summary>
    /// <param name="id">The sensor identifier.</param>
    /// <returns>The matching sensor query result, or <c>null</c> if not found.</returns>
    public async Task<SensorQR?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                s."Id",
                s."ControllerId",
                s."LocationId",
                l."Name" AS "LocationName",
                s."Name",
                s."HardwareModel",
                s."Description",
                s."CommunicationProtocol",
                s."Address",
                s."MeasurementIntervalSeconds",
                s."Notes",
                s."IsActive",
                s."CreatedUtc"
            FROM "Sensors" s
            LEFT JOIN "Locations" l ON l."Id" = s."LocationId"
            WHERE s."Id" = @Id;
            """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<SensorQR>(sql, new
        {
            Id = id
        });
    }

    /// <summary>
    /// Creates a new sensor and returns the generated identifier.
    /// </summary>
    /// <param name="request">The sensor creation request.</param>
    /// <returns>The identifier of the newly created sensor.</returns>

    public async Task<int> CreateAsync(CreateSensorRequest request)
    {
        // Inserts the physical sensor record and returns the generated sensor Id.
        const string insertSensorSql = """
        INSERT INTO "Sensors"
        (
            "ControllerId",
            "LocationId",
            "Name",
            "HardwareModel",
            "Description",
            "CommunicationProtocol",
            "Address",
            "MeasurementIntervalSeconds",
            "Notes",
            "IsActive",
            "CreatedUtc"
        )
        VALUES
        (
            @ControllerId,
            @LocationId,
            @Name,
            @HardwareModel,
            @Description,
            @CommunicationProtocol,
            @Address,
            @MeasurementIntervalSeconds,
            @Notes,
            @IsActive,
            NOW()
        )
        RETURNING "Id";
        """;

        // Inserts each supported measurement type for the newly created sensor.
        // Example: one SHT35 sensor may support both Temperature and Humidity.
        const string insertMeasurementTypeSql = """
        INSERT INTO "SensorMeasurementTypes"
        (
            "SensorId",
            "MeasurementTypeId"
        )
        VALUES
        (
            @SensorId,
            @MeasurementTypeId
        );
        """;

        // Create a database connection using the shared database context.
        using var connection = _databaseContext.CreateConnection();

        // Open the connection before starting the transaction.
        connection.Open();

        // Use a transaction so the sensor and its measurement type mappings are saved together.
        using var transaction = connection.BeginTransaction();

        try
        {
            // Insert the sensor row and capture the generated sensor Id.
            var sensorId = await connection.ExecuteScalarAsync<int>(
                insertSensorSql,
                request,
                transaction);

            // Insert one SensorMeasurementTypes row for each distinct measurement type.
            foreach (var measurementTypeId in request.MeasurementTypeIds.Distinct())
            {
                await connection.ExecuteAsync(
                    insertMeasurementTypeSql,
                    new
                    {
                        SensorId = sensorId,
                        MeasurementTypeId = measurementTypeId
                    },
                    transaction);
            }

            // Commit only after both the sensor and all measurement type mappings succeed.
            transaction.Commit();

            // Return the new sensor Id to the service/controller layer.
            return sensorId;
        }
        catch
        {
            // Roll back the entire operation if any insert fails.
            transaction.Rollback();

            // Re-throw so the service/API layer can handle or log the failure.
            throw;
        }
    }

    public async Task<IEnumerable<SensorQR>> GetByControllerIdAsync(int controllerId)
    {
        const string sql = """
            SELECT
                "Id",
                "ControllerId",
                "LocationId",
                "Name",
                "HardwareModel",
                "Description",
                "CommunicationProtocol",
                "Address",
                "MeasurementIntervalSeconds",
                "Notes",
                "IsActive",
                "CreatedUtc"
            FROM "Sensors"
            WHERE "ControllerId" = @ControllerId
            ORDER BY "Name";
            """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.QueryAsync<SensorQR>(
            sql,
            new
            {
                ControllerId = controllerId
            });
    }

    /// <summary>
    /// Updates the sensor hardware record, clears its existing measurement type mappings,
    /// and inserts the updated measurement type mappings inside a single transaction.
    /// </summary>
    /// <param name="id">The sensor identifier.</param>
    /// <param name="request">The updated sensor details, including supported measurement type identifiers.</param>
    /// <returns>
    /// True when the sensor was updated successfully; false when no sensor exists for the supplied identifier.
    /// </returns>
    public async Task<bool> UpdateSensorAsync(int id, UpdateSensorRequest request)
    {
        const string updateSensorSql = """
        UPDATE "Sensors"
        SET
            "ControllerId" = @ControllerId,
            "LocationId" = @LocationId,
            "Name" = @Name,
            "HardwareModel" = @HardwareModel,
            "Description" = @Description,
            "CommunicationProtocol" = @CommunicationProtocol,
            "Address" = @Address,
            "MeasurementIntervalSeconds" = @MeasurementIntervalSeconds,
            "Notes" = @Notes,
            "IsActive" = @IsActive
        WHERE "Id" = @Id;
        """;

        const string deleteMeasurementTypesSql = """
        DELETE FROM "SensorMeasurementTypes"
        WHERE "SensorId" = @SensorId;
        """;

        const string insertMeasurementTypeSql = """
        INSERT INTO "SensorMeasurementTypes"
        (
            "SensorId",
            "MeasurementTypeId"
        )
        VALUES
        (
            @SensorId,
            @MeasurementTypeId
        );
        """;

        using var connection = _databaseContext.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var affectedRows = await connection.ExecuteAsync(
                updateSensorSql,
                new
                {
                    Id = id,
                    request.ControllerId,
                    request.LocationId,
                    request.Name,
                    request.HardwareModel,
                    request.Description,
                    request.CommunicationProtocol,
                    request.Address,
                    request.MeasurementIntervalSeconds,
                    request.Notes,
                    request.IsActive
                },
                transaction);

            if (affectedRows == 0)
            {
                transaction.Rollback();
                return false;
            }

            await connection.ExecuteAsync(
                deleteMeasurementTypesSql,
                new
                {
                    SensorId = id
                },
                transaction);

            foreach (var measurementTypeId in request.MeasurementTypeIds.Distinct())
            {
                await connection.ExecuteAsync(
                    insertMeasurementTypeSql,
                    new
                    {
                        SensorId = id,
                        MeasurementTypeId = measurementTypeId
                    },
                    transaction);
            }

            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<SensorMeasurementTypeQR>> GetMeasurementTypesBySensorIdAsync(int sensorId)
    {
        const string sql = """
        SELECT
            smt."SensorId",
            mt."Id" AS "MeasurementTypeId",
            mt."Name",
            mt."DisplayName",
            mt."DefaultUnit",
            mt."Icon",
            mt."DisplayKind",
            mt."Priority",
            mt."CssClass",
            mt."AccentColor",
            mt."Description",
            mt."IsActive",
            mt."CreatedUtc",
            mt."Color",
            mt."DisplayStyle",
            mt."ChartGroup"
        FROM "SensorMeasurementTypes" smt
        INNER JOIN "MeasurementTypes" mt ON mt."Id" = smt."MeasurementTypeId"
        WHERE smt."SensorId" = @SensorId
        ORDER BY mt."Priority", mt."DisplayName";
        """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.QueryAsync<SensorMeasurementTypeQR>(
            sql,
            new
            {
                SensorId = sensorId
            });
    }
}