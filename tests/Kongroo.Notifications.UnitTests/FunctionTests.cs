using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Kongroo.Notifications.UnitTests.Support;
using NSubstitute;
using Shouldly;

namespace Kongroo.Notifications.UnitTests;

public sealed class FunctionTests
{
    [Fact]
    public void Handle_WithValidRecords_ShouldLogEachAndReportNoFailures()
    {
        var logger = new RecordingLambdaLogger();
        var context = CreateContext(logger);
        var sqsEvent = CreateEvent(("m-1", Envelopes.UserCreated), ("m-2", Envelopes.PaymentApproved));

        var response = Function.Handle(sqsEvent, context);

        response.BatchItemFailures.ShouldBeEmpty();
        logger.Lines.Count.ShouldBe(2);
        logger.Lines[0].ShouldContain("welcome email");
        logger.Lines[1].ShouldContain("purchase confirmation email");
    }

    [Fact]
    public void Handle_WithOneMalformedRecord_ShouldReportOnlyThatRecordAsFailed()
    {
        var logger = new RecordingLambdaLogger();
        var context = CreateContext(logger);
        var sqsEvent = CreateEvent(
            ("good", Envelopes.UserCreated),
            ("bad", Envelopes.NotJson),
            ("also-good", Envelopes.PaymentRejected)
        );

        var response = Function.Handle(sqsEvent, context);

        response.BatchItemFailures.ShouldHaveSingleItem().ItemIdentifier.ShouldBe("bad");
        logger.Lines.Count.ShouldBe(3);
        logger.Lines[1].ShouldContain("bad");
    }

    [Fact]
    public void Handle_WithUnknownMessageType_ShouldAcknowledgeWithoutFailure()
    {
        var logger = new RecordingLambdaLogger();
        var context = CreateContext(logger);
        var sqsEvent = CreateEvent(("m-1", Envelopes.UnknownType));

        var response = Function.Handle(sqsEvent, context);

        response.BatchItemFailures.ShouldBeEmpty();
        logger.Lines.ShouldHaveSingleItem().ShouldContain("Ignoring unknown message type");
    }

    private static ILambdaContext CreateContext(ILambdaLogger logger)
    {
        var context = Substitute.For<ILambdaContext>();
        context.Logger.Returns(logger);

        return context;
    }

    private static SQSEvent CreateEvent(params (string MessageId, string Body)[] records) =>
        new()
        {
            Records = records
                .Select(record => new SQSEvent.SQSMessage { MessageId = record.MessageId, Body = record.Body })
                .ToList(),
        };
}
