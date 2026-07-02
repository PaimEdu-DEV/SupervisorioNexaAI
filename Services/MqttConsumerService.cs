using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Dtos;
using SupervisorioSIMMAQ_NXA.Options;

namespace SupervisorioSIMMAQ_NXA.Services;

public class MqttConsumerService(
    IServiceScopeFactory scopeFactory,
    IOptions<MqttOptions> mqttOptions,
    ILogger<MqttConsumerService> logger) : BackgroundService
{
    private readonly MqttOptions _options = mqttOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("MQTT desabilitado. Altere Mqtt:Enabled para true quando o broker estiver pronto.");
            return;
        }

        if (_options.Topics.Count == 0)
        {
            logger.LogWarning("MQTT habilitado, mas nenhum topico foi configurado.");
            return;
        }

        var mqttFactory = new MqttFactory();
        using var mqttClient = mqttFactory.CreateMqttClient();

        mqttClient.ApplicationMessageReceivedAsync += async args =>
        {
            var payload = ReadPayload(args.ApplicationMessage.PayloadSegment);
            await SaveMessageAsync(args.ApplicationMessage.Topic, payload, stoppingToken);
        };

        var clientOptionsBuilder = new MqttClientOptionsBuilder()
            .WithClientId(_options.ClientId)
            .WithTcpServer(_options.Host, _options.Port)
            .WithCleanSession();

        if (!string.IsNullOrWhiteSpace(_options.Username))
        {
            clientOptionsBuilder.WithCredentials(_options.Username, _options.Password);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!mqttClient.IsConnected)
                {
                    await mqttClient.ConnectAsync(clientOptionsBuilder.Build(), stoppingToken);

                    foreach (var topic in _options.Topics)
                    {
                        await mqttClient.SubscribeAsync(topic, cancellationToken: stoppingToken);
                        logger.LogInformation("API inscrita no topico MQTT {Topic}.", topic);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Nao foi possivel conectar ao broker MQTT. Nova tentativa em 10 segundos.");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task SaveMessageAsync(string topic, string payload, CancellationToken cancellationToken)
    {
        MqttReadingPayload? parsedPayload = null;
        MqttTagPayload? parsedTagPayload = null;

        try
        {
            parsedPayload = JsonSerializer.Deserialize<MqttReadingPayload>(
                payload,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            parsedTagPayload = JsonSerializer.Deserialize<MqttTagPayload>(
                payload,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Payload MQTT recebido nao esta no formato JSON esperado. Topico: {Topic}", topic);
        }

        using var scope = scopeFactory.CreateScope();
        var tagValueService = scope.ServiceProvider.GetRequiredService<TagValueService>();

        if (!string.IsNullOrWhiteSpace(parsedTagPayload?.Tag) && parsedTagPayload.Value is not null)
        {
            await tagValueService.UpdateTagAsync(
                parsedTagPayload.Tag,
                parsedTagPayload.Value,
                "MQTT",
                topic,
                payload,
                parsedTagPayload.Timestamp,
                cancellationToken);

            return;
        }

        if (!string.IsNullOrWhiteSpace(parsedPayload?.Tag) && parsedPayload.Value is not null)
        {
            await tagValueService.UpdateTagAsync(
                parsedPayload.Tag,
                parsedPayload.Value.Value.ToString(),
                "MQTT",
                topic,
                payload,
                parsedPayload.CollectedAt,
                cancellationToken);
        }
    }

    private static string ReadPayload(ArraySegment<byte> payload)
    {
        if (payload.Array is null || payload.Count == 0)
        {
            return string.Empty;
        }

        return Encoding.UTF8.GetString(payload.Array, payload.Offset, payload.Count);
    }
}
