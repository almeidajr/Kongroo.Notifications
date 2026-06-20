using System.Net;
using Shouldly;

namespace Kongroo.Notifications.IntegrationTests.Support;

public static class TestPolling
{
    public static async Task WaitForHealthyAsync(HttpClient client, CancellationToken cancellationToken)
    {
        // The MassTransit bus connects to RabbitMQ asynchronously, so /health is briefly unhealthy after start.
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
                response.StatusCode.ShouldBe(HttpStatusCode.OK);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        }
    }

    public static async Task WaitUntilAsync(Func<bool> condition, CancellationToken cancellationToken)
    {
        var deadline = DateTimeOffset.UtcNow.AddSeconds(30);
        while (!condition())
        {
            if (DateTimeOffset.UtcNow >= deadline)
            {
                throw new TimeoutException("The expected notification was not observed within the timeout.");
            }

            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        }
    }
}
