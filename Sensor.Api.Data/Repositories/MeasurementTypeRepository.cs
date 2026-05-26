using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public sealed class MeasurementTypeRepository : IMeasurementTypeRepository
{
    private readonly ISensorDbContext sensorDbContext;

    public MeasurementTypeRepository(ISensorDbContext sensorDbContext)
    {
        this.sensorDbContext = sensorDbContext;
    }

    public async Task<IEnumerable<MeasurementTypeQR>> GetMeasurementTypesAsync()
    {
        const string sql = """
            SELECT
                "Id",
                "Name",
                "DisplayName",
                "DefaultUnit",
                "Icon",
                "DisplayKind",
                "Priority",
                "CssClass",
                "AccentColor",
                "Description",
                "IsActive",
                "CreatedUtc"
            FROM public."MeasurementTypes"
            WHERE "IsActive" = TRUE
            ORDER BY "Priority", "DisplayName";
            """;

        using var connection = sensorDbContext.CreateConnection();

        return await connection.QueryAsync<MeasurementTypeQR>(sql);
    }

    public async Task<MeasurementTypeQR> CreateMeasurementTypeAsync(CreateMeasurementTypeRequestQR request)
    {
        const string sql = """
            INSERT INTO public."MeasurementTypes"
            (
                "Name",
                "DisplayName",
                "DefaultUnit",
                "Icon",
                "DisplayKind",
                "Priority",
                "CssClass",
                "AccentColor",
                "Description"
            )
            VALUES
            (
                @Name,
                @DisplayName,
                @DefaultUnit,
                @Icon,
                @DisplayKind,
                @Priority,
                @CssClass,
                @AccentColor,
                @Description
            )
            RETURNING
                "Id",
                "Name",
                "DisplayName",
                "DefaultUnit",
                "Icon",
                "DisplayKind",
                "Priority",
                "CssClass",
                "AccentColor",
                "Description",
                "IsActive",
                "CreatedUtc";
            """;

        using var connection = sensorDbContext.CreateConnection();

        return await connection.QuerySingleAsync<MeasurementTypeQR>(sql, request);
    }
}