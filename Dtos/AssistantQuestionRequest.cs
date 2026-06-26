using System.ComponentModel.DataAnnotations;

namespace SupervisorioSIMMAQ_NXA.Dtos;

public class AssistantQuestionRequest
{
    [Required]
    public string Question { get; set; } = string.Empty;

    public string ConversationId { get; set; } = "default";

    public string? ComponentId { get; set; }

    public string Mode { get; set; } = "diagnostic";
}
