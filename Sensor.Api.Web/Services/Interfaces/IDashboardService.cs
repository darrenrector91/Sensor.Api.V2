using Sensor.Api.Web.Models;

namespace Sensor.Api.Web.Services.Interfaces;

public interface IDashboardService
{
    Task<IReadOnlyList<DashboardMeasurementResponse>> GetMeasurementsAsync();
}