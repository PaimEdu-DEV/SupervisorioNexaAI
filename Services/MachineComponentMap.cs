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
        new("mechanism_right_arm", "Braco Direito", "Mecanismo", "Actuators.AxisY.Move", "Conjunto de movimentacao do eixo Y.", 69, 39, 1.45, "Braco direito")
    ];

    private static readonly IReadOnlyDictionary<string, string> Aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Sensors.EntrySlotCapacitive"] = "sensor_entry_capacitive",
        ["Sensors.Entry.Capacitive"] = "sensor_entry_capacitive",
        ["Sensors.ExitSlotCapacitive"] = "sensor_exit_capacitive",
        ["Sensors.Exit.Capacitive"] = "sensor_exit_capacitive",
        ["Sensors.Inductive"] = "sensor_inductive",
        ["Sensors.Classification.Inductive"] = "sensor_inductive",
        ["Sensors.OpticalReflexive"] = "sensor_optical_reflective",
        ["Sensors.Classification.OpticalReflective"] = "sensor_optical_reflective",
        ["Sensors.OpticalMirror1"] = "sensor_optical_mirror_1",
        ["Sensors.Classification.OpticalMirror1"] = "sensor_optical_mirror_1",
        ["Sensors.OpticalMirror2"] = "sensor_optical_mirror_2",
        ["Sensors.Classification.OpticalMirror2"] = "sensor_optical_mirror_2",
        ["Sensors.AxisXRetracted"] = "sensor_axis_x_retracted",
        ["Sensors.AxisX.Retracted"] = "sensor_axis_x_retracted",
        ["Sensors.AxisXAdvanced"] = "sensor_axis_x_advanced",
        ["Sensors.AxisX.Advanced"] = "sensor_axis_x_advanced",
        ["Sensors.AxisYRetracted"] = "sensor_axis_y_retracted",
        ["Sensors.AxisY.Retracted"] = "sensor_axis_y_retracted",
        ["Sensors.AxisYAdvanced"] = "sensor_axis_y_advanced",
        ["Sensors.AxisY.Advanced"] = "sensor_axis_y_advanced",
        ["Sensors.AxisZRetracted"] = "sensor_axis_z_retracted",
        ["Sensors.AxisZ.Retracted"] = "sensor_axis_z_retracted",
        ["Sensors.AxisZAdvanced"] = "sensor_axis_z_advanced",
        ["Sensors.AxisZ.Advanced"] = "sensor_axis_z_advanced",
        ["Actuators.VacuumCupState"] = "actuator_vacuum",
        ["Actuators.Vacuum.On"] = "actuator_vacuum",
        ["Actuators.ConveyorState"] = "actuator_conveyor",
        ["Actuators.Conveyor.Forward"] = "actuator_conveyor",
        ["Actuators.DiscardCylinder1State"] = "actuator_reject_cylinder_1",
        ["Actuators.RejectCylinder1.Advance"] = "actuator_reject_cylinder_1",
        ["Actuators.DiscardCylinder2State"] = "actuator_reject_cylinder_2",
        ["Actuators.RejectCylinder2.Advance"] = "actuator_reject_cylinder_2",
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
        var haystack = Normalize($"{component.Id} {component.Name} {component.Tag} {component.Type} {component.Area}");
        return haystack
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(token => token.Length > 2)
            .Distinct()
            .Count(normalized.Contains);
    }

    private static string Normalize(string value) => value
        .ToLowerInvariant()
        .Replace("_", " ")
        .Replace(".", " ")
        .Replace("-", " ");
}
