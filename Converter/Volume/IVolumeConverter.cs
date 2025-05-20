namespace Converter.Volume;

public interface IVolumeConverter
{
    double Convert(double value, EVolumeUnit from, EVolumeUnit to);

    double AddVolumes(double firstVal, EVolumeUnit firstUnit, double secondVal, EVolumeUnit secondUnit, EVolumeUnit resultUnit);

    double SubtractVolumes(double firstVal, EVolumeUnit firstUnit, double secondVal, EVolumeUnit secondUnit, EVolumeUnit resultUnit);
    double ScaleVolume(double value, EVolumeUnit unit, double factor, EVolumeUnit resultUnit);

    double VolumeDifference(double firstVal, EVolumeUnit firstUnit, double secondVal, EVolumeUnit secondUnit, EVolumeUnit resultUnit);
    double VolumeAsPercentage(double value, EVolumeUnit unit, double totalValue, EVolumeUnit totalUnit);
}