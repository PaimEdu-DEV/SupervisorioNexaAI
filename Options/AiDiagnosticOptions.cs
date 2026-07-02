namespace SupervisorioSIMMAQ_NXA.Options;

public class AiDiagnosticOptions
{
    public string BaseUrl { get; set; } = "http://localhost:8001";

    public string Model { get; set; } = "llama3.2:3b";

    public int TimeoutSeconds { get; set; } = 90;
}
