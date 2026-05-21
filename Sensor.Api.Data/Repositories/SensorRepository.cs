using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly ISensorDbContext _sensorDbContext;

    public SensorRepository(ISensorDbContext sensorDbContext)
    {
        _sensorDbContext = sensorDbContext;
    }

    public async Task<IReadOnlyList<SensorQR>> GetByControllerIdAsync(int controllerId)
    {
        const string sql = """
            SELECT
                "Id",
                "ControllerId",
                "SensorKey",
                "Name",
                "SensorType",
                "IsActive",
                "CreatedUtc"
            FROM "Sensors"
            WHERE "ControllerId" = @ControllerId
            ORDER BY "Name";
            """;

        using var connection = _sensorDbContext.CreateConnection();

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
                "Id",
                "ControllerId",
                "SensorKey",
                "Name",
                "SensorType",
                "IsActive",
                "CreatedUtc"
            FROM "Sensors"
            WHERE "Id" = @Id;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<SensorQR>(sql, new
        {
            Id = id
        });
    }
}