using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Observability;

/// <summary>
/// OpenTelemetry Observability Configuration
/// </summary>
public interface IObservabilityOptions
{
    ActivitySource CurrentActivitySource { get; }

    string ActivitySourceName => ServiceName;
    string ServiceName { get; }
    string ServiceVersion { get; } 
    
    Uri CollectorEndpoint { get; }
    
    bool IsTracingEnabled { get; }
    bool IsMetricsEnabled { get; }
    bool IsLoggingEnabled { get; }
    bool IsObservabilityEnabled { get; }
    
    bool IsWebApp { get; }
    bool IsConsoleApp { get; }
}

public class ObservabilityOptions : IObservabilityOptions
{
    const string SERVICE_NAME = "SERVICE_NAME";
    const string OPEN_TELEMETRY_COLLECTOR_URL = "open_telemetry_collector_url";
    const string OPEN_TELEMETRY_TRACING_ENABLED = "open_telemetry_tracing_enabled";
    const string OPEN_TELEMETRY_METRICS_ENABLED = "open_telemetry_metrics_enabled";
    const string OPEN_TELEMETRY_LOGGING_ENABLED = "open_telemetry_logging_enabled";

    
    public ObservabilityOptions(
            IConfiguration configuration
        )
    {
        ServiceName = configuration.GetValue<string>(SERVICE_NAME)!;
        
        ServiceVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        CurrentActivitySource = new ActivitySource(ServiceName, ServiceVersion);
        
        IsTracingEnabled = configuration.GetValue<bool>(OPEN_TELEMETRY_TRACING_ENABLED);
        IsMetricsEnabled = configuration.GetValue<bool>(OPEN_TELEMETRY_METRICS_ENABLED);
        IsLoggingEnabled = configuration.GetValue<bool>(OPEN_TELEMETRY_LOGGING_ENABLED);
        CollectorEndpoint = new Uri(configuration.GetValue<string>(OPEN_TELEMETRY_COLLECTOR_URL)!);
    }
    
    public ActivitySource CurrentActivitySource { get;}
    public string ServiceName { get; }
    public string ServiceVersion { get; }
    public Uri CollectorEndpoint { get; }
    public bool IsTracingEnabled { get; }
    public bool IsMetricsEnabled { get; }
    public bool IsLoggingEnabled { get; }
    public bool IsWebApp { get; }
    public bool IsConsoleApp { get; }
    
    public bool IsObservabilityEnabled => IsTracingEnabled || IsMetricsEnabled || IsLoggingEnabled;
}