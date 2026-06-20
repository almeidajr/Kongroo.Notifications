using Kongroo.BuildingBlocks.Application;

namespace Kongroo.BuildingBlocks.Contracts;

/// <summary>
/// Published by the Payments service when a payment is processed. Consumed to send a purchase
/// confirmation email when <see cref="Approved"/> is <c>true</c>.
/// </summary>
public sealed record PaymentProcessedIntegrationEvent(
    Guid OrderId,
    Guid UserId,
    string Email,
    string CustomerName,
    decimal Amount,
    string Currency,
    bool Approved,
    DateTimeOffset ProcessedAt
) : IntegrationEvent;
