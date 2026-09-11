using System.Globalization;
using Kongroo.Identity.Contracts;
using Kongroo.Notifications.Domain;
using Kongroo.Payments.Contracts;

namespace Kongroo.Notifications.Application;

/// <summary>Pure mapping from a raw SQS body to the notification the function simulates.</summary>
public static class NotificationHandler
{
    public const string UserCreatedMessageType = "urn:message:Kongroo.Identity.Contracts:UserCreatedIntegrationEvent";

    public const string PaymentProcessedMessageType =
        "urn:message:Kongroo.Payments.Contracts:PaymentProcessedIntegrationEvent";

    public static NotificationOutcome Handle(string body)
    {
        var (messageType, message) = MassTransitEnvelope.Parse(body);

        return messageType switch
        {
            UserCreatedMessageType => Welcome(MassTransitEnvelope.Deserialize<UserCreatedIntegrationEvent>(message)),
            PaymentProcessedMessageType => PurchaseConfirmation(
                MassTransitEnvelope.Deserialize<PaymentProcessedIntegrationEvent>(message)
            ),
            _ => new NotificationOutcome(NotificationKind.Ignored, $"Ignoring unknown message type '{messageType}'."),
        };
    }

    private static NotificationOutcome Welcome(UserCreatedIntegrationEvent integrationEvent)
    {
        var email = new WelcomeEmail(integrationEvent.Email, integrationEvent.Name);

        return new NotificationOutcome(
            NotificationKind.Welcome,
            $"Sending welcome email to {email.To} ({email.Name})."
        );
    }

    private static NotificationOutcome PurchaseConfirmation(PaymentProcessedIntegrationEvent integrationEvent)
    {
        if (!integrationEvent.IsApproved)
        {
            return new NotificationOutcome(
                NotificationKind.SkippedRejectedPayment,
                $"Ignoring rejected payment for order {integrationEvent.OrderId}; no confirmation email sent."
            );
        }

        var email = new PurchaseConfirmationEmail(
            integrationEvent.CustomerEmail,
            integrationEvent.CustomerName,
            integrationEvent.OrderId,
            integrationEvent.TotalAmount,
            integrationEvent.Currency
        );

        return new NotificationOutcome(
            NotificationKind.PurchaseConfirmation,
            string.Create(
                CultureInfo.InvariantCulture,
                $"Sending purchase confirmation email to {email.To} ({email.Name}) for order {email.OrderId}: {email.Amount:0.00} {email.Currency}."
            )
        );
    }
}
