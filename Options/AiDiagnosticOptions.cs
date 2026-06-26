namespace SupervisorioSIMMAQ_NXA.Options;

public class AiDiagnosticOptions
{
    public string BaseUrl { get; set; } = "http://localhost:8001";

    public string Model { get; set; } = "qwen2.5:7b";

    public int TimeoutSeconds { get; set; } = 90;
}
