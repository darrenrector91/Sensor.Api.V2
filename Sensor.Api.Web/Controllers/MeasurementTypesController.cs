using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/measurement-types")]
public sealed class MeasurementTypesController : ControllerBase
{
    private readonly IMeasurementTypeService measurementTypeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasurementTypesController"/> class.
    /// </summary>
    /// <param name="measurementTypeService">The measurement type service used to manage measurement types.</param>
    public MeasurementTypesController(IMeasurementTypeService measurementTypeService)
    {
        this.measurementTypeService = measurementTypeService;
    }

    /// <summary>
    /// Gets all measurement types.
    /// </summary>
    /// <returns>An <see cref="ActionResult{T}"/> containing the list of measurement types.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementTypeQR>>> GetMeasurementTypes()
    {
        var measurementTypes = await measurementTypeService.GetMeasurementTypesAsync();

        return Ok(measurementTypes);
    }

    /// <summary>
    /// Creates a new measurement type.
    /// </summary>
    /// <param name="request">The measurement type creation request.</param>
    /// <returns>An <see cref="ActionResult{MeasurementTypeQR}"/> with the created measurement type or a bad request result.</returns>
    [HttpPost]
    public async Task<ActionResult<MeasurementTypeQR>> CreateMeasurementType(CreateMeasurementTypeQR request)
    {
        try
        {
            var measurementType = await measurementTypeService.CreateMeasurementTypeAsync(request);

            return CreatedAtAction(
                nameof(GetMeasurementTypes),
                new { id = measurementType.MeasurementTypeId },
                measurementType);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
