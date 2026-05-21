namespace Sensor.Api.Web.Models;

public class SensorLatestReadingResponse
{
    public string DeviceId { get; set; } = string.Empty;

    public decimal TemperatureC { get; set; }

    public decimal HumidityPercent { get; set; }

    public DateTime CreatedUtc { get; set; }
}