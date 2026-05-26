using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Web.Services.Interfaces;

public interface IControllerService
{
    Task<IReadOnlyList<ControllerQR>> GetControllersAsync();
}
