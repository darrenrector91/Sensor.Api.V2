namespace Sensor.Api.Web.Models;

public class CreateSensorReadingRequest
{
    public string DeviceId { get; set; } = string.Empty;

    public decimal TemperatureC { get; set; }

    public decimal HumidityPercent { get; set; }
}