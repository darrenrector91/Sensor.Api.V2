using System.Data;

namespace Sensor.Api.Data;

public interface IDbContext
{
    IDbConnection CreateConnection();
}