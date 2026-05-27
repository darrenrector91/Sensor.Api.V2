using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class ControllerRepository : IControllerRepository
{
    private readonly IDbContext _databaseContext;

    public ControllerRepository(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IReadOnlyList<ControllerQR>> GetAllAsync()
    {
        const string sql = """
            SELECT
                c."Id",
                c."ControllerKey",
                c."Name",
                l."Name" AS "Location",
                c."LocationId",
                c."IsActive",
                c."CreatedUtc"
            FROM "Controllers" c
            LEFT JOIN "Locations" l ON l."Id" = c."LocationId"
            ORDER BY c."Name";
            """;

        using var connection = _databaseContext.CreateConnection();

        var controllers = await connection.QueryAsync<ControllerQR>(sql);

        return controllers.ToList();
    }

    public async Task<ControllerQR?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                c."Id",
                c."ControllerKey",
                c."Name",
                l."Name" AS "Location",
                c."LocationId",
                c."IsActive",
                c."CreatedUtc"
            FROM "Controllers" c
            LEFT JOIN "Locations" l ON l."Id" = c."LocationId"
            WHERE c."Id" = @Id;
            """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<ControllerQR>(
            sql,
            new { Id = id });
    }

    public async Task<int> CreateAsync(CreateControllerQR request)
    {
        const string sql = """
            INSERT INTO "Controllers" ("ControllerKey", "Name", "LocationId")
            VALUES (@ControllerKey, @Name, @LocationId)
            RETURNING "Id";
            """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, request);
    }

    public async Task<bool> UpdateAsync(int id, UpdateControllerQR request)
    {
        const string sql = """
            UPDATE "Controllers"
            SET
                "ControllerKey" = @ControllerKey,
                "Name" = @Name,
                "LocationId" = @LocationId,
                "IsActive" = @IsActive
            WHERE "Id" = @Id;
            """;

        using var connection = _databaseContext.CreateConnection();

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            Id = id,
            request.ControllerKey,
            request.Name,
            request.LocationId,
            request.IsActive
        });

        return rowsAffected > 0;
    }
}