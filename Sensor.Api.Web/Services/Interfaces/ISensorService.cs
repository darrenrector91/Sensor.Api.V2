using Sensor.Api.Data.QueryResults;
using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Services.Interfaces;

public interface ISensorService
{
    Task<IReadOnlyList<SensorResponse>> GetSensorsByControllerIdAsync(int controllerId);

    Task<SensorResponse?> GetSensorByIdAsync(int id);

    Task<int> CreateSensorAsync(CreateSensorQR request);

    Task<bool> UpdateSensorAsync(int id, UpdateSensorQR request);
}