namespace Sensor.Api.Data.QueryResults;

public class SensorReadingQR
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = string.Empty;

    public decimal TemperatureC { get; set; }

    public decimal HumidityPercent { get; set; }

    public DateTime CreatedUtc { get; set; }
}