using Kongroo.BuildingBlocks.Contracts;
using Kongroo.Notifications.Domain;
using MassTransit;

namespace Kongroo.Notifications.Application;

/// <summary>Sends a welcome email when a user is created.</summary>
public sealed class UserCreatedIntegrationEventConsumer(INotificationSender notificationSender)
    : IConsumer<UserCreatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        return notificationSender.SendWelcomeAsync(
            new WelcomeEmail(message.Email, message.Name),
            context.CancellationToken
        );
    }
}
