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