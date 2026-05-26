using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Web.Controllers;

[ApiController]
[Route("api/measurement-types")]
public sealed class MeasurementTypesController : ControllerBase
{
    private readonly IMeasurementTypeRepository measurementTypeRepository;

    public MeasurementTypesController(IMeasurementTypeRepository measurementTypeRepository)
    {
        this.measurementTypeRepository = measurementTypeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeasurementTypeQR>>> GetMeasurementTypes()
    {
        var measurementTypes = await measurementTypeRepository.GetMeasurementTypesAsync();

        return Ok(measurementTypes);
    }

    [HttpPost]
    public async Task<ActionResult<MeasurementTypeQR>> CreateMeasurementType(CreateMeasurementTypeRequestQR request)
    {
        var measurementType = await measurementTypeRepository.CreateMeasurementTypeAsync(request);

        return CreatedAtAction(
            nameof(GetMeasurementTypes),
            new { id = measurementType.Id },
            measurementType);
    }
}