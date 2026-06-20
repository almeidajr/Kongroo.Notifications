using System.Net;
using Testcontainers.RabbitMq;

namespace Kongroo.Notifications.Specs.Support;

public static class SpecsEnvironment
{
    private const string RabbitMqImage = "rabbitmq:4-management";
    private const string RabbitMqUsername = "kongroo";
    private const string RabbitMqPassword = "development";

    private static readonly SemaphoreSlim Gate = new(1, 1);
    private static RabbitMqContainer? _broker;
    private static KongrooWebApplicationFactory? _factory;

    public static KongrooWebApplicationFactory Factory =>
        _factory ?? throw new InvalidOperationException("The specs environment has not been started.");

    public static async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_factory is not null)
        {
            return;
        }

        await Gate.WaitAsync(cancellationToken);
        try
        {
            if (_factory is not null)
            {
                return;
            }

            _broker = new RabbitMqBuilder(RabbitMqImage)
                .WithUsername(RabbitMqUsername)
                .WithPassword(RabbitMqPassword)
                .Build();

            await _broker.StartAsync(cancellationToken);

            _factory = new KongrooWebApplicationFactory(
                _broker.Hostname,
                _broker.GetMappedPublicPort(5672),
                RabbitMqUsername,
                RabbitMqPassword
            );

            await WaitForHealthyAsync(cancellationToken);
        }
        finally
        {
            Gate.Release();
        }
    }

    public static void Reset() => _factory?.NotificationSender.Clear();

    public static async Task StopAsync()
    {
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
            _factory = null;
        }

        if (_broker is not null)
        {
            await _broker.DisposeAsync();
            _broker = null;
        }
    }

    private static async Task WaitForHealthyAsync(CancellationToken cancellationToken)
    {
        using var client = Factory.CreateClient();

        var deadline = DateTimeOffset.UtcNow.AddSeconds(30);
        while (true)
        {
            using var response = await client.GetAsync("/health", cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return;
            }

            if (DateTimeOffset.UtcNow >= deadline)
            {
                response.EnsureSuccessStatusCode();
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        }
    }
}
