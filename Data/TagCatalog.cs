using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Data;

public static class TagCatalog
{
    public static IReadOnlyList<IndustrialTag> CreateDefaultTags() =>
    [
        Tag("Machine.Status", "Status da Máquina", "Machine", "String", "", "Ligada, Parada, Executando ou Emergencia", "Parada"),
        Tag("Machine.Mode", "Modo Manual / Automático", "Machine", "String", "", "Modo atual de operacao", "Manual"),
        Tag("Machine.CycleTimeCurrent", "Tempo de ciclo atual", "Machine", "Number", "s", "Tempo do ciclo em execucao", "0"),
        Tag("Communication.PlcConnected", "Comunicação com CLP", "Communication", "Boolean", "", "Indicador de comunicacao com CLP"),
        Tag("Communication.MqttConnected", "Comunicação MQTT", "Communication", "Boolean", "", "Indicador de comunicacao com broker MQTT"),

        Tag("Sensors.EntrySlotCapacitive", "Sensor Capacitivo - Slot de Entrada", "Sensors"),
        Tag("Sensors.AxisXRetracted", "Sensor Magnético Eixo X Recuado", "Sensors"),
        Tag("Sensors.AxisXAdvanced", "Sensor Magnético Eixo X Avançado", "Sensors"),
        Tag("Sensors.AxisYRetracted", "Sensor Magnético Eixo Y Recuado", "Sensors"),
        Tag("Sensors.AxisYAdvanced", "Sensor Magnético Eixo Y Avançado", "Sensors"),
        Tag("Sensors.AxisZRetracted", "Sensor Magnético Eixo Z Recuado", "Sensors"),
        Tag("Sensors.AxisZAdvanced", "Sensor Magnético Eixo Z Avançado", "Sensors"),
        Tag("Sensors.Inductive", "Sensor Indutivo", "Sensors"),
        Tag("Sensors.OpticalReflexive", "Sensor Óptico Reflexivo", "Sensors"),
        Tag("Sensors.OpticalMirror1", "Sensor Óptico com Espelho Refletor 1", "Sensors"),
        Tag("Sensors.OpticalMirror2", "Sensor Óptico com Espelho Refletor 2", "Sensors"),
        Tag("Sensors.ExitSlotCapacitive", "Sensor Capacitivo - Slot de Saída", "Sensors"),

        Tag("Actuators.AxisXDisplacement", "Deslocamento do Eixo X", "Actuators", "Number", "mm", "Posicao/deslocamento atual do eixo X", "0"),
        Tag("Actuators.AxisYDisplacement", "Deslocamento do Eixo Y", "Actuators", "Number", "mm", "Posicao/deslocamento atual do eixo Y", "0"),
        Tag("Actuators.AxisZDisplacement", "Deslocamento do Eixo Z", "Actuators", "Number", "mm", "Posicao/deslocamento atual do eixo Z", "0"),
        Tag("Actuators.VacuumCupState", "Estado da Ventosa", "Actuators"),
        Tag("Actuators.ConveyorState", "Estado da Esteira", "Actuators", "String", "", "Avancando, Recuando ou Parada", "Parada"),
        Tag("Actuators.DiscardCylinder1State", "Estado do Cilindro de Descarte 1", "Actuators"),
        Tag("Actuators.DiscardCylinder2State", "Estado do Cilindro de Descarte 2", "Actuators"),

        Tag("Commands.Start", "Botão Start", "Commands", isCommand: true),
        Tag("Commands.Stop", "Botão Stop", "Commands", isCommand: true),
        Tag("Commands.Reset", "Botão Reset", "Commands", isCommand: true),
        Tag("Commands.Emergency", "Botão Emergência", "Commands", isCommand: true),
        Tag("Commands.SelectMode", "Seleção Manual / Automático", "Commands", "String", "", "Manual ou Automatico", "Manual", true),
        Tag("Commands.MoveAxisX", "Mover Eixo X", "Commands", isCommand: true),
        Tag("Commands.MoveAxisY", "Mover Eixo Y", "Commands", isCommand: true),
        Tag("Commands.MoveAxisZ", "Mover Eixo Z", "Commands", isCommand: true),
        Tag("Commands.ToggleVacuumCup", "Acionar Ventosa", "Commands", isCommand: true),
        Tag("Commands.ConveyorForward", "Avançar Esteira", "Commands", isCommand: true),
        Tag("Commands.ConveyorBackward", "Recuar Esteira", "Commands", isCommand: true),
        Tag("Commands.ActuateCylinder1", "Acionar Cilindro 1", "Commands", isCommand: true),
        Tag("Commands.ActuateCylinder2", "Acionar Cilindro 2", "Commands", isCommand: true),

        Tag("Counters.TotalProcessed", "Total de Peças Processadas", "Counters", "Number", "pecas", "Total de pecas processadas", "0"),
        Tag("Counters.TotalApproved", "Total de Peças Aprovadas", "Counters", "Number", "pecas", "Total de pecas aprovadas", "0"),
        Tag("Counters.TotalRejected", "Total de Peças Rejeitadas", "Counters", "Number", "pecas", "Total de pecas rejeitadas", "0"),
        Tag("Counters.AverageCycleTime", "Tempo Médio de Ciclo", "Counters", "Number", "s", "Media do tempo de ciclo", "0"),
        Tag("Counters.LastProcessedPart", "Última Peça Processada", "Counters", "String", "", "Identificador ou estado da ultima peca", ""),
        Tag("Counters.MachineEfficiency", "Eficiência da Máquina (%)", "Counters", "Number", "%", "Eficiencia calculada da maquina", "0"),

        Tag("Alarms.EmergencyActivated", "Emergência Acionada", "Alarms"),
        Tag("Alarms.PartStuckEntry", "Peça Presa na Entrada", "Alarms"),
        Tag("Alarms.PartStuckExit", "Peça Presa na Saída", "Alarms"),
        Tag("Alarms.AxisXFailure", "Falha no Eixo X", "Alarms"),
        Tag("Alarms.AxisYFailure", "Falha no Eixo Y", "Alarms"),
        Tag("Alarms.AxisZFailure", "Falha no Eixo Z", "Alarms"),
        Tag("Alarms.VacuumCupFailure", "Falha da Ventosa", "Alarms"),
        Tag("Alarms.PlcCommunicationFailure", "Falha de Comunicação com CLP", "Alarms"),
        Tag("Alarms.MqttCommunicationFailure", "Falha de Comunicação MQTT", "Alarms")
    ];

    private static IndustrialTag Tag(
        string name,
        string displayName,
        string category,
        string dataType = "Boolean",
        string unit = "",
        string description = "",
        string currentValue = "false",
        bool isCommand = false) =>
        new()
        {
            Name = name,
            DisplayName = displayName,
            Category = category,
            DataType = dataType,
            Unit = unit,
            Description = description,
            CurrentValue = currentValue,
            IsCommand = isCommand
        };
}
