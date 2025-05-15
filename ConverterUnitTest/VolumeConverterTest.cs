using Converter.Volume;

namespace ConverterUnitTest;

public class VolumeConverterTest
{
    [Test]
    public void Convert()
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.Convert(1, EVolumeUnit.Deciliter, EVolumeUnit.Cup);

        // Assert
        Assert.That(result, Is.EqualTo(0.41).Within(0.01));
    }
    
    [Test]
    public void Convert_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.Convert(-1, EVolumeUnit.Cup, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Convert_ZeroValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.Convert(0, EVolumeUnit.Cup, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void Convert_SameUnit()
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.Convert(10, EVolumeUnit.Liter, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(10));
    }
    
    [Test]
    public void Convert_UnsupportedUnit()
    {
        // Arrange
        var converter = new VolumeConverter();
        var unsupportedUnit = (EVolumeUnit)(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            converter.Convert(1, EVolumeUnit.Liter, unsupportedUnit);
        });
    }
    
    [Test]
    public void AddVolumes()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.AddVolumes(500, EVolumeUnit.Milliliter, 5, EVolumeUnit.Deciliter, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
    
    [Test]
    public void AddVolumes_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.AddVolumes(-1, EVolumeUnit.Milliliter, 5, EVolumeUnit.Deciliter, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void SubtractVolumes()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.SubtractVolumes(500, EVolumeUnit.Milliliter, 2, EVolumeUnit.Deciliter, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(0.3)); 
    }
    
    [Test]
    public void SubtractVolumes_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.SubtractVolumes(-1, EVolumeUnit.Milliliter, 2, EVolumeUnit.Deciliter, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(0)); 
    }
    
    [Test]
    public void ScaleVolume()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.ScaleVolume(2, EVolumeUnit.Liter, 3, EVolumeUnit.Milliliter);

        // Assert
        Assert.That(result, Is.EqualTo(6000)); 
    }
    
    [Test]
    public void ScaleVolume_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.ScaleVolume(-1, EVolumeUnit.Liter, 3, EVolumeUnit.Milliliter);

        // Assert
        Assert.That(result, Is.EqualTo(0)); 
    }
    
    [Test]
    public void VolumeDifference()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.VolumeDifference(1, EVolumeUnit.Liter, 4, EVolumeUnit.Deciliter, EVolumeUnit.Milliliter);

        // Assert
        Assert.That(result, Is.EqualTo(600)); 
    }
    
    [Test]
    public void VolumeDifference_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.VolumeDifference(-1, EVolumeUnit.Liter, 4, EVolumeUnit.Deciliter, EVolumeUnit.Milliliter);

        // Assert
        Assert.That(result, Is.EqualTo(0)); 
    }
    
    [Test]
    public void VolumeAsPercentage()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.VolumeAsPercentage(1, EVolumeUnit.Centiliter, 1, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(10)); 
    }
    
    [Test]
    public void VolumeAsPercentage_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.VolumeAsPercentage(-1, EVolumeUnit.Centiliter, 1, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(0)); 
    }
    
    [Test]
    public void ConvertToLiter_ThrowsException()
    {
        // Arrange
        var converter = new VolumeConverter();
        var unsupported = (EVolumeUnit)(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            converter.AddVolumes(1, unsupported, 1, EVolumeUnit.Liter, EVolumeUnit.Liter);
        });
    }
    
    [Test]
    public void ConvertFromLiter_ThrowsException()
    {
        // Arrange
        var converter = new VolumeConverter();
        var unsupported = (EVolumeUnit)(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            converter.AddVolumes(1, EVolumeUnit.Liter, 100, EVolumeUnit.Milliliter, unsupported);
        });
    }
}