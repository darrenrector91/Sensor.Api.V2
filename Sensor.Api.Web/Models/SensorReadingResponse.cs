namespace Sensor.Api.Web.Models;

public class SensorReadingResponse
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = string.Empty;

    public decimal TemperatureC { get; set; }

    public decimal HumidityPercent { get; set; }

    public DateTime CreatedUtc { get; set; }
}