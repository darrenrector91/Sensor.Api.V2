using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    [HttpGet("measurements")]
    public async Task<IActionResult> GetMeasurements()
    {
        var measurements = await dashboardService.GetMeasurementsAsync();

        return Ok(measurements);
    }
}