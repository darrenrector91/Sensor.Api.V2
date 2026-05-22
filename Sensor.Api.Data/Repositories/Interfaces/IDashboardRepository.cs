using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<IReadOnlyList<DashboardMeasurementQR>> GetMeasurementsAsync();
}