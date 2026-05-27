namespace Sensor.Api.Data.QueryResults;

public class CreateControllerQR
{
    public string ControllerKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int? LocationId { get; set; }
}
