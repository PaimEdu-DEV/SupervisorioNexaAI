namespace SupervisorioSIMMAQ_NXA.Services;

public record MachineComponent(
    string Id,
    string Name,
    string Type,
    string Tag,
    string Description,
    double XPercent,
    double YPercent,
    double ZoomLevel,
    string Area)
{
    public double X => XPercent;
    public double Y => YPercent;
    public double Zoom => ZoomLevel;
}

public class MachineComponentMap
{
    private static readonly IReadOnlyList<MachineComponent> Components =
    [
        new("sensor_entry_capacitive", "Sensor Capacitivo - Entrada", "Sensor", "Sensors.Entry.Capacitive", "Detecta presenca de peca no slot de entrada.", 22, 58, 1.55, "Entrada"),
        new("sensor_exit_capacitive", "Sensor Capacitivo - Saida", "Sensor", "Sensors.Exit.Capacitive", "Detecta presenca de peca no slot de saida.", 77, 58, 1.55, "Saida"),
        new("sensor_axis_x_retracted", "Sensor Magnetico - Eixo X Recuado", "Sensor", "Sensors.AxisX.Retracted", "Confirma eixo X na posicao recuada.", 30, 42, 1.7, "Braco esquerdo"),
        new("sensor_axis_x_advanced", "Sensor Magnetico - Eixo X Avancado", "Sensor", "Sensors.AxisX.Advanced", "Confirma eixo X na posicao avancada.", 39, 42, 1.7, "Braco esquerdo"),
        new("sensor_axis_y_retracted", "Sensor Magnetico - Eixo Y Recuado", "Sensor", "Sensors.AxisY.Retracted", "Confirma eixo Y na posicao recuada.", 61, 42, 1.7, "Braco direito"),
        new("sensor_axis_y_advanced", "Sensor Magnetico - Eixo Y Avancado", "Sensor", "Sensors.AxisY.Advanced", "Confirma eixo Y na posicao avancada.", 70, 42, 1.7, "Braco direito"),
        new("sensor_axis_z_retracted", "Sensor Magnetico - Eixo Z Recuado", "Sensor", "Sensors.AxisZ.Retracted", "Confirma eixo Z na posicao recuada.", 50, 35, 1.8, "Centro"),
        new("sensor_axis_z_advanced", "Sensor Magnetico - Eixo Z Avancado", "Sensor", "Sensors.AxisZ.Advanced", "Confirma eixo Z na posicao avancada.", 50, 43, 1.8, "Centro"),
        new("sensor_axis_x_retracted_b", "Sensor Magnetico - Eixo X Recuado B", "Sensor", "Sensors.AxisX.Retracted.B", "Segundo ponto de confirmacao do eixo X recuado.", 31, 49, 1.7, "Braco esquerdo"),
        new("sensor_axis_x_advanced_b", "Sensor Magnetico - Eixo X Avancado B", "Sensor", "Sensors.AxisX.Advanced.B", "Segundo ponto de confirmacao do eixo X avancado.", 39, 49, 1.7, "Braco esquerdo"),
        new("sensor_axis_y_retracted_b", "Sensor Magnetico - Eixo Y Recuado B", "Sensor", "Sensors.AxisY.Retracted.B", "Segundo ponto de confirmacao do eixo Y recuado.", 61, 49, 1.7, "Braco direito"),
        new("sensor_axis_y_advanced_b", "Sensor Magnetico - Eixo Y Avancado B", "Sensor", "Sensors.AxisY.Advanced.B", "Segundo ponto de confirmacao do eixo Y avancado.", 70, 49, 1.7, "Braco direito"),
        new("sensor_axis_z_retracted_b", "Sensor Magnetico - Eixo Z Recuado B", "Sensor", "Sensors.AxisZ.Retracted.B", "Segundo ponto de confirmacao do eixo Z recuado.", 46, 35, 1.8, "Centro"),
        new("sensor_axis_z_advanced_b", "Sensor Magnetico - Eixo Z Avancado B", "Sensor", "Sensors.AxisZ.Advanced.B", "Segundo ponto de confirmacao do eixo Z avancado.", 54, 43, 1.8, "Centro"),
        new("sensor_inductive", "Sensor Indutivo", "Sensor", "Sensors.Classification.Inductive", "Detecta caracteristica metalica da peca.", 48, 56, 1.7, "Centro"),
        new("sensor_optical_reflective", "Sensor Optico Reflexivo", "Sensor", "Sensors.Classification.OpticalReflective", "Sensor optico usado na identificacao e classificacao da peca.", 54, 50, 1.7, "Centro"),
        new("sensor_optical_mirror_1", "Sensor Optico com Espelho 1", "Sensor", "Sensors.Classification.OpticalMirror1", "Sensor optico com espelho refletor utilizado no processo de classificacao.", 39, 51, 1.65, "Centro"),
        new("sensor_optical_mirror_2", "Sensor Optico com Espelho 2", "Sensor", "Sensors.Classification.OpticalMirror2", "Segundo sensor optico com espelho refletor utilizado no processo de classificacao.", 63, 51, 1.65, "Centro"),
        new("actuator_vacuum", "Ventosa", "Atuador", "Actuators.Vacuum.On", "Ventosa responsavel por pegar e transportar a peca.", 50, 49, 1.8, "Centro"),
        new("actuator_conveyor", "Esteira", "Atuador", "Actuators.Conveyor.Forward", "Transporta pecas entre as estacoes.", 50, 58, 1.5, "Centro"),
        new("actuator_reject_cylinder_1", "Cilindro de Descarte 1", "Atuador", "Actuators.RejectCylinder1.Advance", "Primeiro cilindro de descarte.", 34, 54, 1.7, "Braco esquerdo"),
        new("actuator_reject_cylinder_2", "Cilindro de Descarte 2", "Atuador", "Actuators.RejectCylinder2.Advance", "Segundo cilindro de descarte.", 67, 54, 1.7, "Braco direito"),
        new("button_start", "Botao Inicio", "Botao", "Buttons.Start", "Botao fisico de inicio do ciclo.", 17, 36, 1.55, "Entrada"),
        new("button_reset", "Botao Reset", "Botao", "Buttons.Reset", "Botao fisico para reset de falhas ou ciclo.", 20, 36, 1.55, "Entrada"),
        new("button_emergency", "Botao Emergencia", "Botao", "Buttons.Emergency", "Botao fisico de emergencia da bancada.", 83, 36, 1.55, "Saida"),
        new("mechanism_left_arm", "Braco Esquerdo", "Mecanismo", "Actuators.AxisX.Move", "Conjunto de movimentacao do eixo X.", 31, 39, 1.45, "Braco esquerdo"),
        new("mechanism_right_arm", "Braco Direito", "Mecanismo", "Actuators.AxisY.Move", "Conjunto de movimentacao do eixo Y.", 69, 39, 1.45, "Braco direito"),
        new("mechanism_left_arm_b", "Braco Esquerdo B", "Mecanismo", "Actuators.AxisX.Move.B", "Segundo comando de movimentacao do eixo X.", 31, 46, 1.45, "Braco esquerdo"),
        new("mechanism_right_arm_b", "Braco Direito B", "Mecanismo", "Actuators.AxisY.Move.B", "Segundo comando de movimentacao do eixo Y.", 69, 46, 1.45, "Braco direito"),
        new("mechanism_z_axis_b", "Eixo Z B", "Mecanismo", "Actuators.AxisZ.Move.B", "Segundo comando de movimentacao do eixo Z.", 52, 39, 1.55, "Centro"),
        new("actuator_vacuum_b", "Ventosa B", "Atuador", "Actuators.Vacuum.On.B", "Segundo comando de acionamento da ventosa.", 52, 52, 1.8, "Centro")
    ];

