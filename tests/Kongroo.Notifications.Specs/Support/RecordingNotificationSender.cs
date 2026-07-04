using Kongroo.Notifications.Application.Abstractions;
using Kongroo.Notifications.Domain;

namespace Kongroo.Notifications.Specs.Support;

public sealed class RecordingNotificationSender : INotificationSender
{
    private readonly Lock _gate = new();
    private readonly List<WelcomeEmail> _welcomeEmails = [];
    private readonly List<PurchaseConfirmationEmail> _confirmationEmails = [];

    public IReadOnlyList<WelcomeEmail> WelcomeEmails
    {
        get
        {
            lock (_gate)
            {
                return _welcomeEmails.ToArray();
            }
        }
    }

    public IReadOnlyList<PurchaseConfirmationEmail> ConfirmationEmails
    {
        get
        {
            lock (_gate)
            {
                return _confirmationEmails.ToArray();
            }
        }
    }

    public void Clear()
    {
        lock (_gate)
        {
            _welcomeEmails.Clear();
            _confirmationEmails.Clear();
        }
    }

    public Task SendWelcomeAsync(WelcomeEmail email, CancellationToken cancellationToken)
    {
        lock (_gate)
        {
            _welcomeEmails.Add(email);
        }

        return Task.CompletedTask;
    }

    public Task SendPurchaseConfirmationAsync(PurchaseConfirmationEmail email, CancellationToken cancellationToken)
    {
        lock (_gate)
        {
            _confirmationEmails.Add(email);
        }

        return Task.CompletedTask;
    }
}
