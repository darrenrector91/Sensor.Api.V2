namespace Sensor.Api.Web.Models;

public class CreateMeasurementRequest
{
    public string MeasurementType { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;
}