using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/assistant")]
public class AssistantController(SupervisorioDbContext dbContext) : ControllerBase
{
    private static readonly string[] AllowedTerms =
    [
        "maquina", "máquina", "sensor", "atuador", "alarme", "falha", "diagnostico", "diagnóstico",
        "producao", "produção", "historico", "histórico", "clp", "mqtt", "eixo", "esteira",
        "ventosa", "cilindro", "peca", "peça", "ciclo", "emergencia", "emergência"
    ];

    [HttpPost("ask")]
    public async Task<ActionResult<object>> Ask(AssistantQuestionRequest request)
    {
        if (!AllowedTerms.Any(term => request.Question.Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            return Ok(new
            {
                answer = "Sou um Assistente Inteligente especializado exclusivamente nesta máquina industrial."
            });
        }

        var pendingDiagnostics = await dbContext.DiagnosticNotifications
            .AsNoTracking()
            .Where(notification => notification.Status == "Pending")
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(5)
            .ToListAsync();

        var tags = await dbContext.IndustrialTags.AsNoTracking().ToListAsync();
        var lastCommands = await dbContext.CommandExecutions
            .AsNoTracking()
            .OrderByDescending(command => command.CreatedAt)
            .Take(5)
            .ToListAsync();

        if (pendingDiagnostics.Count == 0)
        {
            return Ok(new
            {
                answer = "Analisei o estado atual da máquina, sensores, atuadores, alarmes e últimos comandos. No momento não há diagnóstico industrial pendente registrado pelo motor de diagnóstico. Continue monitorando comunicação CLP/MQTT, sensores e tempo de ciclo.",
                diagnostics = pendingDiagnostics
            });
        }

        var mainDiagnostic = pendingDiagnostics.First();
        var probabilities = await BuildCauseProbabilitiesAsync(mainDiagnostic.Description);

        var answer = $"""
            Analisei o estado completo da máquina antes de responder.

            Diagnóstico principal:
            {mainDiagnostic.Title}

            O que foi detectado:
            {mainDiagnostic.Description}

            Possíveis causas:
            {mainDiagnostic.PossibleCauses}

            Ação recomendada:
            {mainDiagnostic.RecommendedActions}

            Nível de severidade: {mainDiagnostic.Severity}
            Confiança estimada: {mainDiagnostic.Confidence:P0}

            Contexto adicional:
            Sensores ativos: {tags.Count(tag => tag.Category == "Sensors" && IsTrue(tag.CurrentValue))}
            Atuadores ativos: {tags.Count(tag => tag.Category == "Actuators" && IsTrue(tag.CurrentValue))}
            Último comando: {lastCommands.FirstOrDefault()?.CommandTag ?? "nenhum comando registrado"}
            """;

        if (probabilities.Count > 0)
        {
            answer += $"{Environment.NewLine}{Environment.NewLine}Histórico de manutenção para falhas semelhantes:{Environment.NewLine}";
            answer += string.Join(Environment.NewLine, probabilities.Select(item => $"- {item.Cause}: {item.Percent:0}%"));
        }

        return Ok(new
        {
            answer,
            diagnostics = pendingDiagnostics
        });
    }

    private async Task<List<(string Cause, double Percent)>> BuildCauseProbabilitiesAsync(string failureDescription)
    {
        var words = failureDescription
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(word => word.Length > 4)
            .Take(8)
            .ToList();

        var history = await dbContext.MaintenanceHistory.AsNoTracking().ToListAsync();
        var related = history
            .Where(item => words.Any(word => item.FailureDescription.Contains(word, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (related.Count == 0)
        {
            return [];
        }

        return related
            .GroupBy(item => item.RealCause)
            .Select(group => (Cause: group.Key, Percent: group.Count() * 100.0 / related.Count))
            .OrderByDescending(item => item.Percent)
            .ToList();
    }

    private static bool IsTrue(string value)
    {
        var normalized = value.ToLowerInvariant();
        return normalized is "true" or "1" or "on" or "ligado";
    }
}
