using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<IEnumerable<LocationQR>> GetLocationsAsync();
    Task<LocationQR?> GetLocationByIdAsync(int id);
    Task<int> CreateLocationAsync(CreateLocationQR request);
    Task<bool> UpdateLocationAsync(int id, UpdateLocationQR request);
    Task<bool> DeleteLocationAsync(int id);
}
