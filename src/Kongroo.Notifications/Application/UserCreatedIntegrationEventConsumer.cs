using Kongroo.Identity.Contracts;
using Kongroo.Notifications.Application.Abstractions;
using Kongroo.Notifications.Domain;
using MassTransit;

namespace Kongroo.Notifications.Application;

public sealed class UserCreatedIntegrationEventConsumer(INotificationSender notificationSender)
    : IConsumer<UserCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        await notificationSender.SendWelcomeAsync(
            new WelcomeEmail(message.Email, message.Name),
            context.CancellationToken
        );
    }
}
