namespace Kongroo.Notifications.Domain;

/// <summary>A welcome email to be "sent" to a newly created user.</summary>
public sealed record WelcomeEmail(string To, string Name);
