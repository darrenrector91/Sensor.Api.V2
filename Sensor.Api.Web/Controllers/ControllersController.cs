using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/controllers")]
public sealed class ControllersController : ControllerBase
{
    private readonly IControllerService controllerService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllersController"/> class.
    /// </summary>
    /// <param name="controllerService">The controller service used to manage controller data.</param>
    public ControllersController(IControllerService controllerService)
    {
        this.controllerService = controllerService;
    }

    /// <summary>
    /// Gets all controllers.
    /// </summary>
    /// <returns>An <see cref="ActionResult{T}"/> containing the list of controllers.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ControllerQR>>> GetControllers()
    {
        var controllers = await controllerService.GetControllersAsync();

        return Ok(controllers);
    }

    /// <summary>
    /// Gets a controller by its identifier.
    /// </summary>
    /// <param name="id">The controller identifier.</param>
    /// <returns>An <see cref="ActionResult{ControllerQR}"/> containing the controller or a NotFound result.</returns>
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


    /// <summary>
    /// Creates a new controller.
    /// </summary>
    /// <param name="request">The controller creation request.</param>
    /// <returns>An <see cref="ActionResult"/> with the created result status.</returns>
    [HttpPost]
    public async Task<ActionResult> CreateController(CreateControllerQR request)
    {
        await controllerService.CreateControllerAsync(request);

        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Updates an existing controller.
    /// </summary>
    /// <param name="id">The controller identifier.</param>
    /// <param name="request">The updated controller values.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the update result.</returns>
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