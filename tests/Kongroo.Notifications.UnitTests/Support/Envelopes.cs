namespace Kongroo.Notifications.UnitTests.Support;

/// <summary>MassTransit JSON envelopes as delivered raw from SNS to the SQS queue.</summary>
internal static class Envelopes
{
    public const string UserCreated = """
        {
          "messageId": "0199342a-2b3c-7d4e-8f90-0123456789ab",
          "conversationId": "0199342a-2b3c-7d4e-8f90-0123456789ac",
          "sourceAddress": "amazonsqs://us-east-1/identity-api_bus_abc",
          "destinationAddress": "amazonsqs://us-east-1/kongroo-user-created?type=topic",
          "messageType": [
            "urn:message:Kongroo.Identity.Contracts:UserCreatedIntegrationEvent",
            "urn:message:Kongroo.BuildingBlocks.Application:IntegrationEvent"
          ],
          "message": {
            "userId": "0199342a-0000-7000-8000-000000000001",
            "email": "ada@example.com",
            "name": "Ada Lovelace",
            "integrationEventId": "0199342a-0000-7000-8000-000000000002",
            "occurredAt": "2026-09-10T12:00:00+00:00"
          },
          "sentTime": "2026-09-10T12:00:00.1234567Z",
          "headers": {},
          "host": { "machineName": "identity-api", "processName": "Kongroo.Identity" }
        }
        """;

    public const string PaymentApproved = """
        {
          "messageId": "0199342a-2b3c-7d4e-8f90-0123456789bb",
          "messageType": [
            "urn:message:Kongroo.Payments.Contracts:PaymentProcessedIntegrationEvent",
            "urn:message:Kongroo.BuildingBlocks.Application:IntegrationEvent"
          ],
          "message": {
            "paymentId": "0199342a-0000-7000-8000-000000000011",
            "orderId": "0199342a-0000-7000-8000-000000000012",
            "customerId": "0199342a-0000-7000-8000-000000000013",
            "customerEmail": "grace@example.com",
            "customerName": "Grace Hopper",
            "totalAmount": 59.90,
            "currency": "BRL",
            "isApproved": true,
            "processedAt": "2026-09-10T12:05:00+00:00",
            "integrationEventId": "0199342a-0000-7000-8000-000000000014",
            "occurredAt": "2026-09-10T12:05:00+00:00"
          }
        }
        """;

    public const string PaymentRejected = """
        {
          "messageId": "0199342a-2b3c-7d4e-8f90-0123456789cc",
          "messageType": ["urn:message:Kongroo.Payments.Contracts:PaymentProcessedIntegrationEvent"],
          "message": {
            "paymentId": "0199342a-0000-7000-8000-000000000021",
            "orderId": "0199342a-0000-7000-8000-000000000022",
            "customerId": "0199342a-0000-7000-8000-000000000023",
            "customerEmail": "grace@example.com",
            "customerName": "Grace Hopper",
            "totalAmount": 5000.00,
            "currency": "BRL",
            "isApproved": false,
            "processedAt": "2026-09-10T12:06:00+00:00"
          }
        }
        """;

    public const string UnknownType = """
        {
          "messageId": "0199342a-2b3c-7d4e-8f90-0123456789dd",
          "messageType": ["urn:message:Kongroo.Identity.Contracts:UserRoleChangedIntegrationEvent"],
          "message": { "userId": "0199342a-0000-7000-8000-000000000031", "previousRole": "User", "currentRole": "Admin" }
        }
        """;

    public const string MissingMessageType = """
        { "messageId": "0199342a-2b3c-7d4e-8f90-0123456789ee", "message": { "email": "x@example.com" } }
        """;

    public const string MessageNotObject = """
        {
          "messageId": "0199342a-2b3c-7d4e-8f90-0123456789ff",
          "messageType": ["urn:message:Kongroo.Identity.Contracts:UserCreatedIntegrationEvent"],
          "message": "just a string"
        }
        """;

    public const string NullMessageType = """
        {
          "messageId": "0199342a-2b3c-7d4e-8f90-012345678900",
          "messageType": [null],
          "message": {}
        }
        """;

    public const string NotJson = "this is not json";
}
