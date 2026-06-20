using Kongroo.BuildingBlocks.Application;

namespace Kongroo.BuildingBlocks.Contracts;

/// <summary>
/// Published by the Identity service when a user is created. Consumed to send a welcome email.
/// </summary>
public sealed record UserCreatedIntegrationEvent(Guid UserId, string Email, string Name) : IntegrationEvent;
