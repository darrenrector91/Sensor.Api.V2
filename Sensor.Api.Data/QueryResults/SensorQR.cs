namespace Sensor.Api.Data.QueryResults;

public class SensorQR
{
    public int Id { get; set; }

    public int ControllerId { get; set; }

    public string SensorKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string SensorType { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }
}