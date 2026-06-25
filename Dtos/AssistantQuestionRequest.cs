using System.ComponentModel.DataAnnotations;

namespace SupervisorioSIMMAQ_NXA.Dtos;

public class AssistantQuestionRequest
{
    [Required]
    public string Question { get; set; } = string.Empty;
}
