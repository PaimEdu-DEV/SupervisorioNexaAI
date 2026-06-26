using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SupervisorioSIMMAQ_NXA.Options;

namespace SupervisorioSIMMAQ_NXA.Services;

public class AiDiagnosticClient(HttpClient httpClient, IOptions<AiDiagnosticOptions> options)
{
    private readonly AiDiagnosticOptions _options = options.Value;

    public async Task<AiDiagnosticResult?> AskAsync(object payload, CancellationToken cancellationToken)
    {
        httpClient.BaseAddress = new Uri(_options.BaseUrl);
        httpClient.Timeout = TimeSpan.FromSeconds(Math.Max(10, _options.TimeoutSeconds));

        var response = await httpClient.PostAsJsonAsync("/diagnose", payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AiDiagnosticResult>(cancellationToken);
    }
}

public class AiDiagnosticResult
{
    [JsonPropertyName("answer")]
    public string Answer { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("severity")]
    public string Severity { get; set; } = "info";

    [JsonPropertyName("intent")]
    public string Intent { get; set; } = "diagnostic";

    [JsonPropertyName("brain")]
    public string Brain { get; set; } = "diagnostic";

    [JsonPropertyName("componentId")]
    public string? ComponentId { get; set; }

    [JsonPropertyName("component")]
    public string? Component { get; set; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; } = "local";

    [JsonPropertyName("showSeeButton")]
    public bool ShowSeeButton { get; set; }

    [JsonPropertyName("showHighlightButton")]
    public bool ShowHighlightButton { get; set; }

    [JsonPropertyName("showInspectButton")]
    public bool ShowInspectButton { get; set; }

    [JsonPropertyName("showInspectionButton")]
    public bool ShowInspectionButton { get; set; }

    [JsonPropertyName("probabilities")]
    public List<AiProbability> Probabilities { get; set; } = [];

    [JsonPropertyName("recommendedActions")]
    public List<string> RecommendedActions { get; set; } = [];

    [JsonPropertyName("needsMaintenance")]
    public bool NeedsMaintenance { get; set; }

    [JsonPropertyName("openManual")]
    public bool OpenManual { get; set; }

    [JsonPropertyName("openHistory")]
    public bool OpenHistory { get; set; }

    [JsonPropertyName("missingData")]
    public List<string> MissingData { get; set; } = [];

    [JsonPropertyName("usedSources")]
    public List<string> UsedSources { get; set; } = [];
}

public class AiProbability
{
    [JsonPropertyName("cause")]
    public string Cause { get; set; } = string.Empty;

    [JsonPropertyName("percent")]
    public double Percent { get; set; }
}
