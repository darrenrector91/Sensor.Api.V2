using Dapper;
using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/status")]
public class StatusController : ControllerBase
{
    private readonly IDbContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusController"/> class.
    /// </summary>
    /// <param name="databaseContext">The database context used to verify database connectivity.</param>
    public StatusController(IDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Gets the current application status.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing status information.</returns>
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

    /// <summary>
    /// Gets the status of the database connection.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing database connectivity status.</returns>
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