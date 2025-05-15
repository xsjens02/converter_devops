using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace Monitoring;

public abstract class MonitorService
{
    // The name of the service is retrieved from the calling assembly, or set to "Unknown" as fallback
    private static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
    
    // TracerProvider is used to configure and manage OpenTelemetry tracing
    public static readonly TracerProvider TracerProvider;
    
    // ActivitySource is used to create tracing activities (spans) in the application
    public static readonly ActivitySource ActivitySource = new ActivitySource(ServiceName);
    
    // Provides access to the configured Serilog logger instance
    public static ILogger Log => Serilog.Log.Logger;
    
    // Static constructor that sets up tracing and logging configuration
    static MonitorService()
    {
        // Configure OpenTelemetry tracing
        var tracerBuilder = Sdk.CreateTracerProviderBuilder()
            .AddConsoleExporter() // Export traces to console
            .AddSource(ActivitySource.Name) // Register activity source for tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName)) // Add metadata
            .SetSampler(new AlwaysOnSampler());

        var zipkinUrl = Environment.GetEnvironmentVariable("ZIPKIN_URL");
        if (!string.IsNullOrWhiteSpace(zipkinUrl))
        {
            tracerBuilder = tracerBuilder.AddZipkinExporter(options =>
            {
                // Export traces to Zipkin endpoint
                options.Endpoint = new Uri(zipkinUrl);
            });
        }
        TracerProvider = tracerBuilder.Build();
        
        // Configure Serilog logging
        var logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL");
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Is(IsValidLogLevel(logLevel!)) // Log level
            .WriteTo.Console() // Output logs to the console
            .Enrich.WithSpan(); // Enrich logs with span context (trace ID, span ID, etc.)

        var seqUrl = Environment.GetEnvironmentVariable("SEQ_URL");
        if (!string.IsNullOrWhiteSpace(seqUrl))
        {
            // Send logs to Seq server
            loggerConfig = loggerConfig.WriteTo.Seq(seqUrl);
        }

        Serilog.Log.Logger = loggerConfig.CreateLogger();
    }
    
    // Helper method to parse the log level and return the corresponding Serilog level
    private static LogEventLevel IsValidLogLevel(string logLevel)
    {
        return logLevel switch
        {
            "VERBOSE" => LogEventLevel.Verbose,
            "DEBUG" => LogEventLevel.Debug,
            "INFORMATION" => LogEventLevel.Information,
            "WARNING" => LogEventLevel.Warning,
            "ERROR" => LogEventLevel.Error,
            "FATAL" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information // Default level
        };
    }
}