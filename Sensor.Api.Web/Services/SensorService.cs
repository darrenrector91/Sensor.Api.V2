using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Services;

public sealed class SensorService : ISensorService
{
    private readonly ISensorRepository sensorRepository;

    public SensorService(ISensorRepository sensorRepository)
    {
        this.sensorRepository = sensorRepository;
    }

    public async Task<IReadOnlyList<SensorResponse>> GetSensorsByControllerIdAsync(int controllerId)
    {
        var sensors = await sensorRepository.GetByControllerIdAsync(controllerId);

        return sensors
            .Select(sensor => new SensorResponse
            {
                Id = sensor.Id,
                ControllerId = sensor.ControllerId,
                SensorKey = sensor.SensorKey,
                Name = sensor.Name,
                SensorType = sensor.SensorType,
                IsActive = sensor.IsActive,
                CreatedUtc = sensor.CreatedUtc
            })
            .ToList();
    }
}
