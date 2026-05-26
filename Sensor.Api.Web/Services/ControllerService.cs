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
        return await controllerRepository.GetAllAsync();
    }
}
