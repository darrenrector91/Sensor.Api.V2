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
      int sensorId,
      DateTime? fromUtc,
      DateTime? toUtc,
      int limit)
    {
        var sql = """
        SELECT
            "Id",
            "SensorId",
            "MeasurementType",
            "Value",
            "Unit",
            "CreatedUtc"
        FROM "SensorMeasurements"
        WHERE "SensorId" = @SensorId
        """;

        var parameters = new DynamicParameters();
        parameters.Add("SensorId", sensorId);
        parameters.Add("Limit", limit);

        if (fromUtc.HasValue)
        {
            sql += """
            
              AND "CreatedUtc" >= @FromUtc
            """;

            parameters.Add("FromUtc", fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            sql += """
            
              AND "CreatedUtc" <= @ToUtc
            """;

            parameters.Add("ToUtc", toUtc.Value);
        }

        sql += """
        
        ORDER BY "CreatedUtc" DESC
        LIMIT @Limit;
        """;

        using var connection = _sensorDbContext.CreateConnection();

        var measurements =
            await connection.QueryAsync<SensorMeasurementQR>(
                sql,
                parameters);

        return measurements
            .OrderBy(measurement => measurement.CreatedUtc)
            .ToList();
    }
}