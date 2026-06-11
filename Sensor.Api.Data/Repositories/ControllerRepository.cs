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

    /// <summary>
    /// Gets all controllers with their resolved location name and active sensor count.
    /// </summary>
    /// <returns>
    /// A read-only list of controllers ordered by controller name.
    /// </returns>
    public async Task<IReadOnlyList<ControllerQR>> GetAllControllersAsync()
    {
        const string sql = """
            SELECT
                c."Id",
                c."ControllerKey",
                c."Name",
                c."LocationId",
                l."Name" AS "LocationName",
                c."IsActive",
                c."CreatedUtc",
                COUNT(s."Id") AS "SensorCount"
            FROM "Controllers" c
            LEFT JOIN "Locations" l
                ON l."Id" = c."LocationId"
            LEFT JOIN "Sensors" s
                ON s."ControllerId" = c."Id"
                AND s."IsActive" = TRUE
            GROUP BY
                c."Id",
                c."ControllerKey",
                c."Name",
                c."LocationId",
                l."Name",
                c."IsActive",
                c."CreatedUtc"
            ORDER BY c."Name";
        """;

        using var connection = _databaseContext.CreateConnection();

        var controllers = await connection.QueryAsync<ControllerQR>(sql);

        return controllers.ToList();
    }
    /// <summary>
    /// Gets the next controller key sequence number for the specified location.
    /// </summary>
    /// <param name="id">The location identifier used to scope the controller key sequence.</param>
    /// <returns>
    /// The next available controller key sequence number for the location.
    /// </returns>
    public async Task<int> GetControllerKey(int id)
    {
        // Extracts the trailing number from each ControllerKey, finds the highest value,
        // defaults to 0 when no matching controllers exist, then adds 1 for the next sequence number.
        // The lookup is limited to controllers belonging to the specified LocationId.
        const string sql = """
            SELECT COALESCE(
                MAX(CAST(SUBSTRING("ControllerKey" FROM '^[a-z0-9-]+-([0-9]+)$') AS INTEGER)),
                0
            ) + 1
            FROM public."Controllers"
            WHERE "LocationId" = @Id;
            """;

        using var connection = _databaseContext.CreateConnection();

        var result = await connection.ExecuteScalarAsync<int?>(sql, new { Id = id });

        return result ?? 0;
    }

    /// <summary>
    /// Gets the highest existing controller key sequence number for the specified location.
    /// </summary>
    /// <param name="locationId">The location identifier used to scope the controller key sequence lookup.</param>
    /// <returns>
    /// The highest existing controller key sequence number for the location, or zero when none exist.
    /// </returns>
    public async Task<int> GetNextControllerSequenceNumberAsync(int locationId)
    {
        const string sql = """
        SELECT COALESCE(
            MAX(
                CAST(
                    SUBSTRING(c."ControllerKey" FROM '-([0-9]+)$')
                    AS INTEGER
                )
            ),
            0
        )
        FROM public."Controllers" c
        WHERE c."LocationId" = @LocationId;
        """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, new { LocationId = locationId });
    }

    // Backwards-compatible shim for callers using the previous method name.
    public Task<int> GetNextControllerKeySequenceNumberAsync(int locationId)
        => GetNextControllerSequenceNumberAsync(locationId);

    /// <summary>
    /// Gets a single controller by identifier, including its resolved location name and active sensor count.
    /// </summary>
    /// <param name="id">The controller identifier.</param>
    /// <returns>
    /// The matching controller when found; otherwise, null.
    /// </returns>
    public async Task<ControllerQR?> GetControllerByIdAsync(int id)
    {
        const string sql = """
            SELECT
                c."Id",
                c."ControllerKey",
                c."Name",
                c."LocationId",
                l."Name" AS "LocationName",
                c."IsActive",
                c."CreatedUtc",
                COUNT(s."Id") AS "SensorCount"
            FROM "Controllers" c
            LEFT JOIN "Locations" l
                ON l."Id" = c."LocationId"
            LEFT JOIN "Sensors" s
                ON s."ControllerId" = c."Id"
                AND s."IsActive" = TRUE
            WHERE c."Id" = @Id
            GROUP BY
                c."Id",
                c."ControllerKey",
                c."Name",
                c."LocationId",
                l."Name",
                c."IsActive",
                c."CreatedUtc";
            """;

        using var connection = _databaseContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<ControllerQR>(
            sql,
            new { Id = id });
    }

    /// <summary>
    /// Creates a new controller and returns its generated identifier.
    /// </summary>
    /// <param name="request">The controller values to create.</param>
    /// <returns>
    /// The generated controller identifier.
    /// </returns>
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

    /// <summary>
    /// Updates an existing controller.
    /// </summary>
    /// <param name="id">The controller identifier.</param>
    /// <param name="request">The updated controller values.</param>
    /// <returns>
    /// True when the controller was updated; otherwise, false.
    /// </returns>
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