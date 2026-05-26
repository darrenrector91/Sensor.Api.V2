using Sensor.Api.Data.QueryResults;

namespace Sensor.Api.Web.Services.Interfaces;

public interface IMeasurementTypeService
{
    Task<IEnumerable<MeasurementTypeQR>> GetMeasurementTypesAsync();
    Task<MeasurementTypeQR> CreateMeasurementTypeAsync(CreateMeasurementTypeQR request);
}
