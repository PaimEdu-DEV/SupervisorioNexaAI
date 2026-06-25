namespace SupervisorioSIMMAQ_NXA.Models;

public class IndustrialTag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string DataType { get; set; } = "Boolean";

    public string Unit { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCommand { get; set; }

    public string CurrentValue { get; set; } = "false";

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
