using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface ISensorRepository
{
    Task<IReadOnlyList<SensorQR>> GetByControllerIdAsync(int controllerId);

    Task<SensorQR?> GetByIdAsync(int id);
}