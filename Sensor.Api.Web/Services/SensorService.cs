using Sensor.Api.Core.Requests;
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
        var sensors = await sensorRepository.GetSensorsByControllerIdAsync(controllerId);

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

    public async Task<int> CreateAsync(CreateSensorRequest request)
    {
        return await sensorRepository.CreateAsync(request);
    }

    public async Task<bool> UpdateSensorAsync(int id, UpdateSensorRequest request)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Sensor id is required.");
        }

        if (request.ControllerId <= 0)
        {
            throw new ArgumentException("ControllerId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.HardwareModel))
        {
            throw new ArgumentException("HardwareModel is required.");
        }

        if (request.MeasurementTypeIds.Count == 0)
        {
            throw new ArgumentException("At least one measurement type is required.");
        }

        if (request.MeasurementIntervalSeconds <= 0)
        {
            throw new ArgumentException("MeasurementIntervalSeconds must be greater than zero.");
        }

        return await sensorRepository.UpdateSensorAsync(id, request);
    }

    private static SensorResponse MapToResponse(SensorQR sensor)
    {
        return new SensorResponse
        {
            Id = sensor.Id,
            ControllerId = sensor.ControllerId,
            LocationId = sensor.LocationId,
            LocationName = sensor.LocationName,
            Name = sensor.Name,
            HardwareModel = sensor.HardwareModel,
            Description = sensor.Description,
            CommunicationProtocol = sensor.CommunicationProtocol,
            Address = sensor.Address,
            MeasurementIntervalSeconds = sensor.MeasurementIntervalSeconds,
            Notes = sensor.Notes,
            IsActive = sensor.IsActive,
            CreatedUtc = sensor.CreatedUtc
        };
    }

    public async Task<IEnumerable<SensorMeasurementTypeQR>> GetMeasurementTypesBySensorIdAsync(int sensorId)
    {
        if (sensorId <= 0)
        {
            throw new ArgumentException("SensorId is required.");
        }

        return await sensorRepository.GetMeasurementTypesBySensorIdAsync(sensorId);
    }
}