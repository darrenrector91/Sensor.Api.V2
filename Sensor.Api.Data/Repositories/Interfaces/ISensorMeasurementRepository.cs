using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface ISensorMeasurementRepository
{
    Task<long> CreateAsync(
        int sensorId,
        string measurementType,
        string value,
        string unit);

    Task<SensorMeasurementQR?> GetLatestBySensorIdAsync(
        int sensorId);

    Task<IReadOnlyList<SensorMeasurementQR>> GetBySensorIdAsync(
        int sensorId);
}