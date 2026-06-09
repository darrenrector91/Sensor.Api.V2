using Dapper;
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
                l."Name" AS "Location",
                s."SensorKey",
                s."Name",
                s."SensorType",
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
                l."Name" AS "Location",
                s."SensorKey",
                s."Name",
                s."SensorType",
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
    public async Task<int> CreateAsync(CreateSensorQR request)
    {
        const string sql = """
            INSERT INTO "Sensors" ("ControllerId", "LocationId", "SensorKey", "Name", "SensorType")
            VALUES (@ControllerId, @LocationId, @SensorKey, @Name, @SensorType)
            RETURNING "Id";
            """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, request);
    }

    /// <summary>
    /// Updates an existing sensor.
    /// </summary>
    /// <param name="id">The sensor identifier.</param>
    /// <param name="request">The updated sensor values.</param>
    /// <returns><c>true</c> if the sensor was updated; otherwise, <c>false</c>.</returns>
    public async Task<bool> UpdateAsync(int id, UpdateSensorQR request)
    {
        const string sql = """
            UPDATE "Sensors"
            SET
                "ControllerId" = @ControllerId,
                "LocationId" = @LocationId,
                "SensorKey" = @SensorKey,
                "Name" = @Name,
                "SensorType" = @SensorType,
                "IsActive" = @IsActive
            WHERE "Id" = @Id;
            """;

        using var connection = _databaseContext.CreateConnection();

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            Id = id,
            request.ControllerId,
            request.LocationId,
            request.SensorKey,
            request.Name,
            request.SensorType,
            request.IsActive
        });

        return rowsAffected > 0;
    }
}