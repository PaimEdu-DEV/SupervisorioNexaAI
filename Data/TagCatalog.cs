using SupervisorioSIMMAQ_NXA.Models;

namespace SupervisorioSIMMAQ_NXA.Data;

public static class TagCatalog
{
    public static IReadOnlyList<IndustrialTag> CreateDefaultTags() =>
    [
        Tag("Machine.Status", "Status da Maquina", "Machine", "String", "", "Ligada, Parada, Executando ou Emergencia", "Parada"),
        Tag("Machine.Mode", "Modo Manual / Automatico", "Machine", "String", "", "Modo atual de operacao", "Manual"),
        Tag("Machine.CycleTimeCurrent", "Tempo de ciclo atual", "Machine", "Number", "s", "Tempo do ciclo em execucao", "0"),
        Tag("Communication.PlcConnected", "Comunicacao com CLP", "Communication", "Boolean", "", "Indicador de comunicacao com CLP"),
        Tag("Communication.MqttConnected", "Comunicacao MQTT", "Communication", "Boolean", "", "Indicador de comunicacao com broker MQTT"),

        Tag("Sensors.EntrySlotCapacitive", "DI0 - Sensor Capacitivo Slot de Entrada", "Sensors"),
        Tag("Sensors.AxisXRetracted", "DI1 - Sensor Magnetico Eixo X Recuado", "Sensors"),
        Tag("Sensors.AxisXAdvanced", "DI2 - Sensor Magnetico Eixo X Avancado", "Sensors"),
        Tag("Sensors.AxisYRetracted", "DI3 - Sensor Magnetico Eixo Y Recuado", "Sensors"),
        Tag("Sensors.AxisYAdvanced", "DI4 - Sensor Magnetico Eixo Y Avancado", "Sensors"),
        Tag("Sensors.AxisZRetracted", "DI5 - Sensor Magnetico Eixo Z Recuado", "Sensors"),
        Tag("Sensors.AxisZAdvanced", "DI6 - Sensor Magnetico Eixo Z Avancado", "Sensors"),
        Tag("Sensors.Inductive", "DI7 - Sensor Indutivo", "Sensors"),
        Tag("Sensors.OpticalReflexive", "DI8 - Sensor Otico Reflexivo", "Sensors"),
        Tag("Sensors.OpticalMirror1", "DI9 - Sensor Otico com Espelho Refletor 1", "Sensors"),
        Tag("Sensors.OpticalMirror2", "DI10 - Sensor Otico com Espelho Refletor 2", "Sensors"),
        Tag("Sensors.ExitSlotCapacitive", "DI11 - Sensor Capacitivo Slot de Saida", "Sensors"),
        Tag("Sensors.AxisXRetractedB", "DI12 - Sensor Magnetico Eixo X Recuado", "Sensors"),
        Tag("Sensors.AxisXAdvancedB", "DI13 - Sensor Magnetico Eixo X Avancado", "Sensors"),
        Tag("Sensors.AxisYRetractedB", "DI14 - Sensor Magnetico Eixo Y Recuado", "Sensors"),
        Tag("Sensors.AxisYAdvancedB", "DI15 - Sensor Magnetico Eixo Y Avancado", "Sensors"),
        Tag("Sensors.AxisZRetractedB", "DI16 - Sensor Magnetico Eixo Z Recuado", "Sensors"),
        Tag("Sensors.AxisZAdvancedB", "DI17 - Sensor Magnetico Eixo Z Avancado", "Sensors"),

        Tag("Actuators.AxisXDisplacement", "Deslocamento do Eixo X", "Actuators", "Number", "mm", "Posicao/deslocamento atual do eixo X", "0"),
        Tag("Actuators.AxisYDisplacement", "Deslocamento do Eixo Y", "Actuators", "Number", "mm", "Posicao/deslocamento atual do eixo Y", "0"),
        Tag("Actuators.AxisZDisplacement", "Deslocamento do Eixo Z", "Actuators", "Number", "mm", "Posicao/deslocamento atual do eixo Z", "0"),
        Tag("Actuators.VacuumCupState", "DO3 - Estado da Ventosa", "Actuators"),
        Tag("Actuators.ConveyorState", "DO4/DO5 - Estado da Esteira", "Actuators", "String", "", "Avancando, Recuando ou Parada", "Parada"),
        Tag("Actuators.DiscardCylinder1State", "DO6 - Estado do Cilindro de Descarte 1", "Actuators"),
        Tag("Actuators.DiscardCylinder2State", "DO7 - Estado do Cilindro de Descarte 2", "Actuators"),
        Tag("Actuators.AxisXMoveB", "DO8 - Desloca Eixo X", "Actuators"),
        Tag("Actuators.AxisYMoveB", "DO9 - Desloca Eixo Y", "Actuators"),
        Tag("Actuators.AxisZMoveB", "DO10 - Desloca Eixo Z", "Actuators"),
        Tag("Actuators.VacuumOnB", "DO11 - Aciona Ventosa", "Actuators"),

        Tag("Commands.Start", "DI18 - Botao Start", "Commands", isCommand: true),
        Tag("Commands.Stop", "Botao Stop", "Commands", isCommand: true),
        Tag("Commands.Reset", "DI19 - Botao Reset", "Commands", isCommand: true),
        Tag("Commands.Emergency", "DI20 - Botao Emergencia", "Commands", isCommand: true),
        Tag("Commands.SelectMode", "Selecao Manual / Automatico", "Commands", "String", "", "Manual ou Automatico", "Manual", true),
        Tag("Commands.MoveAxisX", "DO0/DO8 - Mover Eixo X", "Commands", isCommand: true),
        Tag("Commands.MoveAxisY", "DO1/DO9 - Mover Eixo Y", "Commands", isCommand: true),
        Tag("Commands.MoveAxisZ", "DO2/DO10 - Mover Eixo Z", "Commands", isCommand: true),
        Tag("Commands.ToggleVacuumCup", "DO3/DO11 - Acionar Ventosa", "Commands", isCommand: true),
        Tag("Commands.ConveyorForward", "DO4 - Avancar Esteira", "Commands", isCommand: true),
        Tag("Commands.ConveyorBackward", "DO5 - Recuar Esteira", "Commands", isCommand: true),
        Tag("Commands.ActuateCylinder1", "DO6 - Acionar Cilindro 1", "Commands", isCommand: true),
        Tag("Commands.ActuateCylinder2", "DO7 - Acionar Cilindro 2", "Commands", isCommand: true),

        Tag("Counters.TotalProcessed", "Total de Pecas Processadas", "Counters", "Number", "pecas", "Total de pecas processadas", "0"),
        Tag("Counters.TotalApproved", "Total de Pecas Aprovadas", "Counters", "Number", "pecas", "Total de pecas aprovadas", "0"),
        Tag("Counters.TotalRejected", "Total de Pecas Rejeitadas", "Counters", "Number", "pecas", "Total de pecas rejeitadas", "0"),
        Tag("Counters.AverageCycleTime", "Tempo Medio de Ciclo", "Counters", "Number", "s", "Media do tempo de ciclo", "0"),
        Tag("Counters.LastProcessedPart", "Ultima Peca Processada", "Counters", "String", "", "Identificador ou estado da ultima peca", ""),
        Tag("Counters.MachineEfficiency", "Eficiencia da Maquina (%)", "Counters", "Number", "%", "Eficiencia calculada da maquina", "0"),

        Tag("Alarms.EmergencyActivated", "Emergencia Acionada", "Alarms"),
        Tag("Alarms.PartStuckEntry", "Peca Presa na Entrada", "Alarms"),
        Tag("Alarms.PartStuckExit", "Peca Presa na Saida", "Alarms"),
        Tag("Alarms.AxisXFailure", "Falha no Eixo X", "Alarms"),
        Tag("Alarms.AxisYFailure", "Falha no Eixo Y", "Alarms"),
        Tag("Alarms.AxisZFailure", "Falha no Eixo Z", "Alarms"),
        Tag("Alarms.VacuumCupFailure", "Falha da Ventosa", "Alarms"),
        Tag("Alarms.PlcCommunicationFailure", "Falha de Comunicacao com CLP", "Alarms"),
        Tag("Alarms.MqttCommunicationFailure", "Falha de Comunicacao MQTT", "Alarms")
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
