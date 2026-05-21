using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly ISensorDbContext _sensorDbContext;

    public SensorReadingRepository(ISensorDbContext sensorDbContext)
    {
        _sensorDbContext = sensorDbContext;
    }

    public async Task<int> CreateAsync(string deviceId, decimal temperatureC, decimal humidityPercent)
    {
        const string sql = """
            INSERT INTO "SensorReadings"
            (
                "DeviceId",
                "TemperatureC",
                "HumidityPercent",
                "CreatedUtc"
            )
            VALUES
            (
                @DeviceId,
                @TemperatureC,
                @HumidityPercent,
                @CreatedUtc
            )
            RETURNING "Id";
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            DeviceId = deviceId,
            TemperatureC = temperatureC,
            HumidityPercent = humidityPercent,
            CreatedUtc = DateTime.UtcNow
        });
    }

    public async Task<IReadOnlyList<SensorReadingQR>> GetAllAsync()
    {
        const string sql = """
            SELECT
                "Id",
                "DeviceId",
                "TemperatureC",
                "HumidityPercent",
                "CreatedUtc"
            FROM "SensorReadings"
            ORDER BY "CreatedUtc" DESC;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        var readings = await connection.QueryAsync<SensorReadingQR>(sql);

        return readings.ToList();
    }

    public async Task<SensorLatestReadingQR?> GetLatestAsync()
    {
        const string sql = """
            SELECT
                "DeviceId",
                "TemperatureC",
                "HumidityPercent",
                "CreatedUtc"
            FROM "SensorReadings"
            ORDER BY "CreatedUtc" DESC
            LIMIT 1;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<SensorLatestReadingQR>(sql);
    }
}