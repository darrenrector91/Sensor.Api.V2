namespace Sensor.Api.Data.QueryResults;

public class UpdateControllerQR
{
    public string ControllerKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int? LocationId { get; set; }

    public bool IsActive { get; set; }
}
