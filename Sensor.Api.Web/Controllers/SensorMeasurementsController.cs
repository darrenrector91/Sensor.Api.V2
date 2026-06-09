using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Web.Models;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/sensors/{sensorId:int}/measurements")]
public class SensorMeasurementsController : ControllerBase
{
    private readonly ISensorMeasurementService sensorMeasurementService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorMeasurementsController"/> class.
    /// </summary>
    /// <param name="sensorMeasurementService">The sensor measurement service used to manage sensor measurements.</param>
    public SensorMeasurementsController(ISensorMeasurementService sensorMeasurementService)
    {
        this.sensorMeasurementService = sensorMeasurementService;
    }

    /// <summary>
    /// Gets measurements for a specific sensor within an optional time range.
    /// </summary>
    /// <param name="sensorId">The sensor identifier.</param>
    /// <param name="fromUtc">The optional start of the time range in UTC.</param>
    /// <param name="toUtc">The optional end of the time range in UTC.</param>
    /// <param name="limit">The optional maximum number of measurements to return.</param>
    /// <returns>An <see cref="IActionResult"/> containing the measurements.</returns>
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

    /// <summary>
    /// Gets the latest measurement for a specific sensor.
    /// </summary>
    /// <param name="sensorId">The sensor identifier.</param>
    /// <returns>An <see cref="IActionResult"/> containing the latest measurement or NotFound.</returns>
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

    /// <summary>
    /// Creates a measurement for a specific sensor.
    /// </summary>
    /// <param name="sensorId">The sensor identifier.</param>
    /// <param name="request">The measurement creation request.</param>
    /// <returns>An <see cref="IActionResult"/> with the created measurement result or bad request details.</returns>
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