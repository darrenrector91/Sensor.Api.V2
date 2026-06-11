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
                l."Id" AS "LocationId",
                l."Name" AS "LocationName",
                s."Id" AS "SensorId",
                s."Name" AS "SensorName",
                s."HardwareModel",
                m."MeasurementTypeId",
                mt."Name" AS "MeasurementType",
                mt."DisplayName" AS "MeasurementDisplayName",
                m."Value",
                m."Unit",
                mt."Icon",
                mt."Color",
                mt."DisplayStyle",
                mt."ChartGroup",
                mt."Priority",
                mt."CssClass",
                m."CreatedUtc"
            FROM "Controllers" c
            LEFT JOIN "Locations" l
                ON l."Id" = c."LocationId"
            INNER JOIN "Sensors" s
                ON s."ControllerId" = c."Id"
            INNER JOIN "SensorMeasurements" m
                ON m."SensorId" = s."Id"
            LEFT JOIN "MeasurementTypes" mt
                ON mt."Id" = m."MeasurementTypeId"
            WHERE c."IsActive" = TRUE
                AND s."IsActive" = TRUE
                AND mt."IsActive" = TRUE
            ORDER BY m."CreatedUtc" DESC;
        """;

        using var connection = _databaseContext.CreateConnection();

        var measurements = await connection.QueryAsync<DashboardMeasurementQR>(sql);

        return measurements.ToList();

    }
}