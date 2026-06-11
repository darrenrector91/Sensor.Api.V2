using Sensor.Api.Core.Requests;
using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface ISensorRepository
{
    Task<IReadOnlyList<SensorQR>> GetSensorsByControllerIdAsync(int controllerId);

    Task<SensorQR?> GetByIdAsync(int id);

    Task<int> CreateAsync(CreateSensorRequest request);

    Task<IEnumerable<SensorQR>> GetByControllerIdAsync(int controllerId);

    Task<bool> UpdateSensorAsync(int id, UpdateSensorRequest request);

    Task<IEnumerable<SensorMeasurementTypeQR>> GetMeasurementTypesBySensorIdAsync(int sensorId);
}