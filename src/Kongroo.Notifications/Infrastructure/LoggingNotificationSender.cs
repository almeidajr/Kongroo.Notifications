using Kongroo.Notifications.Application;
using Kongroo.Notifications.Domain;
using Microsoft.Extensions.Logging;

namespace Kongroo.Notifications.Infrastructure;

/// <summary>Simulates email delivery by writing a structured log entry to stdout.</summary>
public sealed class LoggingNotificationSender(ILogger<LoggingNotificationSender> logger) : INotificationSender
{
    public async Task SendWelcomeAsync(WelcomeEmail email, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending welcome email to {Recipient} ({RecipientName}).", email.To, email.Name);

        await Task.Delay(Random.Shared.Next(500, 2000), cancellationToken);
    }

    public async Task SendPurchaseConfirmationAsync(
        PurchaseConfirmationEmail email,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Sending purchase confirmation email to {Recipient} ({RecipientName}) for order {OrderId}: {Amount} {Currency}.",
            email.To,
            email.Name,
            email.OrderId,
            email.Amount,
            email.Currency
        );

        await Task.Delay(Random.Shared.Next(500, 2000), cancellationToken);
    }
}
