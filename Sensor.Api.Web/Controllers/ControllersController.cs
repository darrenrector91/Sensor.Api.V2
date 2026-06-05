using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/controllers")]
public sealed class ControllersController : ControllerBase
{
    private readonly IControllerService controllerService;

    public ControllersController(IControllerService controllerService)
    {
        this.controllerService = controllerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ControllerQR>>> GetControllers()
    {
        var controllers = await controllerService.GetControllersAsync();

        return Ok(controllers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ControllerQR>> GetControllerById(int id)
    {
        var controller = await controllerService.GetControllerByIdAsync(id);

        if (controller is null)
        {
            return NotFound();
        }

        return Ok(controller);
    }


    [HttpPost]
    public async Task<ActionResult> CreateController(CreateControllerQR request)
    {
        await controllerService.CreateControllerAsync(request);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateController(int id, UpdateControllerQR request)
    {
        var updated = await controllerService.UpdateControllerAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}