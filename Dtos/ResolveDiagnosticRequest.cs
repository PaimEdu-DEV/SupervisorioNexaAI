namespace SupervisorioSIMMAQ_NXA.Dtos;

public class ResolveDiagnosticRequest
{
    public string RealCause { get; set; } = string.Empty;

    public string Component { get; set; } = string.Empty;

    public string ActionTaken { get; set; } = string.Empty;

    public string RegisteredBy { get; set; } = "Operador";
}
