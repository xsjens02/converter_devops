using System.Globalization;
using Converter.Weight;
using ConverterAPI.services;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using Monitoring.Logging;
using Exception = System.Exception;

namespace ConverterAPI.controllers;

// API controller to handle weight-related operations such as conversion, addition, and subtraction.
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

    private async Task<ActionResult> HandleRequest(Func<double> func, string methodName, string? action = null, bool saveDb = true)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(methodName, nameof(WeightController)));

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

    private static bool IsSupported(params EWeightUnit[] units)
    {
        // Start tracing activity and log entry of method
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(IsSupported), nameof(WeightController)));

        try
        {
            // Evaluate and log the result
            bool result = units.All(u => Enum.IsDefined(typeof(EWeightUnit), u));
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

    // Endpoint to convert between weight units
    [HttpGet("convert")]
    public async Task<ActionResult> Convert(double value, EWeightUnit from, EWeightUnit to)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(from, to))
            return BadRequest(UnsupportedWeightUnit);

        var logAction = $"convert: {value} {from.ToString()} to {to.ToString()}";
        return await HandleRequest(
            func: () => _converter.Convert(value, from, to),
            methodName: nameof(Convert),
            action: logAction
        );
    }

    // Endpoint to add two weight values
    [HttpGet("add")]
    public async Task<ActionResult> Add(double a, EWeightUnit aUnit, double b, EWeightUnit bUnit, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(aUnit, bUnit, resultUnit))
            return BadRequest(UnsupportedWeightUnit);

        var logAction = $"add: {a} {aUnit.ToString()} + {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.AddWeights(a, aUnit, b, bUnit, resultUnit),
            methodName: nameof(Add),
            action: logAction
        );
    }

    // Endpoint to subtract two weight values
    [HttpGet("subtract")]
    public async Task<ActionResult> Subtract(double a, EWeightUnit aUnit, double b, EWeightUnit bUnit, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(aUnit, bUnit, resultUnit))
            return BadRequest(UnsupportedWeightUnit);

        var logAction = $"subtract: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.SubtractWeights(a, aUnit, b, bUnit, resultUnit),
            methodName: nameof(Subtract),
            action: logAction
        );
    }

    // Endpoint to scale a weight by a factor
    [HttpGet("scale")]
    public async Task<ActionResult> Scale(double value, EWeightUnit valueUnit, double factor, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(valueUnit, resultUnit))
            return BadRequest(UnsupportedWeightUnit);

        var logAction = $"scale: {value} {valueUnit.ToString()} * {factor} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.ScaleWeight(value, valueUnit, factor, resultUnit),
            methodName: nameof(Scale), 
            action: logAction
        );
    }

    // Endpoint to calculate the difference between two weight values
    [HttpGet("difference")]
    public async Task<ActionResult> Difference(double a, EWeightUnit aUnit, double b, EWeightUnit bUnit, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(aUnit, bUnit, resultUnit))
            return BadRequest(UnsupportedWeightUnit);

        var logAction = $"difference: {a} {aUnit.ToString()} - {b} {bUnit.ToString()} in {resultUnit.ToString()}";
        return await HandleRequest(
            func: () => _converter.WeightDifference(a, aUnit, b, bUnit, resultUnit),
            methodName: nameof(Difference),
            action: logAction
        );
    }

    // Endpoint to calculate the percentage of one weight in relation to another
    [HttpGet("percentage")]
    public async Task<ActionResult> Percentage(double a, EWeightUnit part, double b, EWeightUnit whole)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();

        if (!IsSupported(part, whole))
            return BadRequest(UnsupportedWeightUnit);

        var logAction = $"percentage: {a} {part.ToString()} / {b} {whole.ToString()}";
        return await HandleRequest(
            func: () => _converter.WeightAsPercentage(a, part, b, whole),
            methodName: nameof(Percentage),
            action: logAction
        );
    }
}