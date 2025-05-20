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
    public void Convert_ZeroValue()
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.Convert(0, EVolumeUnit.Liter, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void Convert_NegativeValue()
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.Convert(-1, EVolumeUnit.Liter, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void Convert_UnsupportedUnit()
    {
        // Arrange
        var converter = new VolumeConverter();
        var unsupportedUnit = (EVolumeUnit)(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.Convert(1, EVolumeUnit.Liter, unsupportedUnit);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Unsupported volume unit."));
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
    
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    public void AddVolumes_WithZero(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.AddVolumes(firstValue, EVolumeUnit.Liter, secondValue, EVolumeUnit.Liter, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void AddVolumes_WithNegative(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.AddVolumes(firstValue, EVolumeUnit.Liter, secondValue, EVolumeUnit.Liter, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
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
    
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    public void SubtractVolumes_WithZero(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.SubtractVolumes(firstValue, EVolumeUnit.Liter, secondValue, EVolumeUnit.Liter, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void SubtractVolumes_WithNegative(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.SubtractVolumes(firstValue, EVolumeUnit.Liter, secondValue, EVolumeUnit.Liter, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
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
    public void ScaleVolume_WithZeroValue()
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.ScaleVolume(0, EVolumeUnit.Liter, 5, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void ScaleVolume_WithZeroFactor()
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.ScaleVolume(5, EVolumeUnit.Liter, 0, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void ScaleVolume_WithNegative(double firstValue, double factor)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.ScaleVolume(firstValue, EVolumeUnit.Liter, factor, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
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
    
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    public void VolumeDifference_WithZero(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var result = converter.VolumeDifference(firstValue, EVolumeUnit.Liter, secondValue, EVolumeUnit.Liter, EVolumeUnit.Liter);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void VolumeDifference_WithNegative(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.VolumeDifference(firstValue, EVolumeUnit.Liter, secondValue, EVolumeUnit.Liter, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void VolumeAsPercentage()
    {
        // Arrange
        var converter = new VolumeConverter();

        // Act
        var result = converter.VolumeAsPercentage(2, EVolumeUnit.Liter, 0.5, EVolumeUnit.Liter);

        // Assert
        Assert.That(result, Is.EqualTo(400));
    }
    
    [TestCase(-1, 5)]
    [TestCase(0, 5)]
    [TestCase(5, -1)]
    [TestCase(5, 0)]
    public void VolumeAsPercentage_InvalidInputs(double value, double totalValue)
    {
        // Arrange
        var converter = new VolumeConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.VolumeAsPercentage(value, EVolumeUnit.Liter, totalValue, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be higher than zero."));
    }
    
    [Test]
    public void ConvertToLiter_ThrowsException()
    {
        // Arrange
        var converter = new VolumeConverter();
        var unsupported = (EVolumeUnit)(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.AddVolumes(1, unsupported, 1, EVolumeUnit.Liter, EVolumeUnit.Liter);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Unsupported volume unit."));
    }
    
    [Test]
    public void ConvertFromLiter_ThrowsException()
    {
        // Arrange
        var converter = new VolumeConverter();
        var unsupported = (EVolumeUnit)(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.AddVolumes(1, EVolumeUnit.Liter, 100, EVolumeUnit.Milliliter, unsupported);
        });

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Unsupported volume unit."));
    }
}