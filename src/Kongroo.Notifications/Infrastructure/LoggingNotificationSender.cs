using Kongroo.Notifications.Application.Abstractions;
using Kongroo.Notifications.Domain;

namespace Kongroo.Notifications.Infrastructure;

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
