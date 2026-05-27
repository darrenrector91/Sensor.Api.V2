using Sensor.Api.Data.QueryResults;
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
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<SensorResponse?> GetSensorByIdAsync(int id)
    {
        var sensor = await sensorRepository.GetByIdAsync(id);

        if (sensor is null)
        {
            return null;
        }

        return MapToResponse(sensor);
    }

    public async Task<int> CreateSensorAsync(CreateSensorQR request)
    {
        return await sensorRepository.CreateAsync(request);
    }

    public async Task<bool> UpdateSensorAsync(int id, UpdateSensorQR request)
    {
        return await sensorRepository.UpdateAsync(id, request);
    }

    private static SensorResponse MapToResponse(SensorQR sensor)
    {
        return new SensorResponse
        {
            Id = sensor.Id,
            ControllerId = sensor.ControllerId,
            LocationId = sensor.LocationId,
            Location = sensor.Location,
            SensorKey = sensor.SensorKey,
            Name = sensor.Name,
            SensorType = sensor.SensorType,
            IsActive = sensor.IsActive,
            CreatedUtc = sensor.CreatedUtc
        };
    }
}