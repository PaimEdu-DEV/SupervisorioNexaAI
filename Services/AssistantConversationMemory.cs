using System.Collections.Concurrent;

namespace SupervisorioSIMMAQ_NXA.Services;

public class AssistantConversationMemory
{
    private readonly ConcurrentDictionary<string, AssistantConversationState> _sessions = new();

    public AssistantConversationState Get(string conversationId) =>
        _sessions.GetOrAdd(Normalize(conversationId), _ => new AssistantConversationState());

    public void Update(string conversationId, string question, string answer, string? componentId)
    {
        var state = Get(conversationId);
        state.LastQuestion = question;
        state.LastAnswer = answer;
        state.LastInteractionAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(componentId))
        {
            state.LastComponentId = componentId;
        }
    }

    private static string Normalize(string conversationId) =>
        string.IsNullOrWhiteSpace(conversationId) ? "default" : conversationId.Trim();
}

public class AssistantConversationState
{
    public string? LastComponentId { get; set; }

    public string LastQuestion { get; set; } = string.Empty;

    public string LastAnswer { get; set; } = string.Empty;

    public DateTime LastInteractionAt { get; set; } = DateTime.UtcNow;
}
