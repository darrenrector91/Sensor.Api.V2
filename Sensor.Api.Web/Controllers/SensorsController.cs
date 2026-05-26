using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/controllers/{controllerId:int}/sensors")]
public class SensorsController : ControllerBase
{
    private readonly ISensorService sensorService;

    public SensorsController(ISensorService sensorService)
    {
        this.sensorService = sensorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorsByControllerId(int controllerId)
    {
        var sensors = await sensorService.GetSensorsByControllerIdAsync(controllerId);

        return Ok(sensors);
    }
}
