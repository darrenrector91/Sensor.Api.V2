namespace Sensor.Api.Data.QueryResults;

public class MeasurementTypeQR
{
    public int SensorId { get; set; }

    public int MeasurementTypeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string DefaultUnit { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public string DisplayKind { get; set; } = string.Empty;

    public int Priority { get; set; }

    public string CssClass { get; set; } = string.Empty;

    public string AccentColor { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public string Color { get; set; } = string.Empty;

    public string DisplayStyle { get; set; } = string.Empty;

    public string ChartGroup { get; set; } = string.Empty;
}