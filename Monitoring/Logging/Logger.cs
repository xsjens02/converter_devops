namespace Monitoring.Logging;

public abstract class Logger
{
    public static string StartLogMessage(string methodName, string className) => $"Starting - [method:{methodName}] [class:{className}]";
    public static string FailLogMessage(string methodName, string errorMessage) => $"Failed - [method:{methodName}] with error-message: {errorMessage}";
    public static string SuccessLogMessage(string methodName, string action) => $"Success - [method:{methodName}] with action: {action}";
    public static void Log(ELogLevel logLevel, string msg)
    {
        Action<string> logAction = logLevel switch
        {
            ELogLevel.Debug => MonitorService.Log.Debug,
            ELogLevel.Info => MonitorService.Log.Information,
            ELogLevel.Warn => MonitorService.Log.Warning,
            ELogLevel.Error => MonitorService.Log.Error,
            _ => MonitorService.Log.Information  
        };

        logAction(msg);
    }
}