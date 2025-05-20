using ConverterAPI.setup;
using Microsoft.Extensions.Options;
using Monitoring;
using Monitoring.Logging;
using MySql.Data.MySqlClient;

namespace ConverterAPI.services;

public class SqlService
{
    private readonly string _connectionString; 

    public SqlService(IOptions<SqlDbConfig> dbConfig)
    {
        this._connectionString = dbConfig.Value.ConnectionString;
    }

    // Method to save a converter action and its result to the database
    public async Task SaveConversion(string action, string result)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(SaveConversion), nameof(SqlService)));
        
        try
        {
            await CreateConversionLog(action, result); // Writes the data to the database
        }
        catch (Exception ex)
        {
            // Log error if any issue arises during writing to db
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(SaveConversion), ex.Message));
        }
    }

    // Method to insert action and result into the database
    private async Task CreateConversionLog(string action, string result)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(CreateConversionLog), nameof(SqlService)));
        
        await using var connection = new MySqlConnection(this._connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("INSERT INTO tblConverterLogs (action, result) VALUES (@action, @result)", connection);
        command.Parameters.AddWithValue("@action", action);
        command.Parameters.AddWithValue("@result", result);

        // Log and return the result
        await command.ExecuteNonQueryAsync(); 
        Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(nameof(CreateConversionLog), "writing action and result to db."));
    }

    // Method to retrieve converter logs 
    public async Task<List<string>> FetchConversionLogs()
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(FetchConversionLogs), nameof(SqlService)));
        
        try
        {
            var memoryLogs = await GetConversionLogs(5); // Fetch the logs from the database
            return memoryLogs;
        }
        catch (Exception ex)
        {
            // Log error if any issue arises during fetching logs
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(FetchConversionLogs), ex.Message));
            return new List<string>(); // Return empty list in case of failure
        }
    }

    // Method to fetch recent logs from the database
    private async Task<List<string>> GetConversionLogs(int amountRecord)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(GetConversionLogs), nameof(SqlService)));
        
        var log = new List<string>();

        await using var connection = new MySqlConnection(this._connectionString);
        await connection.OpenAsync();

        await using var command = new MySqlCommand("SELECT action, result FROM tblConverterLogs ORDER BY id DESC LIMIT @amountRecord", connection);
        command.Parameters.AddWithValue("@amountRecord", amountRecord);
        await using var reader = await command.ExecuteReaderAsync();

        // Read and add logs to the list
        while (await reader.ReadAsync())
            log.Add($"{reader.GetString(0)} = {reader.GetString(1)}");

        // Log and return the result
        Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(nameof(GetConversionLogs), "reading memory from db."));
        return log;
    }
}
