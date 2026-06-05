using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Services;

public sealed class ControllerService : IControllerService
{
    private readonly IControllerRepository controllerRepository;
    private readonly ILocationRepository locationRepository;

    public ControllerService(IControllerRepository controllerRepository, ILocationRepository locationRepository)
    {
        this.controllerRepository = controllerRepository;
        this.locationRepository = locationRepository;
    }

    public async Task<IReadOnlyList<ControllerQR>> GetControllersAsync()
    {


        return await controllerRepository.GetAllControllersAsync();
    }

    public async Task<ControllerQR?> GetControllerByIdAsync(int id)
    {
        return await controllerRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateControllerAsync(CreateControllerQR request)
    {
        if (request.LocationId is not int locationId)
        {
            throw new ArgumentException("LocationId is required", nameof(request.LocationId));
        }

        var location = await locationRepository.GetLocationByIdAsync(locationId)
            ?? throw new ArgumentException("Location not found", nameof(request.LocationId));

        var keyNumber = await controllerRepository.GetNextControllerSequenceNumberAsync(locationId);

        var locationPrefix = location.Name
            .Trim()
            .ToLowerInvariant()
            .Replace(" ", "-");

        request.ControllerKey = $"{locationPrefix}-{keyNumber + 1:000}";

        return await controllerRepository.CreateAsync(request);
    }
    public async Task<bool> UpdateControllerAsync(int id, UpdateControllerQR request)
    {
        return await controllerRepository.UpdateAsync(id, request);
    }
}