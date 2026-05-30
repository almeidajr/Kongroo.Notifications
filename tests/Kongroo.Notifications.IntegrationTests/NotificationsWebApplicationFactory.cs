using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kongroo.Notifications.IntegrationTests;

public sealed class NotificationsWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration(
            (_, configurationBuilder) =>
            {
                var testConfiguration = new Dictionary<string, string?>
                {
                    ["Jwt:Issuer"] = "Kongroo.Notifications.IntegrationTests",
                    ["Jwt:Audience"] = "Kongroo.Notifications.IntegrationTests",
                    ["Jwt:SigningKey"] = "Kongroo.Notifications.IntegrationTests.SigningKey.For.Tests",
                    ["OutboxProcessing:PollingInterval"] = "00:10:00",
                    ["OutboxProcessing:BatchSize"] = "1",
                };

                configurationBuilder.AddInMemoryCollection(testConfiguration);
            }
        );

        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
    }
}
