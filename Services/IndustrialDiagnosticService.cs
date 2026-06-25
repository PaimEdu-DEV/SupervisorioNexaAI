using Microsoft.EntityFrameworkCore;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Services;

public class IndustrialDiagnosticService(
    IServiceScopeFactory scopeFactory,
    ILogger<IndustrialDiagnosticService> logger) : BackgroundService
{
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan MovementTimeout = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan SensorActiveLimit = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan SensorInactiveLimit = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<SupervisorioDbContext>();
                await AnalyzeAsync(dbContext, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Falha ao executar ciclo do motor de diagnostico industrial.");
            }

            await Task.Delay(ScanInterval, stoppingToken);
        }
    }

    private static async Task AnalyzeAsync(SupervisorioDbContext dbContext, CancellationToken cancellationToken)
    {
        var tags = await dbContext.IndustrialTags.AsNoTracking().ToListAsync(cancellationToken);
        var tagMap = tags.ToDictionary(tag => tag.Name, tag => tag);
        var now = DateTime.UtcNow;

        await DetectCommunicationFailuresAsync(dbContext, tagMap, cancellationToken);
        await DetectIncoherentAxisSensorsAsync(dbContext, tagMap, cancellationToken);
        await DetectActiveAlarmsAsync(dbContext, tags, cancellationToken);
        await DetectMovementTimeoutsAsync(dbContext, tagMap, now, cancellationToken);
        await DetectStuckSensorsAsync(dbContext, tags, now, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task DetectCommunicationFailuresAsync(
        SupervisorioDbContext dbContext,
        Dictionary<string, IndustrialTag> tags,
        CancellationToken cancellationToken)
    {
        if (IsFalse(tags, "Communication.PlcConnected"))
        {
            await AddIfMissingAsync(dbContext, new DiagnosticNotification
            {
                Code = "COMM_PLC_LOST",
                Severity = "Critical",
                Origin = "Communication",
                SourceTag = "Communication.PlcConnected",
                Title = "Falha de comunicacao com CLP",
                Description = "A API nao esta recebendo confirmacao de comunicacao ativa com o CLP.",
                PossibleCauses = "Cabo de rede desconectado; CLP desligado; IP incorreto; falha no Node-RED; timeout de comunicacao.",
                RecommendedActions = "Verifique alimentacao do CLP, rede industrial, Node-RED e configuracao de IP antes de intervir na maquina.",
                Confidence = 0.9
            }, cancellationToken);
        }

        if (IsFalse(tags, "Communication.MqttConnected"))
        {
            await AddIfMissingAsync(dbContext, new DiagnosticNotification
            {
                Code = "COMM_MQTT_LOST",
                Severity = "Warning",
                Origin = "Communication",
                SourceTag = "Communication.MqttConnected",
                Title = "Falha de comunicacao MQTT",
                Description = "O supervisório indica comunicacao MQTT inativa ou sem confirmacao recente.",
                PossibleCauses = "Broker MQTT parado; credenciais incorretas; topico incorreto; rede indisponivel.",
                RecommendedActions = "Verifique o broker MQTT, topicos configurados e publicacoes do Node-RED.",
                Confidence = 0.85
            }, cancellationToken);
        }
    }

    private static async Task DetectIncoherentAxisSensorsAsync(
        SupervisorioDbContext dbContext,
        Dictionary<string, IndustrialTag> tags,
        CancellationToken cancellationToken)
    {
        await DetectOppositeSensorsAsync(dbContext, tags, "X", "Sensors.AxisXRetracted", "Sensors.AxisXAdvanced", cancellationToken);
        await DetectOppositeSensorsAsync(dbContext, tags, "Y", "Sensors.AxisYRetracted", "Sensors.AxisYAdvanced", cancellationToken);
        await DetectOppositeSensorsAsync(dbContext, tags, "Z", "Sensors.AxisZRetracted", "Sensors.AxisZAdvanced", cancellationToken);
    }

    private static async Task DetectOppositeSensorsAsync(
        SupervisorioDbContext dbContext,
        Dictionary<string, IndustrialTag> tags,
        string axis,
        string retractedTag,
        string advancedTag,
        CancellationToken cancellationToken)
    {
        if (!IsTrue(tags, retractedTag) || !IsTrue(tags, advancedTag))
        {
            return;
        }

        await AddIfMissingAsync(dbContext, new DiagnosticNotification
        {
            Code = $"AXIS_{axis}_INCOHERENT_SENSORS",
            Severity = "Critical",
            Origin = "Sensors",
            SourceTag = advancedTag,
            Title = $"Sensores incoerentes no eixo {axis}",
            Description = $"Os sensores de eixo {axis} recuado e avancado estao ativos ao mesmo tempo, uma condicao fisicamente incoerente.",
            PossibleCauses = "Sensor magnetico travado; sensor invertido; curto no sinal; erro de parametrizacao no CLP.",
            RecommendedActions = $"Verifique primeiro os sensores magneticos do eixo {axis}, cabos de sinal e entradas digitais do CLP.",
            Confidence = 0.96
        }, cancellationToken);
    }

    private static async Task DetectActiveAlarmsAsync(
        SupervisorioDbContext dbContext,
        List<IndustrialTag> tags,
        CancellationToken cancellationToken)
    {
        foreach (var alarm in tags.Where(tag => tag.Category == "Alarms" && IsTrue(tag.CurrentValue)))
        {
            await AddIfMissingAsync(dbContext, new DiagnosticNotification
            {
                Code = $"ACTIVE_{alarm.Name.Replace('.', '_').ToUpperInvariant()}",
                Severity = alarm.Name.Contains("Emergency", StringComparison.OrdinalIgnoreCase) ? "Critical" : "Warning",
                Origin = "Alarms",
                SourceTag = alarm.Name,
                Title = alarm.DisplayName,
                Description = $"O alarme '{alarm.DisplayName}' esta ativo no supervisório.",
                PossibleCauses = "Condicao de processo anormal; componente em falha; intertravamento acionado.",
                RecommendedActions = "Consulte o estado dos sensores e atuadores associados antes de resetar a falha.",
                Confidence = 0.92
            }, cancellationToken);
        }
    }

    private static async Task DetectMovementTimeoutsAsync(
        SupervisorioDbContext dbContext,
        Dictionary<string, IndustrialTag> tags,
        DateTime now,
        CancellationToken cancellationToken)
    {
        await DetectCommandTimeoutAsync(dbContext, tags, now, "Commands.MoveAxisX", "Sensors.AxisXAdvanced", "Eixo X nao avancou", cancellationToken);
        await DetectCommandTimeoutAsync(dbContext, tags, now, "Commands.MoveAxisY", "Sensors.AxisYAdvanced", "Eixo Y nao avancou", cancellationToken);
        await DetectCommandTimeoutAsync(dbContext, tags, now, "Commands.MoveAxisZ", "Sensors.AxisZAdvanced", "Eixo Z nao avancou", cancellationToken);
        await DetectCommandTimeoutAsync(dbContext, tags, now, "Commands.ConveyorForward", "Sensors.ExitSlotCapacitive", "Esteira sem confirmacao de movimento/saida", cancellationToken);
    }

    private static async Task DetectCommandTimeoutAsync(
        SupervisorioDbContext dbContext,
        Dictionary<string, IndustrialTag> tags,
        DateTime now,
        string commandTag,
        string expectedSensor,
        string title,
        CancellationToken cancellationToken)
    {
        if (!tags.TryGetValue(commandTag, out var command) || !IsTrue(command.CurrentValue))
        {
            return;
        }

        if (now - command.UpdatedAt < MovementTimeout || IsTrue(tags, expectedSensor))
        {
            return;
        }

        await AddIfMissingAsync(dbContext, new DiagnosticNotification
        {
            Code = $"TIMEOUT_{commandTag.Replace('.', '_').ToUpperInvariant()}",
            Severity = "Critical",
            Origin = "Actuators",
            SourceTag = commandTag,
            Title = title,
            Description = $"A tag {commandTag} foi acionada, mas {expectedSensor} nao confirmou a etapa dentro de {MovementTimeout.TotalSeconds:0}s.",
            PossibleCauses = "Sensor magnetico defeituoso; cilindro travado; valvula solenoide; falta de ar comprimido; falha eletrica.",
            RecommendedActions = $"Verifique primeiro o sensor esperado {expectedSensor}, depois valvula, cilindro e alimentacao pneumática.",
            Confidence = 0.88
        }, cancellationToken);
    }

    private static async Task DetectStuckSensorsAsync(
        SupervisorioDbContext dbContext,
        List<IndustrialTag> tags,
        DateTime now,
        CancellationToken cancellationToken)
    {
        foreach (var sensor in tags.Where(tag => tag.Category == "Sensors"))
        {
            if (IsTrue(sensor.CurrentValue) && now - sensor.UpdatedAt > SensorActiveLimit)
            {
                await AddIfMissingAsync(dbContext, new DiagnosticNotification
                {
                    Code = "SENSOR_STUCK_ON",
                    Severity = "Warning",
                    Origin = "Sensors",
                    SourceTag = sensor.Name,
                    Title = "Sensor permanentemente ligado",
                    Description = $"O sensor {sensor.DisplayName} permanece acionado por tempo excessivo.",
                    PossibleCauses = "Peca presa; sensor travado; ajuste mecanico incorreto; sinal digital em curto.",
                    RecommendedActions = $"Inspecione a regiao do sensor {sensor.DisplayName} e confirme o sinal no CLP.",
                    Confidence = 0.72
                }, cancellationToken);
            }

            if (!IsTrue(sensor.CurrentValue) && now - sensor.UpdatedAt > SensorInactiveLimit)
            {
                await AddIfMissingAsync(dbContext, new DiagnosticNotification
                {
                    Code = "SENSOR_NO_ACTIVITY",
                    Severity = "Info",
                    Origin = "Sensors",
                    SourceTag = sensor.Name,
                    Title = "Sensor sem atividade recente",
                    Description = $"O sensor {sensor.DisplayName} nao mudou de estado no periodo monitorado.",
                    PossibleCauses = "Sensor queimado; maquina parada; ausencia de pecas; cabo rompido.",
                    RecommendedActions = "Compare com o ciclo atual da maquina antes de substituir o sensor.",
                    Confidence = 0.55
                }, cancellationToken);
            }
        }
    }

    private static async Task AddIfMissingAsync(
        SupervisorioDbContext dbContext,
        DiagnosticNotification notification,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.DiagnosticNotifications.AnyAsync(item =>
            item.Code == notification.Code &&
            item.SourceTag == notification.SourceTag &&
            item.Status == "Pending",
            cancellationToken);

        if (!exists)
        {
            dbContext.DiagnosticNotifications.Add(notification);
        }
    }

    private static bool IsTrue(Dictionary<string, IndustrialTag> tags, string tagName) =>
        tags.TryGetValue(tagName, out var tag) && IsTrue(tag.CurrentValue);

    private static bool IsFalse(Dictionary<string, IndustrialTag> tags, string tagName) =>
        tags.TryGetValue(tagName, out var tag) && !IsTrue(tag.CurrentValue);

    private static bool IsTrue(string value)
    {
        var normalized = value.ToLowerInvariant();
        return normalized is "true" or "1" or "on" or "ligado";
    }
}
