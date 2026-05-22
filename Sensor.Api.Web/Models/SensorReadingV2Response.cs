namespace Sensor.Api.Web.Models;

public class SensorReadingV2Response
{
    public long Id { get; set; }

    public int SensorId { get; set; }

    public decimal? TemperatureC { get; set; }

    public decimal? HumidityPercent { get; set; }

    public DateTime CreatedUtc { get; set; }
}