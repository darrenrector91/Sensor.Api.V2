using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/controllers/{controllerId:int}/sensors")]
public class SensorsController : ControllerBase
{
    private readonly ISensorRepository _sensorRepository;

    public SensorsController(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorsByControllerId(int controllerId)
    {
        var sensors = await _sensorRepository.GetByControllerIdAsync(controllerId);

        var response = sensors.Select(sensor => new SensorResponse
        {
            Id = sensor.Id,
            ControllerId = sensor.ControllerId,
            SensorKey = sensor.SensorKey,
            Name = sensor.Name,
            SensorType = sensor.SensorType,
            IsActive = sensor.IsActive,
            CreatedUtc = sensor.CreatedUtc
        });

        return Ok(response);
    }
}