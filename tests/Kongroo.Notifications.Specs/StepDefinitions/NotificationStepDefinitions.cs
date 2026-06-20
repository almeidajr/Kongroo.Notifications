using Kongroo.BuildingBlocks.Contracts;
using Kongroo.Notifications.Specs.Support;
using Reqnroll;
using Shouldly;

namespace Kongroo.Notifications.Specs.StepDefinitions;

[Binding]
public sealed class NotificationStepDefinitions
{
    [When("a user-created event is published for {string} named {string}")]
    public async Task WhenAUserCreatedEventIsPublished(string email, string name) =>
        await SpecsEnvironment.Factory.Bus.Publish(new UserCreatedIntegrationEvent(Guid.CreateVersion7(), email, name));

    [When("an approved payment event is published for {string} named {string}")]
    public async Task WhenAnApprovedPaymentEventIsPublished(string email, string name) =>
        await SpecsEnvironment.Factory.Bus.Publish(
            new PaymentProcessedIntegrationEvent(
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                email,
                name,
                59.90m,
                "BRL",
                Approved: true,
                DateTimeOffset.UtcNow
            )
        );

    [Then("a welcome email is sent to {string}")]
    public async Task ThenAWelcomeEmailIsSentTo(string email)
    {
        await WaitUntilAsync(() =>
            SpecsEnvironment.Factory.NotificationSender.WelcomeEmails.Any(welcome => welcome.To == email)
        );

        SpecsEnvironment.Factory.NotificationSender.WelcomeEmails.ShouldContain(welcome => welcome.To == email);
    }

    [Then("a purchase confirmation email is sent to {string}")]
    public async Task ThenAPurchaseConfirmationEmailIsSentTo(string email)
    {
        await WaitUntilAsync(() =>
            SpecsEnvironment.Factory.NotificationSender.ConfirmationEmails.Any(confirmation => confirmation.To == email)
        );

        SpecsEnvironment.Factory.NotificationSender.ConfirmationEmails.ShouldContain(confirmation =>
            confirmation.To == email
        );
    }

    private static async Task WaitUntilAsync(Func<bool> condition)
    {
        var deadline = DateTimeOffset.UtcNow.AddSeconds(30);
        while (!condition())
        {
            if (DateTimeOffset.UtcNow >= deadline)
            {
                throw new TimeoutException("The expected notification was not observed within the timeout.");
            }

            await Task.Delay(TimeSpan.FromMilliseconds(100));
        }
    }
}
