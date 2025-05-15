namespace Monitoring.Logging;

public abstract class Logger
{
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