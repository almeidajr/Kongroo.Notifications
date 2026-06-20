using System.Globalization;
using Kongroo.Notifications.Application;
using Kongroo.Notifications.IntegrationTests.Support;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Kongroo.Notifications.IntegrationTests;

public sealed class NotificationsWebApplicationFactory(string rabbitMqHost, int rabbitMqPort)
    : WebApplicationFactory<Program>
{
    public RecordingNotificationSender NotificationSender { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration(
            (_, configurationBuilder) =>
                configurationBuilder.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["RabbitMq:Host"] = rabbitMqHost,
                        ["RabbitMq:Port"] = rabbitMqPort.ToString(CultureInfo.InvariantCulture),
                        ["RabbitMq:User"] = RabbitMqFixture.Username,
                        ["RabbitMq:Pass"] = RabbitMqFixture.Password,
                    }
                )
        );

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<INotificationSender>();
            services.AddSingleton<INotificationSender>(NotificationSender);
        });

        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());
    }
}
