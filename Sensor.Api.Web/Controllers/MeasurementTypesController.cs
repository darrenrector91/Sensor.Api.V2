using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/measurement-types")]
public sealed class MeasurementTypesController : ControllerBase
{
    private readonly IMeasurementTypeService measurementTypeService;

    public MeasurementTypesController(IMeasurementTypeService measurementTypeService)
    {
        this.measurementTypeService = measurementTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementTypeQR>>> GetMeasurementTypes()
    {
        var measurementTypes = await measurementTypeService.GetMeasurementTypesAsync();

        return Ok(measurementTypes);
    }

    [HttpPost]
    public async Task<ActionResult<MeasurementTypeQR>> CreateMeasurementType(CreateMeasurementTypeQR request)
    {
        try
        {
            var measurementType = await measurementTypeService.CreateMeasurementTypeAsync(request);

            return CreatedAtAction(
                nameof(GetMeasurementTypes),
                new { id = measurementType.Id },
                measurementType);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
