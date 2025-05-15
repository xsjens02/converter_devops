using Monitoring;
using Monitoring.Logging;

namespace Converter.Weight
{
    public class WeightConverter : IWeightConverter
    {
        private readonly Dictionary<EWeightUnit, double> _weightUnitsInGram = new()
        {
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
            Logger.Log(ELogLevel.Debug, "Starting - [method:Convert] [class:WeightConverter]");
            
            if (value <= 0) return 0;

            if (from == to)
                return value;

            if (_weightUnitsInGram.ContainsKey(from) && _weightUnitsInGram.TryGetValue(to, out var conversionFactor))
                return ConvertToGram(value, from) / conversionFactor;

            Logger.Log(ELogLevel.Error, "Failed - [method:Convert] due to unsupported weight unit.");
            throw new ArgumentException("Unsupported weight unit.");
        }

        // Adds two weight values with different units and returns the total in the desired unit.
        public double AddWeights(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:AddWeights] [class:WeightConverter]");
            
            if (firstVal < 0 || secondVal < 0)
                return 0;

            var firstInGrams = ConvertToGram(firstVal, firstUnit);
            var secondInGrams = ConvertToGram(secondVal, secondUnit);
            var totalInGrams = firstInGrams + secondInGrams;
            return ConvertFromGram(totalInGrams, resultUnit);
        }

        // Subtracts the second weight from the first and returns the result in the desired unit.
        public double SubtractWeights(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:SubtractWeights] [class:WeightConverter]");
            
            if (firstVal < 0 || secondVal < 0)
                return 0;

            var firstInGrams = ConvertToGram(firstVal, firstUnit);
            var secondInGrams = ConvertToGram(secondVal, secondUnit);
            var resultInGrams = firstInGrams - secondInGrams;
            return ConvertFromGram(resultInGrams, resultUnit);
        }

        // Scales a weight value by a given factor and returns the result in the desired unit.
        public double ScaleWeight(double value, EWeightUnit unit, double factor, EWeightUnit resultUnit)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:ScaleWeight] [class:WeightConverter]");
            
            if (value <= 0 || factor < 0)
                return 0;

            var valueInGrams = ConvertToGram(value, unit);
            var scaledInGrams = valueInGrams * factor;
            return ConvertFromGram(scaledInGrams, resultUnit);
        }

        // Calculates the absolute difference between two weight values.
        public double WeightDifference(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:WeightDifference] [class:WeightConverter]");
            
            if (firstVal < 0 || secondVal < 0)
                return 0;

            var firstInGrams = ConvertToGram(firstVal, firstUnit);
            var secondInGrams = ConvertToGram(secondVal, secondUnit);
            var differenceInGrams = Math.Abs(firstInGrams - secondInGrams);
            return ConvertFromGram(differenceInGrams, resultUnit);
        }

        // Calculates what percentage one weight is of another.
        public double WeightAsPercentage(double value, EWeightUnit unit, double totalValue, EWeightUnit totalUnit)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:WeightAsPercentage] [class:WeightConverter]");
            
            if (value <= 0 || totalValue <= 0)
                return 0;

            var valueInGrams = ConvertToGram(value, unit);
            var totalInGrams = ConvertToGram(totalValue, totalUnit);
            return (valueInGrams / totalInGrams) * 100;
        }

        private double ConvertToGram(double value, EWeightUnit from)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:ConvertToGram] [class:WeightConverter]");
            
            if (_weightUnitsInGram.TryGetValue(from, out var valueInGram))
                return value * valueInGram;

            Logger.Log(ELogLevel.Error, "Failed - [method:ConvertToGram] due to unsupported weight unit.");
            throw new ArgumentException("Unsupported weight unit.");
        }

        private double ConvertFromGram(double valueInGram, EWeightUnit to)
        {
            using var activity = MonitorService.ActivitySource.StartActivity();
            Logger.Log(ELogLevel.Debug, "Starting - [method:ConvertFromGram] [class:WeightConverter]");
            
            if (_weightUnitsInGram.TryGetValue(to, out var unitFactor))
                return valueInGram / unitFactor;

            Logger.Log(ELogLevel.Error, "Failed - [method:ConvertFromGram] due to unsupported weight unit.");
            throw new ArgumentException("Unsupported weight unit.");
        }
    }
}