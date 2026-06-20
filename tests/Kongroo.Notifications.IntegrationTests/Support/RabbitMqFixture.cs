using Testcontainers.RabbitMq;

namespace Kongroo.Notifications.IntegrationTests.Support;

public sealed class RabbitMqFixture : IAsyncLifetime
{
    public const string Username = "kongroo";
    public const string Password = "development";

    private readonly RabbitMqContainer _container = new RabbitMqBuilder("rabbitmq:4-management")
        .WithUsername(Username)
        .WithPassword(Password)
        .Build();

    public string Host => _container.Hostname;

    public int Port => _container.GetMappedPublicPort(5672);

    public async ValueTask InitializeAsync() => await _container.StartAsync();

    public async ValueTask DisposeAsync() => await _container.DisposeAsync();
}

[CollectionDefinition(nameof(RabbitMqCollection))]
public sealed class RabbitMqCollection : ICollectionFixture<RabbitMqFixture>;
