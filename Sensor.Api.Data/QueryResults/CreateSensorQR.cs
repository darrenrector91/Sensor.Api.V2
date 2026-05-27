namespace Sensor.Api.Data.QueryResults;

public class CreateSensorQR
{
    public int ControllerId { get; set; }

    public int? LocationId { get; set; }

    public string SensorKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string SensorType { get; set; } = string.Empty;
}