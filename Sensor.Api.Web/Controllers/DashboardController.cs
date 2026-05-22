using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    [HttpGet("measurements")]
    public async Task<IActionResult> GetMeasurements()
    {
        var measurements = await _dashboardRepository.GetMeasurementsAsync();

        var response = measurements.Select(measurement =>
            new DashboardMeasurementResponse
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
            });

        return Ok(response);
    }
}