    private static readonly IReadOnlyDictionary<string, string> Aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Sensors.EntrySlotCapacitive"] = "sensor_entry_capacitive",
        ["Sensors.Entry.Capacitive"] = "sensor_entry_capacitive",
        ["DI0"] = "sensor_entry_capacitive",
        ["Sensors.ExitSlotCapacitive"] = "sensor_exit_capacitive",
        ["Sensors.Exit.Capacitive"] = "sensor_exit_capacitive",
        ["DI11"] = "sensor_exit_capacitive",
        ["Sensors.Inductive"] = "sensor_inductive",
        ["Sensors.Classification.Inductive"] = "sensor_inductive",
        ["DI7"] = "sensor_inductive",
        ["Sensors.OpticalReflexive"] = "sensor_optical_reflective",
        ["Sensors.Classification.OpticalReflective"] = "sensor_optical_reflective",
        ["DI8"] = "sensor_optical_reflective",
        ["Sensors.OpticalMirror1"] = "sensor_optical_mirror_1",
        ["Sensors.Classification.OpticalMirror1"] = "sensor_optical_mirror_1",
        ["DI9"] = "sensor_optical_mirror_1",
        ["Sensors.OpticalMirror2"] = "sensor_optical_mirror_2",
        ["Sensors.Classification.OpticalMirror2"] = "sensor_optical_mirror_2",
        ["DI10"] = "sensor_optical_mirror_2",
        ["Sensors.AxisXRetracted"] = "sensor_axis_x_retracted",
        ["Sensors.AxisX.Retracted"] = "sensor_axis_x_retracted",
        ["DI1"] = "sensor_axis_x_retracted",
        ["Sensors.AxisXAdvanced"] = "sensor_axis_x_advanced",
        ["Sensors.AxisX.Advanced"] = "sensor_axis_x_advanced",
        ["DI2"] = "sensor_axis_x_advanced",
        ["Sensors.AxisYRetracted"] = "sensor_axis_y_retracted",
        ["Sensors.AxisY.Retracted"] = "sensor_axis_y_retracted",
        ["DI3"] = "sensor_axis_y_retracted",
        ["Sensors.AxisYAdvanced"] = "sensor_axis_y_advanced",
        ["Sensors.AxisY.Advanced"] = "sensor_axis_y_advanced",
        ["DI4"] = "sensor_axis_y_advanced",
        ["Sensors.AxisZRetracted"] = "sensor_axis_z_retracted",
        ["Sensors.AxisZ.Retracted"] = "sensor_axis_z_retracted",
        ["DI5"] = "sensor_axis_z_retracted",
        ["Sensors.AxisZAdvanced"] = "sensor_axis_z_advanced",
        ["Sensors.AxisZ.Advanced"] = "sensor_axis_z_advanced",
        ["DI6"] = "sensor_axis_z_advanced",
        ["Sensors.AxisXRetractedB"] = "sensor_axis_x_retracted_b",
        ["Sensors.AxisX.Retracted.B"] = "sensor_axis_x_retracted_b",
        ["DI12"] = "sensor_axis_x_retracted_b",
        ["Sensors.AxisXAdvancedB"] = "sensor_axis_x_advanced_b",
        ["Sensors.AxisX.Advanced.B"] = "sensor_axis_x_advanced_b",
        ["DI13"] = "sensor_axis_x_advanced_b",
        ["Sensors.AxisYRetractedB"] = "sensor_axis_y_retracted_b",
        ["Sensors.AxisY.Retracted.B"] = "sensor_axis_y_retracted_b",
        ["DI14"] = "sensor_axis_y_retracted_b",
        ["Sensors.AxisYAdvancedB"] = "sensor_axis_y_advanced_b",
        ["Sensors.AxisY.Advanced.B"] = "sensor_axis_y_advanced_b",
        ["DI15"] = "sensor_axis_y_advanced_b",
        ["Sensors.AxisZRetractedB"] = "sensor_axis_z_retracted_b",
        ["Sensors.AxisZ.Retracted.B"] = "sensor_axis_z_retracted_b",
        ["DI16"] = "sensor_axis_z_retracted_b",
        ["Sensors.AxisZAdvancedB"] = "sensor_axis_z_advanced_b",
        ["Sensors.AxisZ.Advanced.B"] = "sensor_axis_z_advanced_b",
        ["DI17"] = "sensor_axis_z_advanced_b",
        ["Actuators.VacuumCupState"] = "actuator_vacuum",
        ["Actuators.Vacuum.On"] = "actuator_vacuum",
        ["DO3"] = "actuator_vacuum",
        ["Actuators.ConveyorState"] = "actuator_conveyor",
        ["Actuators.Conveyor.Forward"] = "actuator_conveyor",
        ["DO4"] = "actuator_conveyor",
        ["Actuators.Conveyor.Reverse"] = "actuator_conveyor",
        ["DO5"] = "actuator_conveyor",
        ["Actuators.DiscardCylinder1State"] = "actuator_reject_cylinder_1",
        ["Actuators.RejectCylinder1.Advance"] = "actuator_reject_cylinder_1",
        ["DO6"] = "actuator_reject_cylinder_1",
        ["Actuators.DiscardCylinder2State"] = "actuator_reject_cylinder_2",
        ["Actuators.RejectCylinder2.Advance"] = "actuator_reject_cylinder_2",
        ["DO7"] = "actuator_reject_cylinder_2",
        ["Actuators.AxisX.Move"] = "mechanism_left_arm",
        ["DO0"] = "mechanism_left_arm",
        ["Actuators.AxisY.Move"] = "mechanism_right_arm",
        ["DO1"] = "mechanism_right_arm",
        ["Actuators.AxisZ.Move"] = "mechanism_z_axis_b",
        ["DO2"] = "mechanism_z_axis_b",
        ["Actuators.AxisXMoveB"] = "mechanism_left_arm_b",
        ["Actuators.AxisX.Move.B"] = "mechanism_left_arm_b",
        ["DO8"] = "mechanism_left_arm_b",
        ["Actuators.AxisYMoveB"] = "mechanism_right_arm_b",
        ["Actuators.AxisY.Move.B"] = "mechanism_right_arm_b",
        ["DO9"] = "mechanism_right_arm_b",
        ["Actuators.AxisZMoveB"] = "mechanism_z_axis_b",
        ["Actuators.AxisZ.Move.B"] = "mechanism_z_axis_b",
        ["DO10"] = "mechanism_z_axis_b",
        ["Actuators.VacuumOnB"] = "actuator_vacuum_b",
        ["Actuators.Vacuum.On.B"] = "actuator_vacuum_b",
        ["DO11"] = "actuator_vacuum_b",
        ["Commands.Start"] = "button_start",
        ["Buttons.Start"] = "button_start",
        ["Commands.Reset"] = "button_reset",
        ["Buttons.Reset"] = "button_reset",
        ["Commands.Emergency"] = "button_emergency",
        ["Buttons.Emergency"] = "button_emergency"
    };

    public IReadOnlyList<MachineComponent> GetAll() => Components;

    public MachineComponent? Find(string? componentId)
    {
        if (string.IsNullOrWhiteSpace(componentId))
        {
            return null;
        }

        var normalizedId = Aliases.TryGetValue(componentId, out var mappedId) ? mappedId : componentId;
        return Components.FirstOrDefault(component =>
            component.Id.Equals(normalizedId, StringComparison.OrdinalIgnoreCase) ||
            component.Id.Equals(componentId, StringComparison.OrdinalIgnoreCase) ||
            component.Tag.Equals(componentId, StringComparison.OrdinalIgnoreCase));
    }

    public MachineComponent? FindByText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var normalized = Normalize(text);
        return Components
            .Select(component => new { Component = component, Score = Score(component, normalized) })
            .OrderByDescending(item => item.Score)
            .FirstOrDefault(item => item.Score > 0)
            ?.Component;
    }

    private static int Score(MachineComponent component, string normalized)
    {
        var questionTokens = Normalize(normalized)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var haystack = Normalize($"{component.Id} {component.Name} {component.Tag} {component.Type} {component.Area}");
        return haystack
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(token => token.Length > 2)
            .Distinct()
            .Count(questionTokens.Contains);
    }

    private static string Normalize(string value) => value
        .ToLowerInvariant()
        .Replace("_", " ")
        .Replace(".", " ")
        .Replace("-", " ")
        .Replace("/", " ")
        .Replace(":", " ")
        .Replace("?", " ")
        .Replace("!", " ")
        .Replace(",", " ")
        .Replace(";", " ");
}
