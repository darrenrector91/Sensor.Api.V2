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
            LocationId = measurement.LocationId,
            LocationName = measurement.LocationName,
            SensorId = measurement.SensorId,
            SensorName = measurement.SensorName,
            HardwareModel = measurement.HardwareModel,
            MeasurementTypeId = measurement.MeasurementTypeId,
            MeasurementType = measurement.MeasurementType,
            MeasurementDisplayName = measurement.MeasurementDisplayName,
            Value = measurement.Value,
            Unit = measurement.Unit,
            Icon = measurement.Icon,
            Color = measurement.Color,
            DisplayStyle = measurement.DisplayStyle,
            ChartGroup = measurement.ChartGroup,
            Priority = measurement.Priority,
            CssClass = measurement.CssClass,
            CreatedUtc = measurement.CreatedUtc
        };
    }
}