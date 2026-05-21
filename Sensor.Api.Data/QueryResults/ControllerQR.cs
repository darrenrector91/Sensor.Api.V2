namespace Sensor.Api.Data.QueryResults;

public class ControllerQR
{
    public int Id { get; set; }

    public string ControllerKey { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }
}