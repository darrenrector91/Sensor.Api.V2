using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Web.Services.Interfaces;

public interface IControllerService
{
    Task<IReadOnlyList<ControllerQR>> GetControllersAsync();

    Task<ControllerQR?> GetControllerByIdAsync(int id);

    Task<int> CreateControllerAsync(CreateControllerQR request);

    Task<bool> UpdateControllerAsync(int id, UpdateControllerQR request);
}