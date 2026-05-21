using System.Data;

namespace Sensor.Api.Data;

public interface ISensorDbContext
{
    IDbConnection CreateConnection();
}
