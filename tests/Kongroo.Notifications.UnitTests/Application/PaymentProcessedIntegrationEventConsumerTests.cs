using Kongroo.Notifications.Application;
using Kongroo.Notifications.Application.Abstractions;
using Kongroo.Notifications.Domain;
using Kongroo.Payments.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Kongroo.Notifications.UnitTests.Application;

public sealed class PaymentProcessedIntegrationEventConsumerTests
{
    [Fact]
    public async Task Consume_WhenApproved_ShouldSendPurchaseConfirmationMappedFromTheEvent()
    {
        var sender = Substitute.For<INotificationSender>();
        var consumer = new PaymentProcessedIntegrationEventConsumer(
            sender,
            NullLogger<PaymentProcessedIntegrationEventConsumer>.Instance
        );
        var orderId = Guid.CreateVersion7();
        var message = new PaymentProcessedIntegrationEvent(
            orderId,
            Guid.CreateVersion7(),
            "grace@example.com",
            "Grace Hopper",
            59.90m,
            "BRL",
            IsApproved: true,
            DateTimeOffset.UtcNow
        );

        var context = Substitute.For<ConsumeContext<PaymentProcessedIntegrationEvent>>();
        context.Message.Returns(message);
        context.CancellationToken.Returns(TestContext.Current.CancellationToken);

        await consumer.Consume(context);

        await sender
            .Received(1)
            .SendPurchaseConfirmationAsync(
                Arg.Is<PurchaseConfirmationEmail>(email =>
                    email.To == "grace@example.com"
                    && email.Name == "Grace Hopper"
                    && email.OrderId == orderId
                    && email.Amount == 59.90m
                    && email.Currency == "BRL"
                ),
                TestContext.Current.CancellationToken
            );
    }

    [Fact]
    public async Task Consume_WhenRejected_ShouldNotSendAnyEmail()
    {
        var sender = Substitute.For<INotificationSender>();
        var consumer = new PaymentProcessedIntegrationEventConsumer(
            sender,
            NullLogger<PaymentProcessedIntegrationEventConsumer>.Instance
        );
        var message = new PaymentProcessedIntegrationEvent(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "grace@example.com",
            "Grace Hopper",
            59.90m,
            "BRL",
            IsApproved: false,
            DateTimeOffset.UtcNow
        );

        var context = Substitute.For<ConsumeContext<PaymentProcessedIntegrationEvent>>();
        context.Message.Returns(message);
        context.CancellationToken.Returns(TestContext.Current.CancellationToken);

        await consumer.Consume(context);

        await sender
            .DidNotReceive()
            .SendPurchaseConfirmationAsync(Arg.Any<PurchaseConfirmationEmail>(), Arg.Any<CancellationToken>());
    }
}
