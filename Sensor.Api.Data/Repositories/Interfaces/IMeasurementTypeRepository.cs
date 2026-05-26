using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Data.Repositories.Interfaces;

public interface IMeasurementTypeRepository
{
    Task<IEnumerable<MeasurementTypeQR>> GetMeasurementTypesAsync();
    Task<MeasurementTypeQR> CreateMeasurementTypeAsync(CreateMeasurementTypeRequestQR request);
}