using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly ISensorDbContext _sensorDbContext;

    public DashboardRepository(ISensorDbContext sensorDbContext)
    {
        _sensorDbContext = sensorDbContext;
    }

    public async Task<IReadOnlyList<DashboardMeasurementQR>> GetMeasurementsAsync()
    {
        const string sql = """
            SELECT
                c."Id" AS "ControllerId",
                c."ControllerKey",
                c."Name" AS "ControllerName",
                c."Location",
                s."Id" AS "SensorId",
                s."SensorKey",
                s."Name" AS "SensorName",
                s."SensorType",
                m."MeasurementType",
                m."Value",
                m."Unit",
                m."CreatedUtc"
            FROM "Controllers" c
            INNER JOIN "Sensors" s
                ON s."ControllerId" = c."Id"
            INNER JOIN "SensorMeasurements" m
                ON m."SensorId" = s."Id"
            WHERE c."IsActive" = TRUE
                AND s."IsActive" = TRUE
            ORDER BY m."CreatedUtc" DESC;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        var measurements = await connection.QueryAsync<DashboardMeasurementQR>(sql);

        return measurements.ToList();
    }
}