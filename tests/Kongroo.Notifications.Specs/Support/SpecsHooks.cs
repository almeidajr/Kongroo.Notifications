using Reqnroll;

namespace Kongroo.Notifications.Specs.Support;

[Binding]
public sealed class SpecsHooks(ApiScenarioContext apiScenarioContext)
{
    [BeforeTestRun]
    public static async Task BeforeTestRunAsync() => await SpecsEnvironment.StartAsync();

    [BeforeScenario("@webapi")]
    public void BeforeScenario() => SpecsEnvironment.Reset();

    [AfterScenario("@webapi")]
    public void AfterScenario() => apiScenarioContext.Dispose();

    [AfterTestRun]
    public static async Task AfterTestRunAsync() => await SpecsEnvironment.StopAsync();
}
