using System.Net;
using Kongroo.Notifications.IntegrationTests.Support;
using Shouldly;

namespace Kongroo.Notifications.IntegrationTests;

[Collection(nameof(RabbitMqCollection))]
public sealed class HealthEndpointTests(RabbitMqFixture broker)
{
    [Fact]
    public async Task GetHealth_ShouldReturnOk()
    {
        await using var factory = new NotificationsWebApplicationFactory(broker.Host, broker.Port);
        using var client = factory.CreateClient();

        await TestPolling.WaitForHealthyAsync(client, TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task GetOpenApiDocument_InDevelopment_ShouldReturnOk()
    {
        await using var factory = new NotificationsWebApplicationFactory(broker.Host, broker.Port);
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
