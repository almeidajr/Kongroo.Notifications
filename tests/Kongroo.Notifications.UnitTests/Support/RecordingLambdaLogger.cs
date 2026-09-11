using Amazon.Lambda.Core;

namespace Kongroo.Notifications.UnitTests.Support;

/// <summary>Captures every line written through ILambdaLogger (the level-specific methods route here).</summary>
internal sealed class RecordingLambdaLogger : ILambdaLogger
{
    public List<string> Lines { get; } = [];

    public void Log(string message) => Lines.Add(message);

    public void LogLine(string message) => Lines.Add(message);
}
