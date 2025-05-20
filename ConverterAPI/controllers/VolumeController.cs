using System.Globalization;
using Converter.Volume;
using ConverterAPI.services;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Monitoring.Logging;
using Exception = System.Exception;

namespace ConverterAPI.controllers;

// API controller to handle volume-related operations such as conversion, addition, and subtraction.
[ApiController]
[Route("api/volumes")]
public class VolumeController : ControllerBase
{
    private readonly IVolumeConverter _converter; // Service for volume conversion operations
    private readonly SqlService _db; // Service for saving actions to the database
    private const string UnsupportedVolumeUnit = "Unsupported volume unit."; // Error message for unsupported volume units

    public VolumeController(IVolumeConverter converter, SqlService db)
    {
        _converter = converter;
        _db = db;
    }
    
    private async Task<ActionResult> HandleRequest(Func<double> func, string methodName, string? action = null, bool saveDb = true) 
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(methodName, nameof(VolumeController)));

        try
        {
            // Perform conversion and save action
            var result = func();
            string formatedResult = result.ToString("F4", CultureInfo.InvariantCulture);
            
            if (saveDb && action != null)
                await _db.SaveConversion(action, formatedResult); 
            
            // Log and return the result
            Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(methodName, $"action = {action} - result = {formatedResult}."));
            return Ok(formatedResult);
        }
        catch (Exception ex)
        {
            // Log error if any issue arises
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(methodName, ex.Message));
            return BadRequest(ex.Message);
        }
    }

    private static bool IsSupported(params EVolumeUnit[] units)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(IsSupported), nameof(VolumeController)));

        try
        {
            // Evaluate and log the result
            bool result = units.All(u => Enum.IsDefined(typeof(EVolumeUnit), u));
            Logger.Log(ELogLevel.Info, Logger.SuccessLogMessage(nameof(IsSupported), "evaluating units."));
            return result;
        }
        catch (Exception ex)
        {
            // Log error if any issue arises
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(IsSupported), ex.Message));
            return false;
        }
    }
    
    // Endpoint to convert between volume units
    [HttpGet("convert")]
    public async Task<ActionResult> Convert(double value, EVolumeUnit from, EVolumeUnit to)
    {
        // Start tracing activity
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(from, to))
            return BadRequest(UnsupportedVolumeUnit);
        
        var logAction = $"convert: {value} {from.ToString()} to {to.ToString()}";
        return await HandleRequest(
            func: () => _converter.Convert(value, from, to),
            methodName: nameof(Convert),
            action: logAction
        );
    }
    
    // Endpoint to add two volume values
    [HttpGet("add")]
    public async Task<ActionResult> Add(double a, EVolumeUnit aUnit, double b, EVolumeUnit bUnit, EVolumeUnit resultUnit)
    {
        // Start tracing activity
        using var activity = MonitorService.ActivitySource.StartActivity();
        
        if (!IsSupported(aUnit, bUnit, resultUnit))
            return BadRequest(UnsupportedVolumeUnit);

        var logAction = $"add: {a} {aUnit.ToString()} + {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.AddVolumes(a, aUnit, b, bUnit, resultUnit), 
            methodName: nameof(Add), 
            action: logAction
        );
    }

    // Endpoint to subtract two volume values
    [HttpGet("subtract")]
    public async Task<ActionResult> Subtract(double a, EVolumeUnit aUnit, double b, EVolumeUnit bUnit, EVolumeUnit resultUnit)
    {
        // Start tracing activity
        using var activity = MonitorService.ActivitySource.StartActivity();
        
        if (!IsSupported(aUnit, bUnit, resultUnit))
            return BadRequest(UnsupportedVolumeUnit);

        var logAction = $"subtract: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.SubtractVolumes(a, aUnit, b, bUnit, resultUnit), 
            methodName: nameof(Subtract), 
            action: logAction
        );
    }

    // Endpoint to scale a volume by a factor
    [HttpGet("scale")]
    public async Task<ActionResult> Scale(double value, EVolumeUnit valueUnit, double factor, EVolumeUnit resultUnit)
    {
        // Start tracing activity
        using var activity = MonitorService.ActivitySource.StartActivity();
        
        if (!IsSupported(valueUnit, resultUnit))
            return BadRequest(UnsupportedVolumeUnit);

        var logAction = $"scale: {value} {valueUnit.ToString()} * {factor} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.ScaleVolume(value, valueUnit, factor, resultUnit), 
            methodName: nameof(Scale), 
            action: logAction
        );
    }

    // Endpoint to calculate the difference between two volume values
    [HttpGet("difference")]
    public async Task<ActionResult> Difference(double a, EVolumeUnit aUnit, double b, EVolumeUnit bUnit, EVolumeUnit resultUnit)
    {
        // Start tracing activity
        using var activity = MonitorService.ActivitySource.StartActivity();
        
        if (!IsSupported(aUnit, bUnit, resultUnit))
            return BadRequest(UnsupportedVolumeUnit);

        var logAction = $"difference: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.VolumeDifference(a, aUnit, b, bUnit, resultUnit), 
            methodName: nameof(Difference), 
            action: logAction
        );
    }

    // Endpoint to calculate the percentage of one volume in relation to another
    [HttpGet("percentage")]
    public async Task<ActionResult> Percentage(double a, EVolumeUnit part, double b, EVolumeUnit whole)
    {
        // Start tracing activity
        using var activity = MonitorService.ActivitySource.StartActivity();
        
        if (!IsSupported(part, whole))
            return BadRequest(UnsupportedVolumeUnit);

        var logAction = $"percentage: {a} {part.ToString()} / {b} {whole.ToString()}";
        return await HandleRequest(
            func: () => _converter.VolumeAsPercentage(a, part, b, whole), 
            methodName: nameof(Percentage), 
            action: logAction
        );
    }
}