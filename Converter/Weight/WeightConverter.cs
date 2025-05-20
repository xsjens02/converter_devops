using Monitoring;
using Monitoring.Logging;

namespace Converter.Weight;

public class WeightConverter : IWeightConverter
{
    private const string NegativeValueMessage = "Invalid, value(s) must be non-negative for conversion.";
    private const string UnsupportedUnit = "Unsupported weight unit.";

    private readonly Dictionary<EWeightUnit, double> _weightUnitsInGram = new()
    {
        { EWeightUnit.Microgram, 0.000001 },
        { EWeightUnit.Milligram, 0.001 },
        { EWeightUnit.Gram, 1.0 },
        { EWeightUnit.Kilogram, 1000.0 },
        { EWeightUnit.Ounce, 28.3495 },
        { EWeightUnit.Pound, 453.592 },
        { EWeightUnit.Stone, 6350.29 }
    };

    // Converts a weight value from one unit to another.
    public double Convert(double value, EWeightUnit from, EWeightUnit to)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(Convert), nameof(WeightConverter)));

        if (value < 0 ) 
        {
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(Convert), NegativeValueMessage));
            throw new ArgumentException(NegativeValueMessage);
        }

        if (from == to)
            return value;

        if (_weightUnitsInGram.ContainsKey(from) && _weightUnitsInGram.TryGetValue(to, out var conversionFactor))
            return ConvertToGram(value, from) / conversionFactor;

        Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(Convert), UnsupportedUnit));
        throw new ArgumentException(UnsupportedUnit);
    }

    // Adds two weight values and returns the total in the specified unit.
    public double AddWeights(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(AddWeights), nameof(WeightConverter)));

        if (firstVal < 0 || secondVal < 0)
        {
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(AddWeights), NegativeValueMessage));
            throw new ArgumentException(NegativeValueMessage);
        }

        var firstInGrams = ConvertToGram(firstVal, firstUnit);
        var secondInGrams = ConvertToGram(secondVal, secondUnit);
        var totalInGrams = firstInGrams + secondInGrams;
        return ConvertFromGram(totalInGrams, resultUnit);
    }

    // Subtracts the second weight from the first and returns the result.
    public double SubtractWeights(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(SubtractWeights), nameof(WeightConverter)));

        if (firstVal < 0 || secondVal < 0)
        {
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(SubtractWeights), NegativeValueMessage));
            throw new ArgumentException(NegativeValueMessage);
        }

        var firstInGrams = ConvertToGram(firstVal, firstUnit);
        var secondInGrams = ConvertToGram(secondVal, secondUnit);
        var differenceInGrams = Math.Abs(firstInGrams - secondInGrams);
        return ConvertFromGram(differenceInGrams, resultUnit);
    }

    // Scales a weight value by a factor and returns the result.
    public double ScaleWeight(double value, EWeightUnit unit, double factor, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(ScaleWeight), nameof(WeightConverter)));

        if (value < 0 || factor < 0)
        {
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(ScaleWeight), NegativeValueMessage));
            throw new ArgumentException(NegativeValueMessage);
        }

        var valueInGrams = ConvertToGram(value, unit);
        var scaledValueInGrams = valueInGrams * factor;
        return ConvertFromGram(scaledValueInGrams, resultUnit);
    }

    // Returns the absolute difference between two weights.
    public double WeightDifference(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(WeightDifference), nameof(WeightConverter)));

        if (firstVal < 0 || secondVal < 0)
        {
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(WeightDifference), NegativeValueMessage));
            throw new ArgumentException(NegativeValueMessage);
        }

        var firstInGrams = ConvertToGram(firstVal, firstUnit);
        var secondInGrams = ConvertToGram(secondVal, secondUnit);
        var differenceInGrams = Math.Abs(firstInGrams - secondInGrams);
        return ConvertFromGram(differenceInGrams, resultUnit);
    }

    // Returns what percentage one weight is of another.
    public double WeightAsPercentage(double value, EWeightUnit unit, double totalValue, EWeightUnit totalUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(WeightAsPercentage), nameof(WeightConverter)));

        if (value <= 0 || totalValue <= 0)
        {
            Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(WeightAsPercentage), "Invalid, value(s) must be higher than zero."));
            throw new ArgumentException("Invalid, value(s) must be higher than zero.");
        }

        var valueInGrams = ConvertToGram(value, unit);
        var totalInGrams = ConvertToGram(totalValue, totalUnit);

        return (valueInGrams / totalInGrams) * 100;
    }

    private double ConvertToGram(double value, EWeightUnit from)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(ConvertToGram), nameof(WeightConverter)));

        if (_weightUnitsInGram.TryGetValue(from, out var factor))
            return value * factor;

        Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(ConvertToGram), UnsupportedUnit));
        throw new ArgumentException(UnsupportedUnit);
    }

    private double ConvertFromGram(double valueInGram, EWeightUnit to)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, Logger.StartLogMessage(nameof(ConvertFromGram), nameof(WeightConverter)));

        if (_weightUnitsInGram.TryGetValue(to, out var factor))
            return valueInGram / factor;

        Logger.Log(ELogLevel.Error, Logger.FailLogMessage(nameof(ConvertFromGram), UnsupportedUnit));
        throw new ArgumentException(UnsupportedUnit);
    }
}