using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Web.Models;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/sensors/{sensorId:int}/measurements")]
public class SensorMeasurementsController : ControllerBase
{
    private readonly ISensorMeasurementService sensorMeasurementService;

    public SensorMeasurementsController(ISensorMeasurementService sensorMeasurementService)
    {
        this.sensorMeasurementService = sensorMeasurementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMeasurements(
        int sensorId,
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int? limit)
    {
        var measurements = await sensorMeasurementService.GetMeasurementsAsync(
            sensorId,
            fromUtc,
            toUtc,
            limit);

        return Ok(measurements);
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestMeasurement(int sensorId)
    {
        var measurement = await sensorMeasurementService.GetLatestMeasurementAsync(sensorId);

        if (measurement is null)
        {
            return NotFound(new
            {
                Status = "not_found",
                Message = "No measurements found."
            });
        }

        return Ok(measurement);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeasurement(
        int sensorId,
        CreateMeasurementRequest request)
    {
        try
        {
            var id = await sensorMeasurementService.CreateMeasurementAsync(
                sensorId,
                request);

            return Ok(new
            {
                Id = id,
                Status = "created"
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                Status = "error",
                Message = exception.Message
            });
        }
    }
}