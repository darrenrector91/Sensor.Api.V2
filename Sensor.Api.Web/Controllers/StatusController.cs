using Dapper;
using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/status")]
public class StatusController : ControllerBase
{
    private readonly IDbContext _databaseContext;

    public StatusController(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

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

    [HttpGet("database")]
    public async Task<IActionResult> GetDatabaseStatus()
    {
        try
        {
            using var connection = _databaseContext.CreateConnection();

            var result = await connection.ExecuteScalarAsync<int>("SELECT 1;");

            return Ok(new
            {
                Status = "ok",
                Database = "connected",
                Result = result,
                UtcNow = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "error",
                Database = "unavailable",
                Message = ex.Message,
                UtcNow = DateTime.UtcNow
            });
        }
    }
}