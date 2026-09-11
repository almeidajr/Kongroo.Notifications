using System.Text.Json;
using Kongroo.Identity.Contracts;
using Kongroo.Notifications.Application;
using Kongroo.Notifications.UnitTests.Support;
using Shouldly;

namespace Kongroo.Notifications.UnitTests.Application;

public sealed class MassTransitEnvelopeTests
{
    [Fact]
    public void Parse_WithUserCreatedEnvelope_ShouldReturnFirstMessageTypeAndMessageElement()
    {
        var (messageType, message) = MassTransitEnvelope.Parse(Envelopes.UserCreated);

        messageType.ShouldBe("urn:message:Kongroo.Identity.Contracts:UserCreatedIntegrationEvent");
        message.GetProperty("email").GetString().ShouldBe("ada@example.com");
    }

    [Fact]
    public void Deserialize_WithUserCreatedMessage_ShouldMapCamelCasePropertiesToTheContract()
    {
        var (_, message) = MassTransitEnvelope.Parse(Envelopes.UserCreated);

        var @event = MassTransitEnvelope.Deserialize<UserCreatedIntegrationEvent>(message);

        @event.ShouldSatisfyAllConditions(
            () => @event.UserId.ShouldBe(Guid.Parse("0199342a-0000-7000-8000-000000000001")),
            () => @event.Email.ShouldBe("ada@example.com"),
            () => @event.Name.ShouldBe("Ada Lovelace")
        );
    }

    [Fact]
    public void Parse_WhenMessageTypeIsMissing_ShouldThrowJsonException() =>
        Should.Throw<JsonException>(() => MassTransitEnvelope.Parse(Envelopes.MissingMessageType));

    [Fact]
    public void Parse_WhenBodyIsNotJson_ShouldThrowJsonException() =>
        Should.Throw<JsonException>(() => MassTransitEnvelope.Parse(Envelopes.NotJson));
}
