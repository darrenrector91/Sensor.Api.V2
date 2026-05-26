using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Services;

public sealed class DashboardService : IDashboardService
{
    private readonly IDashboardRepository dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        this.dashboardRepository = dashboardRepository;
    }

    public async Task<IReadOnlyList<DashboardMeasurementResponse>> GetMeasurementsAsync()
    {
        var measurements = await dashboardRepository.GetMeasurementsAsync();

        return measurements
            .Select(MapToResponse)
            .ToList();
    }

    private static DashboardMeasurementResponse MapToResponse(DashboardMeasurementQR measurement)
    {
        return new DashboardMeasurementResponse
        {
            ControllerId = measurement.ControllerId,
            ControllerKey = measurement.ControllerKey,
            ControllerName = measurement.ControllerName,
            Location = measurement.Location,
            SensorId = measurement.SensorId,
            SensorKey = measurement.SensorKey,
            SensorName = measurement.SensorName,
            SensorType = measurement.SensorType,
            MeasurementType = measurement.MeasurementType,
            Value = measurement.Value,
            Unit = measurement.Unit,
            CreatedUtc = measurement.CreatedUtc
        };
    }
}