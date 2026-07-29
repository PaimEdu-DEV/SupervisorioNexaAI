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
        A maquina Nexa representa o CLASSIFICADOR DE PECA da bancada SIMMAQ. Ela recebe uma peca no slot
        de entrada, confirma posicoes dos eixos pneumaticos por sensores magneticos, movimenta a peca com
        ventosa, transporta pela esteira e classifica usando sensores indutivo e opticos. O CLP Schneider M221
        aciona eixos, ventosa, esteira e cilindros de descarte conforme os sinais de entrada DI e saidas DO.
        A IA deve sempre explicar funcao, localizacao, estado atual e caminho de diagnostico, nao apenas dizer
        que um ponto esta ativo ou inativo.
        """;

    public const string OperationalCycle = """
        Fluxo operacional: operador coloca a peca no slot de entrada; DI0 confirma presenca; o ciclo pode ser
        iniciado pelo botao DI18; os eixos X, Y e Z deslocam a peca usando DO0/DO1/DO2 ou DO8/DO9/DO10 conforme
        o conjunto acionado; a ventosa e acionada por DO3 ou DO11; a esteira avanca por DO4 e pode recuar por DO5;
        DI7 identifica caracteristica metalica; DI8, DI9 e DI10 fazem leitura optica; o CLP decide a classificacao;
        DO6 ou DO7 avanca o cilindro de descarte correspondente; DI11 confirma peca no slot de saida. Se DI20
        estiver ativo, nenhuma saida deve ser considerada liberada ate a condicao de emergencia ser removida e
        o reset DI19 ser usado conforme a logica do CLP.
        """;

    public const string DiagnosticKnowledge = """
        Regras de diagnostico: se DI7 detectar peca e DO4 estiver inativo durante uma etapa de transporte, verificar
        emergencia, modo manual, intertravamentos e comando da esteira. Se sensores magneticos recuado e avancado
        do mesmo eixo estiverem ativos ao mesmo tempo, suspeitar de sensor travado, curto no sinal ou erro de ligacao.
        Se uma saida DO estiver acionada e o sensor de fim de curso esperado nao mudar, verificar valvula, pressao
        pneumatica, cilindro, sensor magnetico, cabo e entrada do CLP. Se DI20 emergencia estiver ativo, explicar que
        os atuadores devem permanecer bloqueados e orientar reset apenas depois da causa removida.
        """;

    public static IReadOnlyList<TechnicalComponent> Components { get; } =
    [
        new("sensor_entry_capacitive", "DI0 - Sensor Capacitivo, peca no slot de entrada", "Entrada Digital", "Sensors.Entry.Capacitive", "40001.0 / DI0 / %MW0:X0", "Detecta a presenca de peca no slot de entrada e permite iniciar a sequencia de classificacao."),
        new("sensor_axis_x_retracted", "DI1 - Sensor Magnetico, eixo X recuado", "Entrada Digital", "Sensors.AxisX.Retracted", "40001.0 / DI1 / %MW0:X1", "Confirma que o eixo X esta na posicao recuada antes ou depois do deslocamento."),
        new("sensor_axis_x_advanced", "DI2 - Sensor Magnetico, eixo X avancado", "Entrada Digital", "Sensors.AxisX.Advanced", "40001.0 / DI2 / %MW0:X2", "Confirma que o eixo X chegou ao fim de curso avancado."),
        new("sensor_axis_y_retracted", "DI3 - Sensor Magnetico, eixo Y recuado", "Entrada Digital", "Sensors.AxisY.Retracted", "40001.0 / DI3 / %MW0:X3", "Confirma que o eixo Y esta recuado e pronto para a proxima etapa."),
        new("sensor_axis_y_advanced", "DI4 - Sensor Magnetico, eixo Y avancado", "Entrada Digital", "Sensors.AxisY.Advanced", "40001.0 / DI4 / %MW0:X4", "Confirma que o eixo Y avancou corretamente durante o ciclo."),
        new("sensor_axis_z_retracted", "DI5 - Sensor Magnetico, eixo Z recuado", "Entrada Digital", "Sensors.AxisZ.Retracted", "40001.0 / DI5 / %MW0:X5", "Confirma que o eixo Z esta recolhido, evitando colisao no deslocamento."),
        new("sensor_axis_z_advanced", "DI6 - Sensor Magnetico, eixo Z avancado", "Entrada Digital", "Sensors.AxisZ.Advanced", "40001.0 / DI6 / %MW0:X6", "Confirma que o eixo Z avancou para pegar ou posicionar a peca."),
        new("sensor_inductive", "DI7 - Sensor Indutivo", "Entrada Digital", "Sensors.Classification.Inductive", "40001.0 / DI7 / %MW0:X7", "Detecta caracteristica metalica da peca na etapa de classificacao."),
        new("sensor_optical_reflective", "DI8 - Sensor Otico Reflexivo", "Entrada Digital", "Sensors.Classification.OpticalReflective", "40001.0 / DI8 / %MW0:X8", "Detecta presenca ou passagem da peca na esteira por leitura optica reflexiva."),
        new("sensor_optical_mirror_1", "DI9 - Sensor Otico com Espelho Refletor 1", "Entrada Digital", "Sensors.Classification.OpticalMirror1", "40001.0 / DI9 / %MW0:X9", "Faz leitura optica com refletor para apoiar a classificacao da peca."),
        new("sensor_optical_mirror_2", "DI10 - Sensor Otico com Espelho Refletor 2", "Entrada Digital", "Sensors.Classification.OpticalMirror2", "40001.0 / DI10 / %MW0:X10", "Segundo ponto de leitura optica com refletor na classificacao."),
        new("sensor_exit_capacitive", "DI11 - Sensor Capacitivo, peca no slot de saida", "Entrada Digital", "Sensors.Exit.Capacitive", "40001.0 / DI11 / %MW0:X11", "Confirma que a peca chegou ao slot de saida."),
        new("sensor_axis_x_retracted_b", "DI12 - Sensor Magnetico, eixo X recuado", "Entrada Digital", "Sensors.AxisX.Retracted.B", "40001.0 / DI12 / %MW0:X12", "Segundo ponto de confirmacao do eixo X recuado conforme tabela do classificador."),
        new("sensor_axis_x_advanced_b", "DI13 - Sensor Magnetico, eixo X avancado", "Entrada Digital", "Sensors.AxisX.Advanced.B", "40001.0 / DI13 / %MW0:X13", "Segundo ponto de confirmacao do eixo X avancado conforme tabela do classificador."),
        new("sensor_axis_y_retracted_b", "DI14 - Sensor Magnetico, eixo Y recuado", "Entrada Digital", "Sensors.AxisY.Retracted.B", "40001.0 / DI14 / %MW0:X14", "Segundo ponto de confirmacao do eixo Y recuado conforme tabela do classificador."),
        new("sensor_axis_y_advanced_b", "DI15 - Sensor Magnetico, eixo Y avancado", "Entrada Digital", "Sensors.AxisY.Advanced.B", "40001.0 / DI15 / %MW0:X15", "Segundo ponto de confirmacao do eixo Y avancado conforme tabela do classificador."),
        new("sensor_axis_z_retracted_b", "DI16 - Sensor Magnetico, eixo Z recuado", "Entrada Digital", "Sensors.AxisZ.Retracted.B", "40002.0 / DI16 / %MW1:X0", "Segundo ponto de confirmacao do eixo Z recuado conforme tabela do classificador."),
        new("sensor_axis_z_advanced_b", "DI17 - Sensor Magnetico, eixo Z avancado", "Entrada Digital", "Sensors.AxisZ.Advanced.B", "40002.0 / DI17 / %MW1:X1", "Segundo ponto de confirmacao do eixo Z avancado conforme tabela do classificador."),
        new("button_start", "DI18 - Botao Inicio", "Entrada Digital", "Buttons.Start", "40002.0 / DI18 / %MW1:X2", "Botao fisico que solicita inicio do ciclo automatico ou etapa manual."),
        new("button_reset", "DI19 - Botao Reset", "Entrada Digital", "Buttons.Reset", "40002.0 / DI19 / %MW1:X3", "Botao usado para resetar falhas ou retornar a maquina a uma condicao segura."),
        new("button_emergency", "DI20 - Botao Emergencia", "Entrada Digital", "Buttons.Emergency", "40002.0 / DI20 / %MW1:X4", "Entrada de emergencia. Quando ativa, atuadores devem permanecer bloqueados por seguranca."),

        new("mechanism_left_arm", "DO0 - Desloca eixo X", "Saida Digital", "Actuators.AxisX.Move", "40003.0 / DO0 / %MW2:X0", "Aciona o deslocamento do eixo X."),
        new("mechanism_right_arm", "DO1 - Desloca eixo Y", "Saida Digital", "Actuators.AxisY.Move", "40003.0 / DO1 / %MW2:X1", "Aciona o deslocamento do eixo Y."),
        new("mechanism_z_axis", "DO2 - Desloca eixo Z", "Saida Digital", "Actuators.AxisZ.Move", "40003.0 / DO2 / %MW2:X2", "Aciona o deslocamento do eixo Z."),
        new("actuator_vacuum", "DO3 - Aciona ventosa", "Saida Digital", "Actuators.Vacuum.On", "40003.0 / DO3 / %MW2:X3", "Aciona a ventosa responsavel por pegar, segurar e transportar a peca."),
        new("actuator_conveyor_forward", "DO4 - Esteira avanca", "Saida Digital", "Actuators.Conveyor.Forward", "40003.0 / DO4 / %MW2:X4", "Move a esteira no sentido de avanco para transportar a peca."),
        new("actuator_conveyor_reverse", "DO5 - Esteira recua", "Saida Digital", "Actuators.Conveyor.Reverse", "40003.0 / DO5 / %MW2:X5", "Move a esteira no sentido de retorno quando a logica exigir."),
        new("actuator_reject_cylinder_1", "DO6 - Avanca cilindro de descarte 1", "Saida Digital", "Actuators.RejectCylinder1.Advance", "40003.0 / DO6 / %MW2:X6", "Aciona o primeiro cilindro de descarte para separar a peca classificada."),
        new("actuator_reject_cylinder_2", "DO7 - Avanca cilindro de descarte 2", "Saida Digital", "Actuators.RejectCylinder2.Advance", "40003.0 / DO7 / %MW2:X7", "Aciona o segundo cilindro de descarte para separar outro tipo de peca."),
        new("mechanism_left_arm_b", "DO8 - Desloca eixo X", "Saida Digital", "Actuators.AxisX.Move.B", "40003.0 / DO8 / %MW2:X8", "Segundo comando de deslocamento do eixo X conforme tabela do classificador."),
        new("mechanism_right_arm_b", "DO9 - Desloca eixo Y", "Saida Digital", "Actuators.AxisY.Move.B", "40003.0 / DO9 / %MW2:X9", "Segundo comando de deslocamento do eixo Y conforme tabela do classificador."),
        new("mechanism_z_axis_b", "DO10 - Desloca eixo Z", "Saida Digital", "Actuators.AxisZ.Move.B", "40003.0 / DO10 / %MW2:X10", "Segundo comando de deslocamento do eixo Z conforme tabela do classificador."),
        new("actuator_vacuum_b", "DO11 - Aciona ventosa", "Saida Digital", "Actuators.Vacuum.On.B", "40003.0 / DO11 / %MW2:X11", "Segundo comando de ventosa conforme tabela do classificador."),
        new("actuator_conveyor_speed", "AO0 - Set de velocidade da esteira", "Saida Analogica", "Actuators.Conveyor.SpeedSetpoint", "40006.0 / AO0 / %MW5", "Define o setpoint de velocidade da esteira em faixa analogica 0 a 255.")
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
        var questionTokens = NormalizeTokens(normalized);
        var text = Normalize($"{component.Name} {component.Tag} {component.Address} {component.ComponentId}");
        return text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(token => token.Length > 2)
            .Distinct()
            .Count(questionTokens.Contains);
    }

    private static HashSet<string> NormalizeTokens(string value) =>
        Normalize(value)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static string Normalize(string value) => value
        .ToLowerInvariant()
        .Replace("_", " ")
        .Replace(".", " ")
        .Replace("-", " ")
        .Replace("%", " ")
        .Replace("/", " ")
        .Replace(":", " ")
        .Replace("?", " ")
        .Replace("!", " ")
        .Replace(",", " ")
        .Replace(";", " ");
}
