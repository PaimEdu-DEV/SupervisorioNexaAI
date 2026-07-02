using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Services;

public class IndustrialAgentOrchestrator(
    SupervisorioDbContext dbContext,
    AiDiagnosticClient aiDiagnosticClient,
    MachineComponentMap componentMap,
    AssistantConversationMemory conversationMemory,
    ILogger<IndustrialAgentOrchestrator> logger)
{
    private const string OutOfScopeAnswer = "Sou um Assistente Inteligente especializado exclusivamente nesta máquina industrial.";

    public async Task<IndustrialAgentResponse> AskAsync(AssistantQuestionRequest request, CancellationToken cancellationToken)
    {
        var conversation = conversationMemory.Get(request.ConversationId);
        var currentComponentId = request.ComponentId ?? conversation.LastComponentId;
        var detectedComponent = componentMap.Find(currentComponentId)
            ?? componentMap.FindByText(request.Question);
        var technicalComponent = MachineTechnicalKnowledge.FindByText(request.Question);

        detectedComponent ??= componentMap.Find(technicalComponent?.ComponentId);

        var intent = ClassifyIntent(request.Question, detectedComponent is not null || technicalComponent is not null);
        if (intent is AgentIntent.ProductionQuery or AgentIntent.History &&
            string.IsNullOrWhiteSpace(currentComponentId) &&
            !HasComponentKeyword(request.Question))
        {
            detectedComponent = null;
            technicalComponent = null;
        }

        if (intent == AgentIntent.OutOfScope)
        {
            return IndustrialAgentResponse.OutOfScope(OutOfScopeAnswer);
        }

        var brain = BrainFor(intent);
        var context = await BuildContextAsync(intent, request, detectedComponent, technicalComponent, cancellationToken);

        if (technicalComponent is not null && detectedComponent is not null)
        {
            var sensorResponse = BuildFallback(intent, brain, detectedComponent, technicalComponent, context, request.Question);
            conversationMemory.Update(request.ConversationId, request.Question, sensorResponse.Message, sensorResponse.ComponentId);
            return sensorResponse;
        }

        var payload = new
        {
            question = request.Question,
            mode = request.Mode,
            model = "llama3.2:3b",
            intent = IntentValue(intent),
            brain = BrainValue(brain),
            expectedResponse = "structured_json",
            conversation = new
            {
                request.ConversationId,
                conversation.LastComponentId,
                conversation.LastQuestion,
                conversation.LastAnswer,
                currentComponent = detectedComponent
            },
            context
        };

        try
        {
            var result = await aiDiagnosticClient.AskAsync(payload, cancellationToken);
            if (result is not null && !string.IsNullOrWhiteSpace(result.Message ?? result.Answer))
            {
                var response = FromAiResult(result, intent, brain, detectedComponent, context);
                if (ShouldUseDeterministicFallback(response, intent))
                {
                    response = BuildFallback(intent, brain, detectedComponent, technicalComponent, context, request.Question);
                }

                conversationMemory.Update(request.ConversationId, request.Question, response.Message, response.ComponentId);
                return response;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao consultar ai-diagnostic-service no IndustrialAgentOrchestrator.");
        }

        var fallback = BuildFallback(intent, brain, detectedComponent, technicalComponent, context, request.Question);
        conversationMemory.Update(request.ConversationId, request.Question, fallback.Message, fallback.ComponentId);
        return fallback;
    }

    private async Task<object> BuildContextAsync(
        AgentIntent intent,
        AssistantQuestionRequest request,
        MachineComponent? component,
        TechnicalComponent? technicalComponent,
        CancellationToken cancellationToken)
    {
        var tags = await dbContext.IndustrialTags.AsNoTracking().ToListAsync(cancellationToken);
        var diagnostics = await dbContext.DiagnosticNotifications.AsNoTracking()
            .Where(item => item.Status == "Pending")
            .OrderByDescending(item => item.CreatedAt)
            .Take(intent == AgentIntent.Diagnostic ? 8 : 3)
            .ToListAsync(cancellationToken);
        var tagHistory = await dbContext.TagValueHistory.AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .Take(intent is AgentIntent.ProductionQuery or AgentIntent.History ? 120 : 40)
            .ToListAsync(cancellationToken);
        var maintenanceHistory = await dbContext.MaintenanceHistory.AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
        var readings = await dbContext.MachineReadings.AsNoTracking()
            .OrderByDescending(item => item.CollectedAt)
            .Take(100)
            .ToListAsync(cancellationToken);
        var commands = await dbContext.CommandExecutions.AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        var counters = tags.Where(tag => tag.Category == "Counters").Select(ToTagPayload).ToList();
        var alarms = tags.Where(tag => tag.Category == "Alarms" && IsTrue(tag.CurrentValue)).Select(ToTagPayload).ToList();
        var missingData = MissingDataFor(intent, counters, readings, tagHistory, diagnostics, maintenanceHistory);

        return new
        {
            selectedBrain = BrainValue(BrainFor(intent)),
            selectedIntent = IntentValue(intent),
            machineKnowledge = new
            {
                description = MachineTechnicalKnowledge.MachineDescription,
                operationalCycle = MachineTechnicalKnowledge.OperationalCycle
            },
            component,
            technicalComponent,
            technicalMap = intent is AgentIntent.Technical or AgentIntent.VisualLocation or AgentIntent.ComponentInfo
                ? MachineTechnicalKnowledge.Components
                : MachineTechnicalKnowledge.Components.Take(8),
            componentMap = componentMap.GetAll(),
            machine = tags.Where(tag => tag.Category == "Machine").Select(ToTagPayload),
            communication = tags.Where(tag => tag.Category == "Communication").Select(ToTagPayload),
            sensors = tags.Where(tag => tag.Category == "Sensors").Select(ToTagPayload),
            actuators = tags.Where(tag => tag.Category == "Actuators").Select(ToTagPayload),
            counters,
            activeAlarms = alarms,
            pendingDiagnostics = diagnostics.Select(ToDiagnosticPayload),
            maintenanceHistory = maintenanceHistory.Select(ToMaintenancePayload),
            tagHistory = tagHistory.Select(ToTagHistoryPayload),
            readings = readings.Select(reading => new
            {
                reading.MachineCode,
                reading.Tag,
                reading.Value,
                reading.Unit,
                reading.Status,
                reading.Source,
                reading.CollectedAt
            }),
            lastCommands = commands.Select(command => new
            {
                command.CommandTag,
                command.Value,
                command.Status,
                command.CreatedAt
            }),
            productionSummary = BuildProductionSummary(counters, diagnostics, maintenanceHistory),
            missingData
        };
    }

    private static AgentIntent ClassifyIntent(string question, bool hasComponentContext)
    {
        var q = Normalize(question);

        if (ContainsAny(q, "quem ganhou", "futebol", "copa", "receita", "clima", "dolar", "filme"))
        {
            return AgentIntent.OutOfScope;
        }

        if (IsGreeting(q) || IsStatusQuestion(q))
        {
            return AgentIntent.MachineInfo;
        }

        if (ContainsAny(q, "onde fica", "localiza", "localizacao", "ver na maquina", "mostrar na maquina"))
        {
            return AgentIntent.VisualLocation;
        }

        if (ContainsAny(q, "falha", "parou", "alarme", "erro", "problema", "verificar", "diagnostico", "desligada", "travou", "anomalia", "inspecao", "conclusao", "nao acionou"))
        {
            return AgentIntent.Diagnostic;
        }

        if (ContainsAny(q, "modbus", "endereco", "tag", "entrada digital", "saida digital", "di", "do", "ao", "mw0", "mw1", "mw2", "mw5"))
        {
            return AgentIntent.Technical;
        }

        if (ContainsAny(q, "quantas", "quanto", "eficiencia", "processadas", "aprovadas", "rejeitadas", "producao", "estatistica"))
        {
            return AgentIntent.ProductionQuery;
        }

        if (ContainsAny(q, "historico", "ultima falha", "ultima manutencao", "ja ocorreu", "falhou"))
        {
            return AgentIntent.History;
        }

        if (ContainsAny(q, "o que faz", "oque faz", "que essa maquina faz", "qual funcao", "funcao", "como funciona", "explique o ciclo", "classificacao", "esta maquina", "essa maquina", "bancada"))
        {
            return hasComponentContext || ContainsAny(q, "sensor", "ventosa", "esteira", "cilindro", "botao")
                ? AgentIntent.ComponentInfo
                : AgentIntent.MachineInfo;
        }

        if (hasComponentContext)
        {
            return AgentIntent.ComponentInfo;
        }

        return AgentIntent.OutOfScope;
    }

    private static AgentBrain BrainFor(AgentIntent intent) => intent switch
    {
        AgentIntent.Diagnostic => AgentBrain.Diagnostic,
        AgentIntent.MachineInfo or AgentIntent.ComponentInfo or AgentIntent.VisualLocation => AgentBrain.MachineKnowledge,
        AgentIntent.ProductionQuery or AgentIntent.History => AgentBrain.Database,
        AgentIntent.Technical => AgentBrain.Technical,
        _ => AgentBrain.Diagnostic
    };

    private IndustrialAgentResponse FromAiResult(
        AiDiagnosticResult result,
        AgentIntent intent,
        AgentBrain brain,
        MachineComponent? component,
        dynamic context)
    {
        var componentId = !string.IsNullOrWhiteSpace(result.ComponentId)
            ? result.ComponentId
            : component?.Id;
        var resolvedComponent = componentMap.Find(componentId);
        componentId = resolvedComponent?.Id ?? componentId;

        var showHighlight = result.ShowHighlightButton || result.ShowSeeButton || intent == AgentIntent.VisualLocation || resolvedComponent is not null;
        var showInspection = result.ShowInspectionButton || result.ShowInspectButton || (brain == AgentBrain.Diagnostic && resolvedComponent is not null);

        return new IndustrialAgentResponse
        {
            Message = result.Message ?? result.Answer,
            Answer = result.Message ?? result.Answer,
            Intent = string.IsNullOrWhiteSpace(result.Intent) ? IntentValue(intent) : result.Intent,
            Brain = string.IsNullOrWhiteSpace(result.Brain) ? BrainValue(brain) : result.Brain,
            Severity = NormalizeSeverity(result.Severity),
            ComponentId = componentId,
            ShowHighlightButton = showHighlight,
            ShowSeeButton = showHighlight,
            ShowInspectionButton = showInspection,
            ShowInspectButton = showInspection,
            QuickActions = BuildQuickActions(showHighlight, showInspection, brain),
            MissingData = result.MissingData,
            UsedSources = result.UsedSources.Count > 0 ? result.UsedSources : SourcesFor(brain),
            Probabilities = result.Probabilities,
            RecommendedActions = result.RecommendedActions,
            NeedsMaintenance = result.NeedsMaintenance,
            OpenManual = result.OpenManual || brain == AgentBrain.Technical,
            OpenHistory = result.OpenHistory || brain == AgentBrain.Database,
            Confidence = result.Confidence,
            Source = "industrial-agent"
        };
    }

    private static bool ShouldUseDeterministicFallback(IndustrialAgentResponse response, AgentIntent intent)
    {
        if (intent == AgentIntent.Diagnostic)
        {
            return false;
        }

        var message = Normalize(response.Message);
        return message.Contains("nao existem dados suficientes") ||
            message.Contains("nao consegui encontrar") ||
            string.IsNullOrWhiteSpace(response.Message);
    }

    private IndustrialAgentResponse BuildFallback(
        AgentIntent intent,
        AgentBrain brain,
        MachineComponent? component,
        TechnicalComponent? technicalComponent,
        dynamic context,
        string question)
    {
        var message = intent switch
        {
            AgentIntent.MachineInfo when IsGreeting(Normalize(question)) || IsStatusQuestion(Normalize(question)) => BuildMachineStatusFallback(context),
            AgentIntent.MachineInfo => BuildMachineExplanationFallback(),
            AgentIntent.ComponentInfo when technicalComponent is not null =>
                $"{technicalComponent.Name}: {technicalComponent.Description}",
            AgentIntent.VisualLocation when component is not null =>
                $"{component.Name} fica na area {component.Area} da bancada. Use o destaque visual para localizar o componente na maquina.",
            AgentIntent.Technical when technicalComponent is not null =>
                $"{technicalComponent.Name}\nTag: {technicalComponent.Tag}\nEndereco: {technicalComponent.Address}\nDescricao: {technicalComponent.Description}",
            AgentIntent.ProductionQuery =>
                "Ainda nao existem dados de producao suficientes para responder essa pergunta. Para responder com precisao, preciso de historico de producao, contadores atualizados ou leituras registradas da esteira.",
            AgentIntent.History =>
                "Ainda nao existem dados historicos suficientes para responder essa pergunta. Para responder, preciso de registros em MaintenanceHistory, DiagnosticNotifications ou historico de sensores.",
            AgentIntent.Diagnostic when component is not null =>
                $"{component.Name}: {component.Description} Se houver falha, verifique primeiro LED do sensor, alinhamento mecanico, cabo/conector, alimentacao e a entrada digital correspondente no CLP.",
            _ =>
                "Nao existem dados suficientes para determinar a causa da falha."
        };

        var showHighlight = component is not null && intent is AgentIntent.VisualLocation or AgentIntent.ComponentInfo or AgentIntent.Technical;
        var showInspection = component is not null && intent == AgentIntent.Diagnostic;

        return new IndustrialAgentResponse
        {
            Message = message,
            Answer = message,
            Intent = IntentValue(intent),
            Brain = BrainValue(brain),
            Severity = intent == AgentIntent.Diagnostic ? "warning" : "info",
            ComponentId = component?.Id ?? technicalComponent?.ComponentId,
            ShowHighlightButton = showHighlight,
            ShowSeeButton = showHighlight,
            ShowInspectionButton = showInspection,
            ShowInspectButton = showInspection,
            QuickActions = BuildQuickActions(showHighlight, showInspection, brain),
            MissingData = intent is AgentIntent.ProductionQuery or AgentIntent.History
                ? new List<string> { "historico de producao ou manutencao insuficiente" }
                : new List<string>(),
            UsedSources = SourcesFor(brain),
            Source = "industrial-agent-fallback"
        };
    }

    private static object BuildProductionSummary(
        List<object> counters,
        List<DiagnosticNotification> diagnostics,
        List<MaintenanceHistory> maintenanceHistory) => new
    {
        counters,
        pendingFailures = diagnostics.Count,
        maintenanceRecords = maintenanceHistory.Count,
        note = "Use apenas estes contadores e historicos. Nao invente numeros."
    };

    private static List<string> MissingDataFor(
        AgentIntent intent,
        List<object> counters,
        List<MachineReading> readings,
        List<TagValueHistory> tagHistory,
        List<DiagnosticNotification> diagnostics,
        List<MaintenanceHistory> maintenanceHistory)
    {
        var missing = new List<string>();
        if (intent == AgentIntent.ProductionQuery && counters.Count == 0)
        {
            missing.Add("contadores de producao");
        }
        if (intent == AgentIntent.ProductionQuery && readings.Count == 0 && tagHistory.Count == 0)
        {
            missing.Add("historico de leituras da esteira ou pecas");
        }
        if (intent == AgentIntent.History && diagnostics.Count == 0 && maintenanceHistory.Count == 0)
        {
            missing.Add("historico de diagnosticos ou manutencoes");
        }

        return missing;
    }

    private static List<object> BuildQuickActions(bool hasComponent, bool canInspect, AgentBrain brain) =>
    [
        new { id = "see", label = "Ver na Maquina", enabled = hasComponent },
        new { id = "inspect", label = "Iniciar Inspecao", enabled = canInspect },
        new { id = "history", label = "Historico", enabled = true },
        new { id = "manual", label = "Manual", enabled = brain is AgentBrain.Technical or AgentBrain.MachineKnowledge },
        new { id = "full-diagnostic", label = "Diagnostico Completo", enabled = true },
        new { id = "refresh", label = "Atualizar Dados", enabled = true }
    ];

    private static List<string> SourcesFor(AgentBrain brain) => brain switch
    {
        AgentBrain.Diagnostic => ["Motor de Diagnostico Industrial", "Alarmes ativos", "Sensores", "Atuadores", "Historico recente"],
        AgentBrain.MachineKnowledge => ["machine_knowledge", "MachineComponentMap", "Cadastro de componentes"],
        AgentBrain.Database => ["Banco de dados", "Counters", "TagValueHistory", "MaintenanceHistory", "DiagnosticNotifications"],
        AgentBrain.Technical => ["technical_map", "component_map", "Tabela de sensores e atuadores"],
        _ => []
    };

    private static object ToTagPayload(IndustrialTag tag) => new
    {
        id = tag.Name,
        tag.Name,
        tag.DisplayName,
        tag.Category,
        tag.DataType,
        tag.Unit,
        tag.Description,
        tag.CurrentValue,
        tag.UpdatedAt
    };

    private static object ToDiagnosticPayload(DiagnosticNotification diagnostic) => new
    {
        diagnostic.Id,
        diagnostic.Code,
        diagnostic.Severity,
        diagnostic.Origin,
        diagnostic.Title,
        diagnostic.Description,
        diagnostic.PossibleCauses,
        diagnostic.RecommendedActions,
        diagnostic.SourceTag,
        diagnostic.Confidence,
        diagnostic.CreatedAt
    };

    private static object ToMaintenancePayload(MaintenanceHistory history) => new
    {
        history.Id,
        history.FailureDescription,
        history.RealCause,
        history.Component,
        history.ActionTaken,
        history.CreatedAt
    };

    private static object ToTagHistoryPayload(TagValueHistory history) => new
    {
        history.TagName,
        history.Value,
        history.Source,
        history.CreatedAt
    };

    private static bool IsTrue(string value)
    {
        var normalized = value.ToLowerInvariant();
        return normalized is "true" or "1" or "on" or "ligado";
    }

    private static string Normalize(string value) => value
        .ToLowerInvariant()
        .Replace("á", "a").Replace("à", "a").Replace("ã", "a").Replace("â", "a")
        .Replace("é", "e").Replace("ê", "e")
        .Replace("í", "i")
        .Replace("ó", "o").Replace("ô", "o").Replace("õ", "o")
        .Replace("ú", "u")
        .Replace("ç", "c");

    private static bool ContainsAny(string value, params string[] terms) =>
        terms.Any(value.Contains);

    private static bool IsGreeting(string value)
    {
        var cleaned = value.Trim(' ', '.', ',', '!', '?');
        return cleaned is "oi" or "ola" or "olá" or "bom dia" or "boa tarde" or "boa noite";
    }

    private static bool IsStatusQuestion(string value) =>
        ContainsAny(value, "status", "estado atual", "situacao", "como esta", "como ta", "esta online", "maquina ligada");

    private static string BuildMachineExplanationFallback()
    {
        return $"{MachineTechnicalKnowledge.MachineDescription.Trim()} {MachineTechnicalKnowledge.OperationalCycle.Trim()}";
    }

    private static string BuildMachineStatusFallback(dynamic context)
    {
        var status = FindTagValue(context.machine, "Machine.Status") ?? "Parada";
        var mode = FindTagValue(context.machine, "Machine.Mode") ?? "Manual";
        var cycleTime = FindTagValue(context.machine, "Machine.CycleTimeCurrent") ?? "0";
        var plc = FindTagValue(context.communication, "Communication.PlcConnected") ?? "false";
        var mqtt = FindTagValue(context.communication, "Communication.MqttConnected") ?? "false";

        return $"Estou online no supervisório. Estado atual da máquina: {status}. Modo: {mode}. Tempo de ciclo: {cycleTime}. Comunicação CLP: {ConnectionLabel(plc)}. MQTT: {ConnectionLabel(mqtt)}.";
    }

    private static string? FindTagValue(IEnumerable<object> tags, string name)
    {
        foreach (var tag in tags)
        {
            var type = tag.GetType();
            var tagName = type.GetProperty("name")?.GetValue(tag)?.ToString();
            if (string.Equals(tagName, name, StringComparison.OrdinalIgnoreCase))
            {
                return type.GetProperty("currentValue")?.GetValue(tag)?.ToString();
            }
        }

        return null;
    }

    private static string ConnectionLabel(string value) => IsTrue(value) ? "online" : "offline";

    private static bool HasComponentKeyword(string question)
    {
        var q = Normalize(question);
        return ContainsAny(q,
            "sensor", "capacitivo", "indutivo", "optico", "esteira", "ventosa", "cilindro",
            "eixo", "botao", "emergencia", "entrada", "saida", "do4", "di", "modbus");
    }

    private static string IntentValue(AgentIntent intent) => intent switch
    {
        AgentIntent.MachineInfo => "machine_info",
        AgentIntent.ProductionQuery => "production_query",
        AgentIntent.ComponentInfo => "component_info",
        AgentIntent.VisualLocation => "visual_location",
        AgentIntent.History => "history",
        AgentIntent.Technical => "technical",
        AgentIntent.OutOfScope => "out_of_scope",
        _ => "diagnostic"
    };

    private static string BrainValue(AgentBrain brain) => brain switch
    {
        AgentBrain.MachineKnowledge => "machine_knowledge",
        AgentBrain.Database => "database",
        AgentBrain.Technical => "technical",
        _ => "diagnostic"
    };

    private static string NormalizeSeverity(string severity) => Normalize(severity) switch
    {
        "critical" or "critico" or "critica" => "critical",
        "warning" or "aviso" => "warning",
        _ => "info"
    };
}

public enum AgentIntent
{
    Diagnostic,
    MachineInfo,
    ProductionQuery,
    ComponentInfo,
    VisualLocation,
    History,
    Technical,
    OutOfScope
}

public enum AgentBrain
{
    Diagnostic,
    MachineKnowledge,
    Database,
    Technical
}

public class IndustrialAgentResponse
{
    public string Message { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string Intent { get; set; } = "diagnostic";
    public string Brain { get; set; } = "diagnostic";
    public string Severity { get; set; } = "info";
    public string? ComponentId { get; set; }
    public bool ShowHighlightButton { get; set; }
    public bool ShowSeeButton { get; set; }
    public bool ShowInspectionButton { get; set; }
    public bool ShowInspectButton { get; set; }
    public List<object> QuickActions { get; set; } = [];
    public List<string> MissingData { get; set; } = [];
    public List<string> UsedSources { get; set; } = [];
    public List<AiProbability> Probabilities { get; set; } = [];
    public List<string> RecommendedActions { get; set; } = [];
    public bool NeedsMaintenance { get; set; }
    public bool OpenManual { get; set; }
    public bool OpenHistory { get; set; }
    public double Confidence { get; set; }
    public string Source { get; set; } = "industrial-agent";

    public static IndustrialAgentResponse OutOfScope(string message) => new()
    {
        Message = message,
        Answer = message,
        Intent = "out_of_scope",
        Brain = "diagnostic",
        Severity = "info",
        Source = "industrial-agent-context-guard",
        UsedSources = []
    };
}
