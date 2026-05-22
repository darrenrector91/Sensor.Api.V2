using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/sensors/{sensorId:int}/measurements")]
public class SensorMeasurementsController : ControllerBase
{
    private readonly ISensorMeasurementRepository _sensorMeasurementRepository;

    public SensorMeasurementsController(
        ISensorMeasurementRepository sensorMeasurementRepository)
    {
        _sensorMeasurementRepository = sensorMeasurementRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMeasurements(
        int sensorId)
    {
        var measurements =
            await _sensorMeasurementRepository
                .GetBySensorIdAsync(sensorId);

        var response =
            measurements.Select(measurement =>
                new SensorMeasurementResponse
                {
                    Id = measurement.Id,
                    SensorId = measurement.SensorId,
                    MeasurementType = measurement.MeasurementType,
                    Value = measurement.Value,
                    Unit = measurement.Unit,
                    CreatedUtc = measurement.CreatedUtc
                });

        return Ok(response);
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestMeasurement(
        int sensorId)
    {
        var measurement =
            await _sensorMeasurementRepository
                .GetLatestBySensorIdAsync(sensorId);

        if (measurement is null)
        {
            return NotFound(new
            {
                Status = "not_found",
                Message = "No measurements found."
            });
        }

        return Ok(new SensorMeasurementResponse
        {
            Id = measurement.Id,
            SensorId = measurement.SensorId,
            MeasurementType = measurement.MeasurementType,
            Value = measurement.Value,
            Unit = measurement.Unit,
            CreatedUtc = measurement.CreatedUtc
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeasurement(
    int sensorId,
    CreateMeasurementRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.MeasurementType))
        {
            return BadRequest(new
            {
                Status = "error",
                Message = "MeasurementType is required."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Value))
        {
            return BadRequest(new
            {
                Status = "error",
                Message = "Value is required."
            });
        }

        var id = await _sensorMeasurementRepository.CreateAsync(
            sensorId,
            request.MeasurementType,
            request.Value,
            request.Unit);

        return Ok(new
        {
            Id = id,
            Status = "created"
        });
    }
}