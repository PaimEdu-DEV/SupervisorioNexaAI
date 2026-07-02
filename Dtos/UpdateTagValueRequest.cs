using System.ComponentModel.DataAnnotations;

namespace SupervisorioSIMMAQ_NXA.Dtos;

public class UpdateTagValueRequest
{
    [Required]
    public string Value { get; set; } = string.Empty;

    public string Source { get; set; } = "Manual";
}
