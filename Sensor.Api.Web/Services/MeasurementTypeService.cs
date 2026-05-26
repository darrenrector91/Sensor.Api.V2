using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories.Interfaces;
using Sensor.Api.Web.Services.Interfaces;

namespace Sensor.Api.Web.Services;

public sealed class MeasurementTypeService : IMeasurementTypeService
{
    private readonly IMeasurementTypeRepository measurementTypeRepository;

    public MeasurementTypeService(IMeasurementTypeRepository measurementTypeRepository)
    {
        this.measurementTypeRepository = measurementTypeRepository;
    }

    public async Task<IEnumerable<MeasurementTypeQR>> GetMeasurementTypesAsync()
    {
        return await measurementTypeRepository.GetMeasurementTypesAsync();
    }

    public async Task<MeasurementTypeQR> CreateMeasurementTypeAsync(CreateMeasurementTypeQR request)
    {
        request.Name = NormalizeRequired(request.Name);
        request.DisplayName = NormalizeRequired(request.DisplayName);
        request.DefaultUnit = NormalizeOptional(request.DefaultUnit);
        request.Icon = NormalizeWithDefault(request.Icon, "monitoring");
        request.DisplayKind = NormalizeWithDefault(request.DisplayKind, "ValueCard");
        request.CssClass = NormalizeWithDefault(request.CssClass, "metric-card--default");
        request.AccentColor = NormalizeWithDefault(request.AccentColor, "#9fc9ff");
        request.Description = NormalizeOptional(request.Description);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Measurement type name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            throw new ArgumentException("Measurement type display name is required.");
        }

        if (request.Priority <= 0)
        {
            request.Priority = 999;
        }

        return await measurementTypeRepository.CreateMeasurementTypeAsync(request);
    }

    private static string NormalizeRequired(string value)
    {
        return value.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string NormalizeWithDefault(string value, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }
}