using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/readings")]
public class ReadingsController : ControllerBase
{
    private readonly ISensorReadingRepository _sensorReadingRepository;

    public ReadingsController(ISensorReadingRepository sensorReadingRepository)
    {
        _sensorReadingRepository = sensorReadingRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReading(CreateSensorReadingRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceId))
        {
            return BadRequest(new
            {
                Status = "error",
                Message = "DeviceId is required."
            });
        }

        var id = await _sensorReadingRepository.CreateAsync(
            request.DeviceId,
            request.TemperatureC,
            request.HumidityPercent);

        return CreatedAtAction(nameof(GetAllReadings), new { id }, new
        {
            Id = id,
            Status = "created"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReadings()
    {
        var readings = await _sensorReadingRepository.GetAllAsync();

        var response = readings.Select(reading => new SensorReadingResponse
        {
            Id = reading.Id,
            DeviceId = reading.DeviceId,
            TemperatureC = reading.TemperatureC,
            HumidityPercent = reading.HumidityPercent,
            CreatedUtc = reading.CreatedUtc
        });

        return Ok(response);
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestReading()
    {
        var reading = await _sensorReadingRepository.GetLatestAsync();

        if (reading is null)
        {
            return NotFound(new
            {
                Status = "not_found",
                Message = "No sensor readings were found."
            });
        }

        return Ok(new SensorLatestReadingResponse
        {
            DeviceId = reading.DeviceId,
            TemperatureC = reading.TemperatureC,
            HumidityPercent = reading.HumidityPercent,
            CreatedUtc = reading.CreatedUtc
        });
    }
}