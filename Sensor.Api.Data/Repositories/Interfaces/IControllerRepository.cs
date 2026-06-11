using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface IControllerRepository
{
    Task<IReadOnlyList<ControllerQR>> GetAllControllersAsync();

    Task<ControllerQR?> GetControllerByIdAsync(int id);

    Task<int> CreateAsync(CreateControllerQR request);

    Task<bool> UpdateAsync(int id, UpdateControllerQR request);

    Task<int> GetControllerKey(int id);

    Task<int> GetNextControllerKeySequenceNumberAsync(int locationId);
}