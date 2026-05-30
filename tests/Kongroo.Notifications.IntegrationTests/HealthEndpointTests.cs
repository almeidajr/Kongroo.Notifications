using System.Net;
using Shouldly;

namespace Kongroo.Notifications.IntegrationTests;

public sealed class HealthEndpointTests
{
    [Fact]
    public async Task GetHealth_ShouldReturnOk()
    {
        await using var factory = new NotificationsWebApplicationFactory();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/health", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOpenApiDocument_InDevelopment_ShouldReturnOk()
    {
        await using var factory = new NotificationsWebApplicationFactory();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
