using System.ComponentModel.DataAnnotations;

namespace SupervisorioSIMMAQ_NXA.Dtos;

public class ExecuteCommandRequest
{
    [Required]
    public string CommandTag { get; set; } = string.Empty;

    public string Value { get; set; } = "true";

    public string RequestedBy { get; set; } = "Postman";
}
