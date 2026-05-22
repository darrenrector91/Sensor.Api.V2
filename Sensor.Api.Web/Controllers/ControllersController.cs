using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/controllers")]
public class ControllersController : ControllerBase
{
    private readonly IControllerRepository _controllerRepository;

    public ControllersController(
        IControllerRepository controllerRepository)
    {
        _controllerRepository = controllerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetControllers()
    {
        var controllers = await _controllerRepository.GetAllAsync();

        var response = controllers.Select(controller => new ControllerResponse
        {
            Id = controller.Id,
            ControllerKey = controller.ControllerKey,
            Name = controller.Name,
            Location = controller.Location,
            IsActive = controller.IsActive,
            CreatedUtc = controller.CreatedUtc
        });

        return Ok(response);
    }
}