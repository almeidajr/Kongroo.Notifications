namespace Kongroo.Notifications.Application;

/// <summary>What the function decided to do with one message, and the line it logs for it.</summary>
public sealed record NotificationOutcome(NotificationKind Kind, string LogMessage);
