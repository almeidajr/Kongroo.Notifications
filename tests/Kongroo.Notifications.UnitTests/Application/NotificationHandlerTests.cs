using System.Text.Json;
using Kongroo.Notifications.Application;
using Kongroo.Notifications.UnitTests.Support;
using Shouldly;

namespace Kongroo.Notifications.UnitTests.Application;

public sealed class NotificationHandlerTests
{
    [Fact]
    public void Handle_WithUserCreated_ShouldProduceWelcomeEmailLog()
    {
        var outcome = NotificationHandler.Handle(Envelopes.UserCreated);

        outcome.Kind.ShouldBe(NotificationKind.Welcome);
        outcome.LogMessage.ShouldContain("ada@example.com");
        outcome.LogMessage.ShouldContain("Ada Lovelace");
    }

    [Fact]
    public void Handle_WithApprovedPayment_ShouldProducePurchaseConfirmationLog()
    {
        var outcome = NotificationHandler.Handle(Envelopes.PaymentApproved);

        outcome.Kind.ShouldBe(NotificationKind.PurchaseConfirmation);
        outcome.LogMessage.ShouldContain("grace@example.com");
        outcome.LogMessage.ShouldContain("0199342a-0000-7000-8000-000000000012");
        outcome.LogMessage.ShouldContain("59.90 BRL");
    }

    [Fact]
    public void Handle_WithRejectedPayment_ShouldSkipWithoutEmail()
    {
        var outcome = NotificationHandler.Handle(Envelopes.PaymentRejected);

        outcome.Kind.ShouldBe(NotificationKind.SkippedRejectedPayment);
        outcome.LogMessage.ShouldContain("0199342a-0000-7000-8000-000000000022");
    }

    [Fact]
    public void Handle_WithUnknownMessageType_ShouldIgnore()
    {
        var outcome = NotificationHandler.Handle(Envelopes.UnknownType);

        outcome.Kind.ShouldBe(NotificationKind.Ignored);
        outcome.LogMessage.ShouldContain("UserRoleChangedIntegrationEvent");
    }

    [Fact]
    public void Handle_WithMalformedBody_ShouldThrowJsonException() =>
        Should.Throw<JsonException>(() => NotificationHandler.Handle(Envelopes.NotJson));
}
