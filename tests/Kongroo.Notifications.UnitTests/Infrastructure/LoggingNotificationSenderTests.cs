using Kongroo.Notifications.Domain;
using Kongroo.Notifications.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Shouldly;

namespace Kongroo.Notifications.UnitTests.Infrastructure;

public sealed class LoggingNotificationSenderTests
{
    [Fact]
    public async Task SendWelcomeAsync_WithWelcomeEmail_ShouldLogInformationContainingRecipient()
    {
        var collector = new FakeLogCollector();
        var sender = new LoggingNotificationSender(new FakeLogger<LoggingNotificationSender>(collector));

        await sender.SendWelcomeAsync(
            new WelcomeEmail("ada@example.com", "Ada Lovelace"),
            TestContext.Current.CancellationToken
        );

        var record = collector.GetSnapshot().ShouldHaveSingleItem();
        record.Level.ShouldBe(LogLevel.Information);
        record.Message.ShouldContain("ada@example.com");
        record.Message.ShouldContain("Ada Lovelace");
    }

    [Fact]
    public async Task SendPurchaseConfirmationAsync_WithPurchaseConfirmationEmail_ShouldLogInformationContainingOrderDetails()
    {
        var collector = new FakeLogCollector();
        var sender = new LoggingNotificationSender(new FakeLogger<LoggingNotificationSender>(collector));
        var orderId = Guid.CreateVersion7();

        await sender.SendPurchaseConfirmationAsync(
            new PurchaseConfirmationEmail("grace@example.com", "Grace Hopper", orderId, 59.90m, "BRL"),
            TestContext.Current.CancellationToken
        );

        var record = collector.GetSnapshot().ShouldHaveSingleItem();
        record.Level.ShouldBe(LogLevel.Information);
        record.Message.ShouldContain("grace@example.com");
        record.Message.ShouldContain(orderId.ToString());
        record.Message.ShouldContain("BRL");
    }
}
