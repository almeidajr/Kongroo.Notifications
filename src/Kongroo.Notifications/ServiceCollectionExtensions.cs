using Kongroo.Notifications.Application;
using Kongroo.Notifications.Infrastructure;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kongroo.Notifications;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddNotificationsModule(IConfiguration configuration)
        {
            services.AddScoped<INotificationSender, LoggingNotificationSender>();

            services
                .AddOptions<RabbitMqTransportOptions>()
                .Bind(configuration.GetRequiredSection("RabbitMq"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddMassTransit(busRegistration =>
            {
                busRegistration.SetKebabCaseEndpointNameFormatter();
                busRegistration.AddConsumer<UserCreatedIntegrationEventConsumer>();
                busRegistration.AddConsumer<PaymentProcessedIntegrationEventConsumer>();
                busRegistration.UsingRabbitMq((context, busFactory) => busFactory.ConfigureEndpoints(context));
            });

            return services;
        }
    }
}
