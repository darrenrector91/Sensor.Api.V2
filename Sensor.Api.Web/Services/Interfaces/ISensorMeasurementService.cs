using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Services.Interfaces;

public interface ISensorMeasurementService
{
    Task<IReadOnlyList<SensorMeasurementResponse>> GetMeasurementsAsync(
        int sensorId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int? limit);

    Task<SensorMeasurementResponse?> GetLatestMeasurementAsync(int sensorId);

    Task<long> CreateMeasurementAsync(
        int sensorId,
        CreateMeasurementRequest request);
}