namespace Sensor.Api.Data.QueryResults;

public sealed class CreateMeasurementTypeRequestQR
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? DefaultUnit { get; set; }
    public string Icon { get; set; } = "monitoring";
    public string DisplayKind { get; set; } = "ValueCard";
    public int Priority { get; set; } = 999;
    public string CssClass { get; set; } = "metric-card--default";
    public string AccentColor { get; set; } = "#9fc9ff";
    public string? Description { get; set; }
}