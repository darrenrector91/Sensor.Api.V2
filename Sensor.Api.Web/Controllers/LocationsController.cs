using Microsoft.AspNetCore.Mvc;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;

namespace Sensor.Api.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly ILocationRepository _locationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationsController"/> class.
    /// </summary>
    /// <param name="locationRepository">The location repository used to manage locations.</param>
    public LocationsController(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    /// <summary>
    /// Gets all locations.
    /// </summary>
    /// <returns>An <see cref="ActionResult{T}"/> containing the list of locations.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationQR>>> GetLocations()
    {
        var locations = await _locationRepository.GetLocationsAsync();

        return Ok(locations);
    }

    /// <summary>
    /// Gets a location by its identifier.
    /// </summary>
    /// <param name="id">The location identifier.</param>
    /// <returns>An <see cref="ActionResult{LocationQR}"/> containing the location or a NotFound result.</returns>
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

    /// <summary>
    /// Creates a new location.
    /// </summary>
    /// <param name="request">The location creation request.</param>
    /// <returns>An <see cref="ActionResult"/> with the created location identifier.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateLocation(CreateLocationQR request)
    {
        var id = await _locationRepository.CreateLocationAsync(request);

        return CreatedAtAction(nameof(GetLocationById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing location.
    /// </summary>
    /// <param name="id">The location identifier.</param>
    /// <param name="request">The updated location values.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the update result.</returns>
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

    /// <summary>
    /// Deletes a location by its identifier.
    /// </summary>
    /// <param name="id">The location identifier.</param>
    /// <returns>An <see cref="IActionResult"/> indicating whether the location was deleted.</returns>
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