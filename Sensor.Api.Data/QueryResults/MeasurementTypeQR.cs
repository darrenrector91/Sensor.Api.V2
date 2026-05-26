namespace Sensor.Api.Data.QueryResults;

public sealed class MeasurementTypeQR
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? DefaultUnit { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string DisplayKind { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string CssClass { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedUtc { get; set; }
}