using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DashboardController"/> class.
    /// </summary>
    /// <param name="dashboardService">The dashboard service used to retrieve dashboard data.</param>
    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    /// <summary>
    /// Gets the latest dashboard measurements.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing dashboard measurements.</returns>
    [HttpGet("measurements")]
    public async Task<IActionResult> GetMeasurements()
    {
        var measurements = await dashboardService.GetMeasurementsAsync();

        return Ok(measurements);
    }
}