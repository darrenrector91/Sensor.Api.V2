public class SensorMeasurementTypeQR
{
    public int SensorId { get; set; }

    public int MeasurementTypeId { get; set; }

    public string MeasurementTypeName { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string DefaultUnit { get; set; } = string.Empty;
}