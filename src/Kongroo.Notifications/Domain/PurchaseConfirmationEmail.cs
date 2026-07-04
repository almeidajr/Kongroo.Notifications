namespace Kongroo.Notifications.Domain;

public sealed record PurchaseConfirmationEmail(string To, string Name, Guid OrderId, decimal Amount, string Currency);
