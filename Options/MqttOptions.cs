namespace SupervisorioSIMMAQ_NXA.Options;

public class MqttOptions
{
    public bool Enabled { get; set; }

    public string Host { get; set; } = "localhost";

    public int Port { get; set; } = 1883;

    public string ClientId { get; set; } = "supervisorio-simmaq-nxa-api";

    public string? Username { get; set; }

    public string? Password { get; set; }

    public List<string> Topics { get; set; } = [];
}
