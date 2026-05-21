using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Sensor.Api.Data;

public sealed class SensorDbContext : ISensorDbContext
{
    private readonly string _connectionString;

    public SensorDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' was not found.");
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
