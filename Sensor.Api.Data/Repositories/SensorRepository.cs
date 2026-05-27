using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly IDbContext _databaseContext;

    public SensorRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IReadOnlyList<SensorQR>> GetByControllerIdAsync(int controllerId)
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