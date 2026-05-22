using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class SensorMeasurementRepository : ISensorMeasurementRepository
{
    private readonly ISensorDbContext _sensorDbContext;

    public SensorMeasurementRepository(
        ISensorDbContext sensorDbContext)
    {
        _sensorDbContext = sensorDbContext;
    }

    public async Task<long> CreateAsync(
        int sensorId,
        string measurementType,
        string value,
        string unit)
    {
        const string sql = """
            INSERT INTO "SensorMeasurements"
            (
                "SensorId",
                "MeasurementType",
                "Value",
                "Unit",
                "CreatedUtc"
            )
            VALUES
            (
                @SensorId,
                @MeasurementType,
                @Value,
                @Unit,
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
                MeasurementType = measurementType,
                Value = value,
                Unit = unit,
                CreatedUtc = DateTime.UtcNow
            });
    }

    public async Task<SensorMeasurementQR?> GetLatestBySensorIdAsync(
        int sensorId)
    {
        const string sql = """
            SELECT
                "Id",
                "SensorId",
                "MeasurementType",
                "Value",
                "Unit",
                "CreatedUtc"
            FROM "SensorMeasurements"
            WHERE "SensorId" = @SensorId
            ORDER BY "CreatedUtc" DESC
            LIMIT 1;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<SensorMeasurementQR>(
            sql,
            new
            {
                SensorId = sensorId
            });
    }

    public async Task<IReadOnlyList<SensorMeasurementQR>> GetBySensorIdAsync(
        int sensorId)
    {
        const string sql = """
            SELECT
                "Id",
                "SensorId",
                "MeasurementType",
                "Value",
                "Unit",
                "CreatedUtc"
            FROM "SensorMeasurements"
            WHERE "SensorId" = @SensorId
            ORDER BY "CreatedUtc" DESC;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        var measurements =
            await connection.QueryAsync<SensorMeasurementQR>(
                sql,
                new
                {
                    SensorId = sensorId
                });

        return measurements.ToList();
    }
}