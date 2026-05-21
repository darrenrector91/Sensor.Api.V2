using Microsoft.AspNetCore.Mvc;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            Status = "ok",
            Application = "Sensor.Api",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            UtcNow = DateTime.UtcNow
        });
    }
}