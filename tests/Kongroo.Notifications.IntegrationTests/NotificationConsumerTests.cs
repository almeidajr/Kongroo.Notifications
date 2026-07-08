using Kongroo.Identity.Contracts;
using Kongroo.Notifications.IntegrationTests.Support;
using Kongroo.Payments.Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Kongroo.Notifications.IntegrationTests;

[Collection(nameof(RabbitMqCollection))]
public sealed class NotificationConsumerTests(RabbitMqFixture broker)
{
    [Fact]
    public async Task Consume_WithUserCreatedIntegrationEvent_ShouldProduceWelcomeEmail()
    {
        await using var factory = new NotificationsWebApplicationFactory(broker.Host, broker.Port);
        using var client = factory.CreateClient();
        await TestPolling.WaitForHealthyAsync(client, TestContext.Current.CancellationToken);

        var bus = factory.Services.GetRequiredService<IBus>();
        await bus.Publish(
            new UserCreatedIntegrationEvent(Guid.CreateVersion7(), "ada@example.com", "Ada Lovelace"),
            TestContext.Current.CancellationToken
        );

        await TestPolling.WaitUntilAsync(
            () => factory.NotificationSender.WelcomeEmails.Count == 1,
            TestContext.Current.CancellationToken
        );

        var email = factory.NotificationSender.WelcomeEmails.Single();
        email.To.ShouldBe("ada@example.com");
        email.Name.ShouldBe("Ada Lovelace");
    }

    [Fact]
    public async Task Consume_WithApprovedPaymentProcessedIntegrationEvent_ShouldProduceConfirmationEmail()
    {
        await using var factory = new NotificationsWebApplicationFactory(broker.Host, broker.Port);
        using var client = factory.CreateClient();
        await TestPolling.WaitForHealthyAsync(client, TestContext.Current.CancellationToken);

        var orderId = Guid.CreateVersion7();
        var bus = factory.Services.GetRequiredService<IBus>();
        await bus.Publish(
            new PaymentProcessedIntegrationEvent(
                Guid.CreateVersion7(),
                orderId,
                Guid.CreateVersion7(),
                "grace@example.com",
                "Grace Hopper",
                59.90m,
                "BRL",
                IsApproved: true,
                DateTimeOffset.UtcNow
            ),
            TestContext.Current.CancellationToken
        );

        await TestPolling.WaitUntilAsync(
            () => factory.NotificationSender.ConfirmationEmails.Count == 1,
            TestContext.Current.CancellationToken
        );

        var email = factory.NotificationSender.ConfirmationEmails.Single();
        email.To.ShouldBe("grace@example.com");
        email.OrderId.ShouldBe(orderId);
        email.Currency.ShouldBe("BRL");
    }
}
