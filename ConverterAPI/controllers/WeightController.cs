using System.Globalization;
using Converter.Weight;
using ConverterAPI.services;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Monitoring.Logging;

namespace ConverterAPI.controllers;

// API controller for handling weight-related operations such as conversion, addition, and subtraction.
[ApiController]
[Route("api/weights")]
public class WeightController : ControllerBase
{
    private readonly IWeightConverter _converter; // Service for weight conversion operations
    private readonly SqlService _db; // Service for saving actions to the database
    private const string UnsupportedWeightUnit = "Unsupported weight unit."; // Error message for unsupported weight units

    public WeightController(IWeightConverter converter, SqlService db)
    {
        _converter = converter;
        _db = db;
    }
    
    // Endpoint to convert between different weight units
    [HttpGet("convert")]
    public async Task<ActionResult<double>> Convert(double value, EWeightUnit from, EWeightUnit to)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Convert] [class:WeightController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EWeightUnit), from) || !Enum.IsDefined(typeof(EWeightUnit), to))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Convert] due to unsupported unit.");
            return BadRequest(UnsupportedWeightUnit);
        }

        // Perform the conversion and save the action
        var result = _converter.Convert(value, from, to);
        string action = $"convert: {value} {from.ToString()} to {to.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Convert] with {value} {from.ToString()} to {to.ToString()} = {resultString} {to.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to add two weights together
    [HttpGet("add")]
    public async Task<ActionResult<double>> Add(double a, EWeightUnit aUnit, double b, EWeightUnit bUnit, EWeightUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Add] [class:WeightController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EWeightUnit), aUnit) || !Enum.IsDefined(typeof(EWeightUnit), bUnit) || !Enum.IsDefined(typeof(EWeightUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Add] due to unsupported unit.");
            return BadRequest(UnsupportedWeightUnit);
        }

        // Perform the addition and save the action
        var result = _converter.AddWeights(a, aUnit, b, bUnit, resultUnit);
        var action = $"add: {a} {aUnit.ToString()} + {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Add] with {a} {aUnit.ToString()} + {b} {bUnit.ToString()} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to subtract two weights
    [HttpGet("subtract")]
    public async Task<ActionResult<double>> Subtract(double a, EWeightUnit aUnit, double b, EWeightUnit bUnit, EWeightUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Subtract] [class:WeightController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EWeightUnit), aUnit) || !Enum.IsDefined(typeof(EWeightUnit), bUnit) || !Enum.IsDefined(typeof(EWeightUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Subtract] due to unsupported unit.");
            return BadRequest(UnsupportedWeightUnit);
        }

        // Perform the subtraction and save the action
        var result = _converter.SubtractWeights(a, aUnit, b, bUnit, resultUnit);
        var action = $"subtract: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Subtract] with {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to scale a weight by a factor
    [HttpGet("scale")]
    public async Task<ActionResult<double>> Scale(double value, EWeightUnit valueUnit, double factor, EWeightUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Scale] [class:WeightController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EWeightUnit), valueUnit) || !Enum.IsDefined(typeof(EWeightUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Scale] due to unsupported unit.");
            return BadRequest(UnsupportedWeightUnit);
        }

        // Perform the scaling and save the action
        var result = _converter.ScaleWeight(value, valueUnit, factor, resultUnit);
        var action = $"scale: {value} {valueUnit.ToString()} * {factor} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Scale] with {value} {valueUnit.ToString()} by factor {factor} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to calculate the difference between two weights
    [HttpGet("difference")]
    public async Task<ActionResult<double>> Difference(double a, EWeightUnit aUnit, double b, EWeightUnit bUnit, EWeightUnit resultUnit)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Difference] [class:WeightController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EWeightUnit), aUnit) || !Enum.IsDefined(typeof(EWeightUnit), bUnit) || !Enum.IsDefined(typeof(EWeightUnit), resultUnit))
        {
            Logger.Log(ELogLevel.Error, "Failed - [method:Difference] due to unsupported unit.");
            return BadRequest(UnsupportedWeightUnit);
        }

        // Perform the difference calculation and save the action
        var result = _converter.WeightDifference(a, aUnit, b, bUnit, resultUnit);
        var action = $"difference: {a} {aUnit.ToString()} - {b} {bUnit} in {resultUnit.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Difference] with {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()} = {resultString} {resultUnit.ToString()}");
        return Ok(resultString);
    }

    // Endpoint to calculate the percentage of one weight in relation to another
    [HttpGet("percentage")]
    public async Task<ActionResult<double>> Percentage(double a, EWeightUnit part, double b, EWeightUnit whole)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Percentage] [class:WeightController]");

        // Validate input units
        if (!Enum.IsDefined(typeof(EWeightUnit), part) || !Enum.IsDefined(typeof(EWeightUnit), whole))
        {
            Logger.Log(ELogLevel.Error, "Failed [method:Percentage] due to unsupported unit.");
            return BadRequest(UnsupportedWeightUnit);
        }

        // Perform the percentage calculation and save the action
        var result = _converter.WeightAsPercentage(a, part, b, whole);
        var action = $"percentage: {a} {part.ToString()} / {b} {whole.ToString()}";
        var resultString = result.ToString("F4", CultureInfo.InvariantCulture);
        await _db.SaveConversion(action, resultString);
        
        // Log and return the result
        Logger.Log(ELogLevel.Info, $"Success - [method:Percentage] with {a} {part.ToString()} of {b} {whole.ToString()} = {resultString}");
        return Ok(resultString);
    }
}
