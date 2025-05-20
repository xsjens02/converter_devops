using Converter.Weight;

namespace ConverterUnitTest;

public class WeightConverterTest
{
    [Test]
    public void Convert()
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.Convert(1, EWeightUnit.Kilogram, EWeightUnit.Pound);

        // Assert
        Assert.That(result, Is.EqualTo(2.204).Within(0.01));
    }
    
    [Test]
    public void Convert_SameUnit()
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.Convert(10, EWeightUnit.Gram, EWeightUnit.Gram);
        
        // Assert
        Assert.That(result, Is.EqualTo(10));
    }
    
    [Test]
    public void Convert_ZeroValue()
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.Convert(0, EWeightUnit.Gram, EWeightUnit.Kilogram);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void Convert_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.Convert(-1, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void Convert_UnsupportedUnit()
    {
        // Arrange
        var converter = new WeightConverter();
        var unsupportedUnit = (EWeightUnit)(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.Convert(1, EWeightUnit.Kilogram, unsupportedUnit);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Unsupported weight unit."));
    }
    
    [Test]
    public void AddWeights()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.AddWeights(500, EWeightUnit.Gram, 1, EWeightUnit.Kilogram, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(1.5));
    }
    
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    public void AddWeights_WithZero(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.AddWeights(firstValue, EWeightUnit.Kilogram, secondValue, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void AddWeights_WithNegative(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.AddWeights(firstValue, EWeightUnit.Kilogram, secondValue, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void SubtractWeights()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.SubtractWeights(2, EWeightUnit.Kilogram, 500, EWeightUnit.Gram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(1500)); 
    }
    
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    public void SubtractWeights_WithZero(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.SubtractWeights(firstValue, EWeightUnit.Kilogram, secondValue, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void SubtractWeights_WithNegative(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.SubtractWeights(firstValue, EWeightUnit.Kilogram, secondValue, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void ScaleWeight()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.ScaleWeight(2, EWeightUnit.Kilogram, 3, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(6)); 
    }
    
    [Test]
    public void ScaleWeight_WithZeroValue()
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.ScaleWeight(0, EWeightUnit.Kilogram, 5, EWeightUnit.Kilogram);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void ScaleWeight_WithZeroFactor()
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.ScaleWeight(5, EWeightUnit.Kilogram, 0, EWeightUnit.Kilogram);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void ScaleWeight_WithNegative(double firstValue, double factor)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.ScaleWeight(firstValue, EWeightUnit.Kilogram, factor, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void WeightDifference()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.WeightDifference(5, EWeightUnit.Kilogram, 3, EWeightUnit.Kilogram, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(2)); 
    }
    
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    public void WeightDifference_WithZero(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var result = converter.WeightDifference(firstValue, EWeightUnit.Kilogram, secondValue, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        
        // Assert
        Assert.That(result, Is.EqualTo(5));
    }
    
    [TestCase(-1, 5)]
    [TestCase(5, -1)]
    public void WeightDifference_WithNegative(double firstValue, double secondValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.WeightDifference(firstValue, EWeightUnit.Kilogram, secondValue, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be non-negative for conversion."));
    }
    
    [Test]
    public void WeightAsPercentage()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.WeightAsPercentage(500, EWeightUnit.Gram, 2, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(25));
    }
    
    [TestCase(-1, 5)]
    [TestCase(0, 5)]
    [TestCase(5, -1)]
    [TestCase(5, 0)]
    public void WeightAsPercentage_InvalidInputs(double value, double totalValue)
    {
        // Arrange
        var converter = new WeightConverter();
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.WeightAsPercentage(value, EWeightUnit.Gram, totalValue, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Invalid, value(s) must be higher than zero."));
    }
    
    [Test]
    public void ConvertToKilogram_ThrowsException()
    {
        // Arrange
        var converter = new WeightConverter();
        var unsupported = (EWeightUnit)(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.AddWeights(1, unsupported, 1, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        });
        
        // Assert
        Assert.That(ex.Message, Is.EqualTo("Unsupported weight unit."));
    }
    
    [Test]
    public void ConvertFromKilogram_ThrowsException()
    {
        // Arrange
        var converter = new WeightConverter();
        var unsupported = (EWeightUnit)(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            converter.AddWeights(1, EWeightUnit.Kilogram, 100, EWeightUnit.Gram, unsupported);
        });

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Unsupported weight unit."));
    }
}