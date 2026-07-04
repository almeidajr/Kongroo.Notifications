using Kongroo.Notifications.Application.Abstractions;
using Kongroo.Notifications.Domain;
using Kongroo.Payments.Contracts;
using MassTransit;

namespace Kongroo.Notifications.Application;

public sealed class PaymentProcessedIntegrationEventConsumer(
    INotificationSender notificationSender,
    ILogger<PaymentProcessedIntegrationEventConsumer> logger
) : IConsumer<PaymentProcessedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<PaymentProcessedIntegrationEvent> context)
    {
        var message = context.Message;
        if (!message.IsApproved)
        {
            logger.LogDebug(
                "Ignoring rejected payment for order {OrderId}; no confirmation email sent.",
                message.OrderId
            );
            return;
        }

        await notificationSender.SendPurchaseConfirmationAsync(
            new PurchaseConfirmationEmail(
                message.Email,
                message.CustomerName,
                message.OrderId,
                message.Amount,
                message.Currency
            ),
            context.CancellationToken
        );
    }
}
