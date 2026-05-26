using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Services.Interfaces;

public interface ISensorService
{
    Task<IReadOnlyList<SensorResponse>> GetSensorsByControllerIdAsync(int controllerId);
}
