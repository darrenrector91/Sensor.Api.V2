namespace Sensor.Api.Data.QueryResults;

public class CreateLocationQR
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
