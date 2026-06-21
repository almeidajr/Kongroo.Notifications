using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Kongroo.Notifications;
using Kongroo.Notifications.Api;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)
    )
);

builder.Services.AddSerilog(configuration =>
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithMachineName()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .Enrich.WithProperty("Application", AppDomain.CurrentDomain.FriendlyName)
);

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();

builder.Services.AddHealthChecks().AddApplicationLifecycleHealthCheck().AddResourceUtilizationHealthCheck();

builder.Services.AddNotificationsModule(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapHealthChecks("health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

app.MapOpenApi();
app.MapScalarApiReference();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
