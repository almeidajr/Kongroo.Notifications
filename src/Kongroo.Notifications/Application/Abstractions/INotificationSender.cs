using Kongroo.Notifications.Domain;

namespace Kongroo.Notifications.Application.Abstractions;

public interface INotificationSender
{
    Task SendWelcomeAsync(WelcomeEmail email, CancellationToken cancellationToken);
    Task SendPurchaseConfirmationAsync(PurchaseConfirmationEmail email, CancellationToken cancellationToken);
}
