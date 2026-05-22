using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/sensors/{sensorId:int}/readings")]
public class SensorReadingsController : ControllerBase
{
    private readonly ISensorReadingV2Repository _sensorReadingRepository;

    public SensorReadingsController(
        ISensorReadingV2Repository sensorReadingRepository)
    {
        _sensorReadingRepository = sensorReadingRepository;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestReading(int sensorId)
    {
        var reading = await _sensorReadingRepository
            .GetLatestBySensorIdAsync(sensorId);

        if (reading is null)
        {
            return NotFound(new
            {
                Status = "not_found",
                Message = "No readings found."
            });
        }

        return Ok(new SensorReadingV2Response
        {
            Id = reading.Id,
            SensorId = reading.SensorId,
            TemperatureC = reading.TemperatureC,
            HumidityPercent = reading.HumidityPercent,
            CreatedUtc = reading.CreatedUtc
        });
    }
}