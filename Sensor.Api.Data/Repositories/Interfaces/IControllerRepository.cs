using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface IControllerRepository
{
    Task<IReadOnlyList<ControllerQR>> GetAllAsync();

    Task<ControllerQR?> GetByIdAsync(int id);
}