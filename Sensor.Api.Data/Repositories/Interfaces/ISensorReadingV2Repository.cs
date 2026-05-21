using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface ISensorReadingV2Repository
{
    Task<long> CreateAsync(
        int sensorId,
        decimal? temperatureC,
        decimal? humidityPercent);

    Task<SensorReadingV2QR?> GetLatestBySensorIdAsync(int sensorId);
}