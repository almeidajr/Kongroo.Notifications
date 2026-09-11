using System.Diagnostics.CodeAnalysis;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Kongroo.Notifications.Application;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Kongroo.Notifications;

/// <summary>
/// SQS-triggered entry point. Each record is a MassTransit envelope delivered raw from SNS.
/// Records that fail for any reason are reported individually so the rest of the batch is acknowledged.
/// </summary>
[SuppressMessage(
    "Naming",
    "CA1716:Identifiers should not match keywords",
    Justification = "Name is fixed by the AWS Lambda handler string in aws-lambda-tools-defaults.json."
)]
public static class Function
{
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "Per-record isolation boundary: the failure is logged and reported to SQS as a batch item failure for retry and dead-lettering."
    )]
    public static SQSBatchResponse Handle(SQSEvent sqsEvent, ILambdaContext context)
    {
        var failures = new List<SQSBatchResponse.BatchItemFailure>();

        foreach (var record in sqsEvent.Records)
        {
            try
            {
                var outcome = NotificationHandler.Handle(record.Body);
                context.Logger.LogInformation(outcome.LogMessage);
            }
            catch (Exception exception)
            {
                context.Logger.LogError($"Message {record.MessageId} could not be processed: {exception.Message}");
                failures.Add(new SQSBatchResponse.BatchItemFailure { ItemIdentifier = record.MessageId });
            }
        }

        return new SQSBatchResponse(failures);
    }
}
