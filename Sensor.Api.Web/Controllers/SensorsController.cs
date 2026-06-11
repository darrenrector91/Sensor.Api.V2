using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Core.Requests;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/sensors")]
public class SensorsController : ControllerBase
{
    private readonly ISensorService sensorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorsController"/> class.
    /// </summary>
    /// <param name="sensorService">The sensor service used to manage sensor data.</param>
    public SensorsController(ISensorService sensorService)
    {
        this.sensorService = sensorService;
    }

    /// <summary>
    /// Gets all sensors for the specified controller.
    /// </summary>
    /// <param name="controllerId">The controller identifier.</param>
    /// <returns>An <see cref="IActionResult"/> containing the list of sensors.</returns>
    [HttpGet]
    public async Task<IActionResult> GetSensorsByControllerId([FromQuery] int controllerId)
    {
        var sensors = await sensorService.GetSensorsByControllerIdAsync(controllerId);

        return Ok(sensors);
    }

    /// <summary>
    /// Gets a sensor by its identifier.
    /// </summary>
    /// <param name="id">The sensor identifier.</param>
    /// <returns>An <see cref="IActionResult"/> containing the sensor or a NotFound result.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSensorById(int id)
    {
        var sensor = await sensorService.GetSensorByIdAsync(id);

        if (sensor is null)
        {
            return NotFound();
        }

        return Ok(sensor);
    }

    /// <summary>
    /// Creates a new sensor.
    /// </summary>
    /// <param name="request">The sensor creation request.</param>
    /// <returns>An <see cref="ActionResult"/> with the created sensor identifier.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSensorRequest request)
    {
        var sensorId = await sensorService.CreateAsync(request);

        return Created($"/api/sensors/{sensorId}", sensorId);
    }

    /// <summary>
    /// Updates an existing sensor.
    /// </summary>
    /// <param name="id">The sensor identifier.</param>
    /// <param name="request">The updated sensor values.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the update result.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSensor(int id, UpdateSensorRequest request)
    {
        var updated = await sensorService.UpdateSensorAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}