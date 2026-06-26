using Microsoft.AspNetCore.Mvc;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Models;
using SupervisorioSIMMAQ_NXA.Services;

namespace SupervisorioSIMMAQ_NXA.Controllers;

[ApiController]
[Route("api/assistant")]
public class AssistantController(
    SupervisorioDbContext dbContext,
    MachineComponentMap componentMap,
    IndustrialAgentOrchestrator industrialAgentOrchestrator) : ControllerBase
{
    [HttpGet("components")]
    public ActionResult<IEnumerable<MachineComponent>> GetComponents() => Ok(componentMap.GetAll());

    [HttpPost("inspection/complete")]
    public async Task<IActionResult> CompleteInspection(CompleteInspectionRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RealCause))
        {
            return BadRequest(new { message = "Informe a causa real para alimentar o historico de manutencao." });
        }

        var component = componentMap.Find(request.ComponentId);
        dbContext.MaintenanceHistory.Add(new MaintenanceHistory
        {
            FailureDescription = string.IsNullOrWhiteSpace(request.FailureDescription)
                ? $"Inspecao guiada finalizada para {component?.Name ?? request.ComponentId}."
                : request.FailureDescription,
            RealCause = request.RealCause,
            Component = component?.Name ?? request.ComponentId,
            ActionTaken = request.ActionTaken,
            RegisteredBy = request.RegisteredBy,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(new { message = "Historico de manutencao salvo com sucesso." });
    }

    [HttpPost("ask")]
    public async Task<ActionResult<IndustrialAgentResponse>> Ask(
        AssistantQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await industrialAgentOrchestrator.AskAsync(request, cancellationToken);
        return Ok(response);
    }
}
