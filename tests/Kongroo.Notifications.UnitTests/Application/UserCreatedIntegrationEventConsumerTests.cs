using Kongroo.BuildingBlocks.Contracts;
using Kongroo.Notifications.Application;
using Kongroo.Notifications.Domain;
using MassTransit;
using NSubstitute;

namespace Kongroo.Notifications.UnitTests.Application;

public sealed class UserCreatedIntegrationEventConsumerTests
{
    [Fact]
    public async Task Consume_ShouldSendWelcomeEmailMappedFromTheEvent()
    {
        var sender = Substitute.For<INotificationSender>();
        var consumer = new UserCreatedIntegrationEventConsumer(sender);
        var message = new UserCreatedIntegrationEvent(Guid.CreateVersion7(), "ada@example.com", "Ada Lovelace");

        var context = Substitute.For<ConsumeContext<UserCreatedIntegrationEvent>>();
        context.Message.Returns(message);
        context.CancellationToken.Returns(TestContext.Current.CancellationToken);

        await consumer.Consume(context);

        await sender
            .Received(1)
            .SendWelcomeAsync(
                Arg.Is<WelcomeEmail>(email => email.To == "ada@example.com" && email.Name == "Ada Lovelace"),
                TestContext.Current.CancellationToken
            );
    }
}
