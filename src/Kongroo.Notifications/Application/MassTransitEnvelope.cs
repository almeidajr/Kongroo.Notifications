using System.Text.Json;

namespace Kongroo.Notifications.Application;

/// <summary>The subset of the MassTransit JSON envelope this function needs.</summary>
public static class MassTransitEnvelope
{
    private static readonly JsonSerializerOptions MessageOptions = new(JsonSerializerDefaults.Web);

    public static (string MessageType, JsonElement Message) Parse(string body)
    {
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;

        if (
            !root.TryGetProperty("messageType", out var messageTypes)
            || messageTypes.ValueKind != JsonValueKind.Array
            || messageTypes.GetArrayLength() == 0
        )
        {
            throw new JsonException("Envelope has no messageType array.");
        }

        if (!root.TryGetProperty("message", out var message) || message.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("Envelope has no message object.");
        }

        var messageType = messageTypes[0].GetString() ?? throw new JsonException("messageType[0] is null.");

        return (messageType, message.Clone());
    }

    public static T Deserialize<T>(JsonElement message) =>
        message.Deserialize<T>(MessageOptions)
        ?? throw new JsonException($"Message could not be deserialized as {typeof(T).Name}.");
}
