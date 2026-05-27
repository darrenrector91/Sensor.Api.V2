using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly ILocationRepository _locationRepository;

    public LocationsController(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationQR>>> GetLocations()
    {
        var locations = await _locationRepository.GetLocationsAsync();

        return Ok(locations);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LocationQR>> GetLocationById(int id)
    {
        var location = await _locationRepository.GetLocationByIdAsync(id);

        if (location is null)
        {
            return NotFound();
        }

        return Ok(location);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateLocation(CreateLocationQR request)
    {
        var id = await _locationRepository.CreateLocationAsync(request);

        return CreatedAtAction(nameof(GetLocationById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateLocation(int id, UpdateLocationQR request)
    {
        var updated = await _locationRepository.UpdateLocationAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var deleted = await _locationRepository.DeleteLocationAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}