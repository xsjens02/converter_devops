using FeatureHubSDK;
using Monitoring;
using Monitoring.Logging;

namespace FeatureToggle;


public class FeatureToggleService : IFeatureToggleService
{
    // Holds the context used to access feature flags
    private readonly IClientContext? _context;

    // Static constructor for setting up logging for FeatureHub SDK
    static FeatureToggleService()
    {
        FeatureLogging.DebugLogger += (_, s) => Console.WriteLine("DEBUG: " + s);
        FeatureLogging.TraceLogger += (_, s) => Console.WriteLine("TRACE: " + s);
        FeatureLogging.InfoLogger += (_, s) => Console.WriteLine("INFO: " + s);
        FeatureLogging.ErrorLogger += (_, s) => Console.WriteLine("ERROR: " + s);
    }

    // Constructor that initializes the connection to FeatureHub
    public FeatureToggleService()
    {
        try
        {
            // Read FeatureHub URL and API key from environment variables
            var url = Environment.GetEnvironmentVariable("FEATUREHUB_URL");
            var apiKey = Environment.GetEnvironmentVariable("FEATUREHUB_API_KEY");
            
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(apiKey))
            {
                Logger.Log(ELogLevel.Warn, "FeatureHub URL or API key is missing from environment variables.");
                _context = null;
                return;
            }

            // Configure the FeatureHub connection
            var config = new EdgeFeatureHubConfig(url, apiKey);
            _context = config.NewContext().Build().Result;
        }
        catch (Exception ex)
        {
            // Log warning if initialization fails and fallback to null context
            Logger.Log(ELogLevel.Warn, $"Failed to instantiate FeatureToggleService with error-message: {ex.Message}");
            _context = null;
        }
    }

    // Public method to check if a feature flag is enabled
    public Task<bool> IsFeatureEnabled(string flagName)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(IsFeatureEnabled), nameof(FeatureToggleService)));
        
        try
        {
            // Return false if context is not initialized
            if (_context == null)
            {
                return Task.FromResult(false);
            }

            // Check if the given feature flag is enabled
            var feature = _context[flagName];
            Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(nameof(IsFeatureEnabled), "reading feature flag."));
            return Task.FromResult(feature.IsEnabled);
        }
        catch (Exception ex)
        {
            // Log warning if evaluation of feature flag fails
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(IsFeatureEnabled), ex.Message));
            return Task.FromResult(false);
        }
    }
}