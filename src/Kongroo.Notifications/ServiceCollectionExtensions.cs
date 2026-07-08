using Kongroo.Notifications.Application;
using Kongroo.Notifications.Application.Abstractions;
using Kongroo.Notifications.Infrastructure;
using MassTransit;

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
                busRegistration.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("notifications"));
                busRegistration.AddConsumer<UserCreatedIntegrationEventConsumer>();
                busRegistration.AddConsumer<PaymentProcessedIntegrationEventConsumer>();
                busRegistration.UsingRabbitMq((context, busFactory) => busFactory.ConfigureEndpoints(context));
            });

            return services;
        }
    }
}
