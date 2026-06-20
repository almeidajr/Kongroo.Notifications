using Kongroo.Notifications.Domain;

namespace Kongroo.Notifications.Application;

/// <summary>Delivers simulated notification emails. The only implementation logs to stdout.</summary>
public interface INotificationSender
{
    Task SendWelcomeAsync(WelcomeEmail email, CancellationToken cancellationToken);

    Task SendPurchaseConfirmationAsync(PurchaseConfirmationEmail email, CancellationToken cancellationToken);
}
