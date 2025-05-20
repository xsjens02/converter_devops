using ConverterAPI.services;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Monitoring.Logging;

namespace ConverterAPI.controllers;

// API controller to handle memory-related operations such as retrieving the latest logs.
[ApiController]
[Route("api/memory")]
public class MemoryController : ControllerBase
{
    private readonly SqlService _db; // Service for interacting with the database

    public MemoryController(SqlService db)
    {
        _db = db;
    }
    
    // Endpoint to get the latest logs from the database
    [HttpGet]
    public async Task<ActionResult<List<string>>> GetMemory()
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(GetMemory), nameof(MemoryController)));

        try
        {
            var logs = await _db.FetchConversionLogs();  // Retrieve latest logs

            // Log and return the result
            Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(nameof(GetMemory), "fetching from memory."));
            return Ok(logs);
        }
        catch (Exception ex)
        {
            // Log error if any issue arises during fetching logs
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(GetMemory), ex.Message));
            return StatusCode(500, "An error occurred while retrieving logs.");
        }
    }
}