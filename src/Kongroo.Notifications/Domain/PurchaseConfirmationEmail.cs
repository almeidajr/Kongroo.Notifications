namespace Kongroo.Notifications.Domain;

/// <summary>A purchase-confirmation email to be "sent" after an approved payment.</summary>
public sealed record PurchaseConfirmationEmail(string To, string Name, Guid OrderId, decimal Amount, string Currency);
