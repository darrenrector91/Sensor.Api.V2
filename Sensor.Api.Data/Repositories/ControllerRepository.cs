using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class ControllerRepository : IControllerRepository
{
    private readonly ISensorDbContext _sensorDbContext;

    public ControllerRepository(ISensorDbContext sensorDbContext)
    {
        _sensorDbContext = sensorDbContext;
    }

    public async Task<IReadOnlyList<ControllerQR>> GetAllAsync()
    {
        const string sql = """
            SELECT
                "Id",
                "ControllerKey",
                "Name",
                "Location",
                "IsActive",
                "CreatedUtc"
            FROM "Controllers"
            ORDER BY "Name";
            """;

        using var connection = _sensorDbContext.CreateConnection();

        var controllers = await connection.QueryAsync<ControllerQR>(sql);

        return controllers.ToList();
    }

    public async Task<ControllerQR?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                "Id",
                "ControllerKey",
                "Name",
                "Location",
                "IsActive",
                "CreatedUtc"
            FROM "Controllers"
            WHERE "Id" = @Id;
            """;

        using var connection = _sensorDbContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<ControllerQR>(
            sql,
            new { Id = id });
    }
}