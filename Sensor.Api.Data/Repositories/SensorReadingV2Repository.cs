using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class SensorReadingV2Repository : ISensorReadingV2Repository
{
    private readonly ISensorDbContext _sensorDbContext;

    public SensorReadingV2Repository(ISensorDbContext sensorDbContext)
    {
        _sensorDbContext = sensorDbContext;
    }

    public async Task<long> CreateAsync(
        int sensorId,
        decimal? temperatureC,
        decimal? humidityPercent)
    {
        const string sql = """
            INSERT INTO "SensorReadingsV2"
            (
                "SensorId",
                "TemperatureC",
                "HumidityPercent",
                "CreatedUtc"
            )
            VALUES
            (
                @SensorId,
                @TemperatureC,
                @HumidityPercent,
                @CreatedUtc
            )
            RETURNING "Id";
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.ExecuteScalarAsync<long>(
            sql,
            new
            {
                SensorId = sensorId,
                TemperatureC = temperatureC,
                HumidityPercent = humidityPercent,
                CreatedUtc = DateTime.UtcNow
            });
    }

    public async Task<SensorReadingV2QR?> GetLatestBySensorIdAsync(int sensorId)
    {
        const string sql = """
            SELECT
                "Id",
                "SensorId",
                "TemperatureC",
                "HumidityPercent",
                "CreatedUtc"
            FROM "SensorReadingsV2"
            WHERE "SensorId" = @SensorId
            ORDER BY "CreatedUtc" DESC
            LIMIT 1;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<SensorReadingV2QR>(
            sql,
            new
            {
                SensorId = sensorId
            });
    }
}