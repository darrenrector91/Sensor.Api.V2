using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Services;

public sealed class ControllerService : IControllerService
{
    private readonly IControllerRepository controllerRepository;

    public ControllerService(IControllerRepository controllerRepository)
    {
        this.controllerRepository = controllerRepository;
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
            throw new System.ArgumentException("LocationId is required", nameof(request.LocationId));
        }

        var keyNumber = await controllerRepository.GetControllerKey(locationId);

        request.ControllerKey = $"{request.Name.ToLowerInvariant()}-{keyNumber + 1}";

        return await controllerRepository.CreateAsync(request);
    }

    public async Task<bool> UpdateControllerAsync(int id, UpdateControllerQR request)
    {
        return await controllerRepository.UpdateAsync(id, request);
    }
}