using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
public class SensorsController : ControllerBase
{
    private readonly ISensorService sensorService;

    public SensorsController(ISensorService sensorService)
    {
        this.sensorService = sensorService;
    }

    [HttpGet("api/sensors")]
    public async Task<IActionResult> GetSensorsByControllerId([FromQuery] int controllerId)
    {
        var sensors = await sensorService.GetSensorsByControllerIdAsync(controllerId);

        return Ok(sensors);
    }

    [HttpGet("api/sensors/{id:int}")]
    public async Task<IActionResult> GetSensorById(int id)
    {
        var sensor = await sensorService.GetSensorByIdAsync(id);

        if (sensor is null)
        {
            return NotFound();
        }

        return Ok(sensor);
    }

    [HttpPost("api/sensors")]
    public async Task<ActionResult<int>> CreateSensor(CreateSensorQR request)
    {
        var id = await sensorService.CreateSensorAsync(request);

        return CreatedAtAction(nameof(GetSensorById), new { id }, id);
    }

    [HttpPut("api/sensors/{id:int}")]
    public async Task<IActionResult> UpdateSensor(int id, UpdateSensorQR request)
    {
        var updated = await sensorService.UpdateSensorAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}