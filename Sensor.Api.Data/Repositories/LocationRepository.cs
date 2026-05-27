using Dapper;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Data.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly IDbContext _dbContext;

    public LocationRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LocationQR>> GetLocationsAsync()
    {
        using var connection = _dbContext.CreateConnection();

        const string sql = """
            SELECT "Id", "Name", "Description", "Latitude", "Longitude", "CreatedUtc"
            FROM "Locations"
            ORDER BY "Name";
            """;

        return await connection.QueryAsync<LocationQR>(sql);
    }

    public async Task<LocationQR?> GetLocationByIdAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        const string sql = """
            SELECT "Id", "Name", "Description", "Latitude", "Longitude", "CreatedUtc"
            FROM "Locations"
            WHERE "Id" = @Id;
            """;

        return await connection.QuerySingleOrDefaultAsync<LocationQR>(sql, new { Id = id });
    }

    public async Task<int> CreateLocationAsync(CreateLocationQR request)
    {
        using var connection = _dbContext.CreateConnection();

        const string sql = """
            INSERT INTO "Locations" ("Name", "Description", "Latitude", "Longitude")
            VALUES (@Name, @Description, @Latitude, @Longitude)
            RETURNING "Id";
            """;

        return await connection.ExecuteScalarAsync<int>(sql, request);
    }

    public async Task<bool> UpdateLocationAsync(int id, UpdateLocationQR request)
    {
        using var connection = _dbContext.CreateConnection();

        const string sql = """
            UPDATE "Locations"
            SET "Name" = @Name,
                "Description" = @Description,
                "Latitude" = @Latitude,
                "Longitude" = @Longitude
            WHERE "Id" = @Id;
            """;

        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            Id = id,
            request.Name,
            request.Description,
            request.Latitude,
            request.Longitude
        });

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteLocationAsync(int id)
    {
        using var connection = _dbContext.CreateConnection();

        const string sql = """
            DELETE FROM "Locations"
            WHERE "Id" = @Id;
            """;

        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

        return rowsAffected > 0;
    }
}