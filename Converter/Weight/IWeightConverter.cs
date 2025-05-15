namespace Converter.Weight;

public interface IWeightConverter
{
    double Convert(double value, EWeightUnit from, EWeightUnit to);

    double AddWeights(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit);

    double SubtractWeights(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit);
    double ScaleWeight(double value, EWeightUnit unit, double factor, EWeightUnit resultUnit);

    double WeightDifference(double firstVal, EWeightUnit firstUnit, double secondVal, EWeightUnit secondUnit, EWeightUnit resultUnit);
    double WeightAsPercentage(double value, EWeightUnit unit, double totalValue, EWeightUnit totalUnit);
}