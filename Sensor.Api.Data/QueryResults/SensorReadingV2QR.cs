namespace Sensor.Api.Data.QueryResults;

public class SensorReadingV2QR
{
    public long Id { get; set; }

    public int SensorId { get; set; }

    public decimal? TemperatureC { get; set; }

    public decimal? HumidityPercent { get; set; }

    public DateTime CreatedUtc { get; set; }
}