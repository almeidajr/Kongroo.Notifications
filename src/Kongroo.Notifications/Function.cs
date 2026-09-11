using System.Diagnostics.CodeAnalysis;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Kongroo.Notifications;

/// <summary>Placeholder Lambda entry point; filled in by Task 4.</summary>
[SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "Name is fixed by the AWS Lambda handler string in aws-lambda-tools-defaults.json."
)]
public static class Function
{
    public static SQSBatchResponse Handle(SQSEvent sqsEvent, ILambdaContext context) => new([]);
}
