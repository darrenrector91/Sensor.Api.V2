namespace Sensor.Api.Web.Models;

public class SensorMeasurementResponse
{
    public long Id { get; set; }

    public int SensorId { get; set; }

    public string MeasurementType { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }
}