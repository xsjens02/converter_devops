using System.Globalization;
using Converter.Volume;
using ConverterAPI.services;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Monitoring.Logging;

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
    
    // Endpoint to convert between volume units
    [HttpGet("convert")]
    public async Task<ActionResult<double>> Convert(double value, EVolumeUnit from, EVolumeUnit to)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Convert] [class:VolumeController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EVolumeUnit), from) || !Enum.IsDefined(typeof(EVolumeUnit), to))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Convert] due to unsupported unit.");
            return BadRequest(UnsupportedVolumeUnit);
        }

        // Perform conversion and save action
        var result = _converter.Convert(value, from, to);
        string action = $"convert: {value} {from.ToString()} to {to.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Convert] with {value} {from.ToString()} to {to.ToString()} = {resultString} {to.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to add two volume values
    [HttpGet("add")]
    public async Task<ActionResult<double>> Add(double a, EVolumeUnit aUnit, double b, EVolumeUnit bUnit, EVolumeUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Add] [class:VolumeController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EVolumeUnit), aUnit) || !Enum.IsDefined(typeof(EVolumeUnit), bUnit) || !Enum.IsDefined(typeof(EVolumeUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Add] due to unsupported unit.");
            return BadRequest(UnsupportedVolumeUnit);
        }

        // Perform addition and save action
        var result = _converter.AddVolumes(a, aUnit, b, bUnit, resultUnit);
        var action = $"add: {a} {aUnit.ToString()} + {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Add] with {a} {aUnit.ToString()} + {b} {bUnit.ToString()} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to subtract two volume values
    [HttpGet("subtract")]
    public async Task<ActionResult<double>> Subtract(double a, EVolumeUnit aUnit, double b, EVolumeUnit bUnit, EVolumeUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Subtract] [class:VolumeController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EVolumeUnit), aUnit) || !Enum.IsDefined(typeof(EVolumeUnit), bUnit) || !Enum.IsDefined(typeof(EVolumeUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Subtract] due to unsupported unit.");
            return BadRequest(UnsupportedVolumeUnit);
        }

        // Perform subtraction and save action
        var result = _converter.SubtractVolumes(a, aUnit, b, bUnit, resultUnit);
        var action = $"subtract: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Subtract] with {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to scale a volume by a factor
    [HttpGet("scale")]
    public async Task<ActionResult<double>> Scale(double value, EVolumeUnit valueUnit, double factor, EVolumeUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Scale] [class:VolumeController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EVolumeUnit), valueUnit) || !Enum.IsDefined(typeof(EVolumeUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Scale] due to unsupported unit.");
            return BadRequest(UnsupportedVolumeUnit);
        }

        // Perform scaling and save action
        var result = _converter.ScaleVolume(value, valueUnit, factor, resultUnit);
        var action = $"scale: {value} {valueUnit.ToString()} * {factor} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Scale] with {value} {valueUnit.ToString()} by factor {factor} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to calculate the difference between two volume values
    [HttpGet("difference")]
    public async Task<ActionResult<double>> Difference(double a, EVolumeUnit aUnit, double b, EVolumeUnit bUnit, EVolumeUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Difference] [class:VolumeController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EVolumeUnit), aUnit) || !Enum.IsDefined(typeof(EVolumeUnit), bUnit) || !Enum.IsDefined(typeof(EVolumeUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Difference] due to unsupported unit.");
            return BadRequest(UnsupportedVolumeUnit);
        }

        // Perform the difference calculation and save action
        var result = _converter.VolumeDifference(a, aUnit, b, bUnit, resultUnit);
        var action = $"difference: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Difference] with {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to calculate the percentage of one volume in relation to another
    [HttpGet("percentage")]
    public async Task<ActionResult<double>> Percentage(double a, EVolumeUnit part, double b, EVolumeUnit whole)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Percentage] [class:VolumeController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EVolumeUnit), part) || !Enum.IsDefined(typeof(EVolumeUnit), whole))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Percentage] due to unsupported unit.");
            return BadRequest(UnsupportedVolumeUnit);
        }

        // Perform the percentage calculation and save action
        var result = _converter.VolumeAsPercentage(a, part, b, whole);
        var action = $"percentage: {a} {part.ToString()} / {b} {whole.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Percentage] with {a} {part.ToString()} of {b} {whole.ToString()} = {resultString}");
        return Ok(resultString);
    }
}
