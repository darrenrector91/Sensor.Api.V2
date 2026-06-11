namespace Sensor.Api.Data.QueryResults;

public class DashboardMeasurementQR
{
    public int ControllerId { get; set; }

    public string ControllerKey { get; set; } = string.Empty;

    public string ControllerName { get; set; } = string.Empty;

    public int? LocationId { get; set; }

    public string? LocationName { get; set; }

    public int SensorId { get; set; }

    public string SensorName { get; set; } = string.Empty;

    public string HardwareModel { get; set; } = string.Empty;

    public int MeasurementTypeId { get; set; }

    public string MeasurementType { get; set; } = string.Empty;

    public string MeasurementDisplayName { get; set; } = string.Empty;

    public decimal Value { get; set; }

    public string Unit { get; set; } = string.Empty;

    public string? Icon { get; set; }

    public string? Color { get; set; }

    public string? DisplayStyle { get; set; }

    public string? ChartGroup { get; set; }

    public int? Priority { get; set; }

    public string? CssClass { get; set; }

    public DateTime CreatedUtc { get; set; }
}