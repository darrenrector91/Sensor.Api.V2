using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Services;

public sealed class SensorMeasurementService : ISensorMeasurementService
{
    private readonly ISensorMeasurementRepository sensorMeasurementRepository;

    public SensorMeasurementService(ISensorMeasurementRepository sensorMeasurementRepository)
    {
        this.sensorMeasurementRepository = sensorMeasurementRepository;
    }

    public async Task<IReadOnlyList<SensorMeasurementResponse>> GetMeasurementsAsync(
        int sensorId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int? limit)
    {
        var safeLimit = Math.Clamp(limit ?? 500, 1, 5000);

        var measurements = await sensorMeasurementRepository.GetBySensorIdAsync(
            sensorId,
            fromUtc,
            toUtc,
            safeLimit);

        return measurements
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<SensorMeasurementResponse?> GetLatestMeasurementAsync(int sensorId)
    {
        var measurement = await sensorMeasurementRepository.GetLatestBySensorIdAsync(sensorId);

        return measurement is null
            ? null
            : MapToResponse(measurement);
    }

    public async Task<long> CreateMeasurementAsync(
        int sensorId,
        CreateMeasurementRequest request)
    {
        request.MeasurementType = NormalizeRequired(
            request.MeasurementType,
            "MeasurementType is required.");

        request.Value = NormalizeRequired(
            request.Value,
            "Value is required.");

        request.Unit = request.Unit.Trim();

        return await sensorMeasurementRepository.CreateAsync(
            sensorId,
            request.MeasurementType,
            request.Value,
            request.Unit);
    }

    private static SensorMeasurementResponse MapToResponse(SensorMeasurementQR measurement)
    {
        return new SensorMeasurementResponse
        {
            Id = measurement.Id,
            SensorId = measurement.SensorId,
            MeasurementType = measurement.MeasurementType,
            Value = measurement.Value,
            Unit = measurement.Unit,
            CreatedUtc = measurement.CreatedUtc
        };
    }

    private static string NormalizeRequired(
        string value,
        string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(errorMessage);
        }

        return value.Trim();
    }
}