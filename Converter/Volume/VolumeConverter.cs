using Monitoring;
using Monitoring.Logging;

namespace Converter.Volume;

public class VolumeConverter : IVolumeConverter
{
    private readonly Dictionary<EVolumeUnit, double> _volumeUnitsInLiter = new()
    {
        { EVolumeUnit.Milliliter, 0.001 },
        { EVolumeUnit.Centiliter, 0.01 },
        { EVolumeUnit.Deciliter, 0.1 },
        { EVolumeUnit.Liter, 1.0 },
        { EVolumeUnit.CubicMeter, 1000.0 },
        { EVolumeUnit.Cup, 0.24 },
        { EVolumeUnit.Quart, 0.946353 },
        { EVolumeUnit.Gallon, 3.78541 }
    };

    // Converts a volume value from one unit to another.
    public double Convert(double value, EVolumeUnit from, EVolumeUnit to)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:Convert] [class:VolumeConverter]");
        
        if (value <= 0) return 0;

        if (from == to)
            return value;

        if (_volumeUnitsInLiter.ContainsKey(from) && _volumeUnitsInLiter.TryGetValue(to, out var conversionFactor))
            return ConvertToLiter(value, from) / conversionFactor;

        Logger.Log(ELogLevel.Error, "Failed - [method:Convert] due to unsupported volume unit.");
        throw new ArgumentException("Unsupported volume unit.");
    }

    // Adds two volume values and returns the total in the specified unit.
    public double AddVolumes(double firstVal, EVolumeUnit firstUnit, double secondVal, EVolumeUnit secondUnit, EVolumeUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:AddVolumes] [class:VolumeConverter]");
        
        if (firstVal < 0 || secondVal < 0)
            return 0;

        var firstInLiters = ConvertToLiter(firstVal, firstUnit);
        var secondInLiters = ConvertToLiter(secondVal, secondUnit);
        var totalInLiters = firstInLiters + secondInLiters;
        return ConvertFromLiter(totalInLiters, resultUnit);
    }

    // Subtracts the second volume from the first and returns the result.
    public double SubtractVolumes(double firstVal, EVolumeUnit firstUnit, double secondVal, EVolumeUnit secondUnit, EVolumeUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:SubtractVolumes] [class:VolumeConverter]");
        
        if (firstVal < 0 || secondVal < 0)
            return 0;

        var firstInLiters = ConvertToLiter(firstVal, firstUnit);
        var secondInLiters = ConvertToLiter(secondVal, secondUnit);
        var resultInLiters = firstInLiters - secondInLiters;
        return ConvertFromLiter(resultInLiters, resultUnit);
    }

    // Scales a volume value by a factor and returns the result.
    public double ScaleVolume(double value, EVolumeUnit unit, double factor, EVolumeUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:ScaleVolume] [class:VolumeConverter]");
        
        if (value <= 0 || factor < 0)
            return 0;

        var valueInLiters = ConvertToLiter(value, unit);
        var scaledValueInLiters = valueInLiters * factor;
        return ConvertFromLiter(scaledValueInLiters, resultUnit);
    }

    // Returns the absolute difference between two volumes.
    public double VolumeDifference(double firstVal, EVolumeUnit firstUnit, double secondVal, EVolumeUnit secondUnit, EVolumeUnit resultUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:VolumeDifference] [class:VolumeConverter]");
        
        if (firstVal < 0 || secondVal < 0)
            return 0;

        var firstInLiters = ConvertToLiter(firstVal, firstUnit);
        var secondInLiters = ConvertToLiter(secondVal, secondUnit);
        var differenceInLiters = Math.Abs(firstInLiters - secondInLiters);
        return ConvertFromLiter(differenceInLiters, resultUnit);
    }

    // Returns what percentage one volume is of another.
    public double VolumeAsPercentage(double value, EVolumeUnit unit, double totalValue, EVolumeUnit totalUnit)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:VolumeAsPercentage] [class:VolumeConverter]");
        
        if (value <= 0 || totalValue <= 0)
            return 0;

        var valueInLiters = ConvertToLiter(value, unit);
        var totalInLiters = ConvertToLiter(totalValue, totalUnit);
        return (valueInLiters / totalInLiters) * 100;
    }

    private double ConvertToLiter(double value, EVolumeUnit from)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:ConvertToLiter] [class:VolumeConverter]");
        
        if (_volumeUnitsInLiter.TryGetValue(from, out var factor))
            return value * factor;

        Logger.Log(ELogLevel.Error, "Failed - [method:ConvertToLiter] due to unsupported volume unit.");
        throw new ArgumentException("Unsupported volume unit.");
    }

    private double ConvertFromLiter(double valueInLiter, EVolumeUnit to)
    {
        using var activity = MonitorService.ActivitySource.StartActivity();
        Logger.Log(ELogLevel.Debug, "Starting - [method:ConvertFromLiter] [class:VolumeConverter]");
        
        if (_volumeUnitsInLiter.TryGetValue(to, out var factor))
            return valueInLiter / factor;

        Logger.Log(ELogLevel.Error, "Failed - [method:ConvertFromLiter] due to unsupported volume unit.");
        throw new ArgumentException("Unsupported volume unit.");
    }
}
