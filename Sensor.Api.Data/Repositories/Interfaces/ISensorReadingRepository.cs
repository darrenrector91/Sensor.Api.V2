using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface ISensorReadingRepository
{
    Task<int> CreateAsync(string deviceId, decimal temperatureC, decimal humidityPercent);

    Task<IReadOnlyList<SensorReadingQR>> GetAllAsync();

    Task<SensorLatestReadingQR?> GetLatestAsync();
}