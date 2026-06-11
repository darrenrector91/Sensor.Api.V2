public class SensorResponse
{
    public int Id { get; set; }

    public int ControllerId { get; set; }

    public int? LocationId { get; set; }

    public string? LocationName { get; set; }

    public string Name { get; set; } = string.Empty;

    public string HardwareModel { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CommunicationProtocol { get; set; } = string.Empty;

    public string? Address { get; set; }

    public int MeasurementIntervalSeconds { get; set; }

    public string Notes { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }
}