using System.Globalization;
using Kongroo.Notifications.Application;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kongroo.Notifications.Specs.Support;

public sealed class KongrooWebApplicationFactory(
    string rabbitMqHost,
    int rabbitMqPort,
    string rabbitMqUser,
    string rabbitMqPass
) : WebApplicationFactory<Program>
{
    public RecordingNotificationSender NotificationSender { get; } = new();

    public IBus Bus => Services.GetRequiredService<IBus>();

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
                        ["RabbitMq:User"] = rabbitMqUser,
                        ["RabbitMq:Pass"] = rabbitMqPass,
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
