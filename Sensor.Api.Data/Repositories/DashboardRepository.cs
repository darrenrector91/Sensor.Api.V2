using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly IDbContext _databaseContext;

    public DashboardRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IReadOnlyList<DashboardMeasurementQR>> GetMeasurementsAsync()
    {
        const string sql = """
        SELECT
            c."Id" AS "ControllerId",
            c."ControllerKey",
            c."Name" AS "ControllerName",
            l."Name" AS "Location",
            s."Id" AS "SensorId",
            s."SensorKey",
            s."Name" AS "SensorName",
            s."SensorType",
            m."MeasurementType",
            m."Value",
            m."Unit",
            mt."Icon" AS "Icon",
            mt."Color" AS "Color",
            mt."DisplayStyle" AS "DisplayStyle",
            mt."ChartGroup" AS "ChartGroup",
            mt."Priority" AS "Priority",
            mt."CssClass" AS "CssClass",
            m."CreatedUtc"
        FROM "Controllers" c
        LEFT JOIN "Locations" l
            ON l."Id" = c."LocationId"
        INNER JOIN "Sensors" s
            ON s."ControllerId" = c."Id"
        INNER JOIN "SensorMeasurements" m
            ON m."SensorId" = s."Id"
        LEFT JOIN "MeasurementTypes" mt
            ON mt."Name" = m."MeasurementType"
        WHERE c."IsActive" = TRUE
            AND s."IsActive" = TRUE
        ORDER BY m."CreatedUtc" DESC;
        """;

        using var connection = _databaseContext.CreateConnection();

        var measurements = await connection.QueryAsync<DashboardMeasurementQR>(sql);

        return measurements.ToList();

    }
}