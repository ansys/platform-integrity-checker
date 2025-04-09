// ©2025, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace pic.ServiceDefaults;

// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
                                                     {
                                                         http.AddStandardResilienceHandler();
                                                         http.AddServiceDiscovery();
                                                     });

        return builder;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
                                         {
                                             logging.IncludeFormattedMessage = true;
                                             logging.IncludeScopes = true;
                                         });

        builder.Services.AddOpenTelemetry()
               .WithMetrics(metrics =>
                            {
                                metrics.AddAspNetCoreInstrumentation()
                                       .AddHttpClientInstrumentation()
                                       .AddRuntimeInstrumentation();
                            })
               .WithTracing(tracing =>
                            {
                                Func<HttpContext, bool> filter = builder.Configuration.GetValue<bool>("SilentProbes")
                                                                     ? context => !context.Request.Path.Equals("/health") &&
                                                                                  !context.Request.Path.Equals("/alive")
                                                                     : _ => true;


                                tracing.AddAspNetCoreInstrumentation(o => o.Filter = filter)
                                       .AddHttpClientInstrumentation();
                            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/alive", new HealthCheckOptions
                                      {
                                          Predicate = r => r.Tags.Contains("live")
                                      });

        return app;
    }
}