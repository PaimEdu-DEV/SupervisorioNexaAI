namespace SupervisorioSIMMAQ_NXA.Services;

public record TechnicalComponent(
    string ComponentId,
    string Name,
    string Type,
    string Tag,
    string Address,
    string Description);

public static class MachineTechnicalKnowledge
{
    public const string MachineDescription = """
        A maquina Nexa e uma bancada didatica classificadora de pecas. Ela utiliza uma estacao de entrada,
        eixos pneumaticos, ventosa, esteira transportadora, sensores capacitivos, indutivos, opticos e
        magneticos para detectar, transportar e classificar pecas. O sistema e supervisionado pela API C#,
        MQTT, banco de dados, motor de diagnostico industrial, servico local de IA e front-end Nexa.
        """;

    public const string OperationalCycle = """
        Ciclo resumido: a peca e detectada no slot de entrada, o conjunto pneumatico movimenta a peca,
        a ventosa realiza a pega, a esteira transporta a peca pela area de classificacao, sensores indutivos
        e opticos identificam caracteristicas da peca, e cilindros de descarte podem separar pecas conforme a regra.
        """;

    public static IReadOnlyList<TechnicalComponent> Components { get; } =
    [
        new("sensor_entry_capacitive", "Sensor Capacitivo - Peca no Slot de Entrada", "Entrada Digital", "Sensors.Entry.Capacitive", "DI0 / %MW0:X0 / Modbus 40001", "Detecta presenca de peca no slot de entrada."),
        new("sensor_axis_x_retracted", "Sensor Magnetico - Eixo X Recuado", "Entrada Digital", "Sensors.AxisX.Retracted", "DI1 / %MW0:X1 / Modbus 40001", "Confirma eixo X na posicao recuada."),
        new("sensor_axis_x_advanced", "Sensor Magnetico - Eixo X Avancado", "Entrada Digital", "Sensors.AxisX.Advanced", "DI2 / %MW0:X2 / Modbus 40001", "Confirma eixo X na posicao avancada."),
        new("sensor_axis_y_retracted", "Sensor Magnetico - Eixo Y Recuado", "Entrada Digital", "Sensors.AxisY.Retracted", "DI3 / %MW0:X3 / Modbus 40001", "Confirma eixo Y na posicao recuada."),
        new("sensor_axis_y_advanced", "Sensor Magnetico - Eixo Y Avancado", "Entrada Digital", "Sensors.AxisY.Advanced", "DI4 / %MW0:X4 / Modbus 40001", "Confirma eixo Y na posicao avancada."),
        new("sensor_axis_z_retracted", "Sensor Magnetico - Eixo Z Recuado", "Entrada Digital", "Sensors.AxisZ.Retracted", "DI5 / %MW0:X5 / Modbus 40001", "Confirma eixo Z na posicao recuada."),
        new("sensor_axis_z_advanced", "Sensor Magnetico - Eixo Z Avancado", "Entrada Digital", "Sensors.AxisZ.Advanced", "DI6 / %MW0:X6 / Modbus 40001", "Confirma eixo Z na posicao avancada."),
        new("sensor_inductive", "Sensor Indutivo", "Entrada Digital", "Sensors.Classification.Inductive", "DI7 / %MW0:X7 / Modbus 40001", "Detecta caracteristica metalica da peca."),
        new("sensor_optical_reflective", "Sensor Optico Reflexivo", "Entrada Digital", "Sensors.Classification.OpticalReflective", "DI8 / %MW0:X8 / Modbus 40001", "Sensor optico usado na identificacao e classificacao da peca."),
        new("sensor_optical_mirror_1", "Sensor Optico com Espelho Refletor 1", "Entrada Digital", "Sensors.Classification.OpticalMirror1", "DI9 / %MW0:X9 / Modbus 40001", "Sensor optico com espelho refletor utilizado no processo de classificacao."),
        new("sensor_optical_mirror_2", "Sensor Optico com Espelho Refletor 2", "Entrada Digital", "Sensors.Classification.OpticalMirror2", "DI10 / %MW0:X10 / Modbus 40001", "Segundo sensor optico com espelho refletor utilizado no processo de classificacao."),
        new("sensor_exit_capacitive", "Sensor Capacitivo - Peca no Slot de Saida", "Entrada Digital", "Sensors.Exit.Capacitive", "DI11 / %MW0:X11 / Modbus 40001", "Detecta presenca de peca no slot de saida."),
        new("button_start", "Botao Inicio", "Entrada Digital", "Buttons.Start", "DI18 / %MW1:X2 / Modbus 40002", "Botao fisico de inicio do ciclo."),
        new("button_reset", "Botao Reset", "Entrada Digital", "Buttons.Reset", "DI19 / %MW1:X3 / Modbus 40002", "Botao fisico para reset de falhas ou ciclo."),
        new("button_emergency", "Botao Emergencia", "Entrada Digital", "Buttons.Emergency", "DI20 / %MW1:X4 / Modbus 40002", "Botao fisico de emergencia da bancada."),
        new("mechanism_left_arm", "Desloca Eixo X", "Saida Digital", "Actuators.AxisX.Move", "DO0 / %MW2:X0 / Modbus 40003", "Aciona o deslocamento do eixo X."),
        new("mechanism_right_arm", "Desloca Eixo Y", "Saida Digital", "Actuators.AxisY.Move", "DO1 / %MW2:X1 / Modbus 40003", "Aciona o deslocamento do eixo Y."),
        new("sensor_axis_z_advanced", "Desloca Eixo Z", "Saida Digital", "Actuators.AxisZ.Move", "DO2 / %MW2:X2 / Modbus 40003", "Aciona o deslocamento do eixo Z."),
        new("actuator_vacuum", "Aciona Ventosa", "Saida Digital", "Actuators.Vacuum.On", "DO3 / %MW2:X3 / Modbus 40003", "Aciona a ventosa responsavel por pegar a peca."),
        new("actuator_conveyor", "Esteira Avanca", "Saida Digital", "Actuators.Conveyor.Forward", "DO4 / %MW2:X4 / Modbus 40003", "Aciona a esteira no sentido de avanco."),
        new("actuator_conveyor", "Esteira Recua", "Saida Digital", "Actuators.Conveyor.Reverse", "DO5 / %MW2:X5 / Modbus 40003", "Aciona a esteira no sentido reverso."),
        new("actuator_reject_cylinder_1", "Avanca Cilindro de Descarte 1", "Saida Digital", "Actuators.RejectCylinder1.Advance", "DO6 / %MW2:X6 / Modbus 40003", "Aciona o primeiro cilindro de descarte."),
        new("actuator_reject_cylinder_2", "Avanca Cilindro de Descarte 2", "Saida Digital", "Actuators.RejectCylinder2.Advance", "DO7 / %MW2:X7 / Modbus 40003", "Aciona o segundo cilindro de descarte."),
        new("actuator_conveyor", "Set de Velocidade da Esteira", "Saida Analogica", "Actuators.Conveyor.SpeedSetpoint", "AO0 / %MW5 / Modbus 40006", "Define a velocidade da esteira.")
    ];

    public static TechnicalComponent? FindByText(string question)
    {
        var normalized = Normalize(question);
        return Components
            .Select(component => new { Component = component, Score = Score(component, normalized) })
            .OrderByDescending(item => item.Score)
            .FirstOrDefault(item => item.Score > 0)
            ?.Component;
    }

    private static int Score(TechnicalComponent component, string normalized)
    {
        var text = Normalize($"{component.Name} {component.Tag} {component.Address} {component.ComponentId}");
        return text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(token => token.Length > 2)
            .Distinct()
            .Count(normalized.Contains);
    }

    private static string Normalize(string value) => value
        .ToLowerInvariant()
        .Replace("_", " ")
        .Replace(".", " ")
        .Replace("-", " ")
        .Replace("%", " ");
}
