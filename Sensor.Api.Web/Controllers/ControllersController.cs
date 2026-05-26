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
}
