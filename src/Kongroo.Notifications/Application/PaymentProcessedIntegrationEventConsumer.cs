using Kongroo.BuildingBlocks.Contracts;
using Kongroo.Notifications.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Kongroo.Notifications.Application;

/// <summary>Sends a purchase-confirmation email when a payment is approved; ignores rejections.</summary>
public sealed class PaymentProcessedIntegrationEventConsumer(
    INotificationSender notificationSender,
    ILogger<PaymentProcessedIntegrationEventConsumer> logger
) : IConsumer<PaymentProcessedIntegrationEvent>
{
    public Task Consume(ConsumeContext<PaymentProcessedIntegrationEvent> context)
    {
        var message = context.Message;

        if (!message.Approved)
        {
            logger.LogDebug(
                "Ignoring rejected payment for order {OrderId}; no confirmation email sent.",
                message.OrderId
            );

            return Task.CompletedTask;
        }

        return notificationSender.SendPurchaseConfirmationAsync(
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
