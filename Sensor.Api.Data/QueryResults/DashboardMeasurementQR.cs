namespace Sensor.Api.Data.QueryResults;

public class DashboardMeasurementQR
{
    public int ControllerId { get; set; }

    public string ControllerKey { get; set; } = string.Empty;

    public string ControllerName { get; set; } = string.Empty;

    public string? Location { get; set; }

    public int SensorId { get; set; }

    public string SensorKey { get; set; } = string.Empty;

    public string SensorName { get; set; } = string.Empty;

    public string SensorType { get; set; } = string.Empty;

    public string MeasurementType { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; }
}