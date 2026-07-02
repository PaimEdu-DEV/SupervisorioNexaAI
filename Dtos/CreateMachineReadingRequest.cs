using System.ComponentModel.DataAnnotations;

namespace SupervisorioSIMMAQ_NXA.Dtos;

public class CreateMachineReadingRequest
{
    [Required]
    public string MachineCode { get; set; } = string.Empty;

    [Required]
    public string Tag { get; set; } = string.Empty;

    public double Value { get; set; }

    public string Unit { get; set; } = string.Empty;

    public string Status { get; set; } = "Normal";

    public DateTime? CollectedAt { get; set; }
}
