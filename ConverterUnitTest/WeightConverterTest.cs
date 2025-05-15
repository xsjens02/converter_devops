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
        var result = converter.Convert(1000, EWeightUnit.Gram, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void Convert_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.Convert(-1, EWeightUnit.Gram, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Convert_ZeroValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.Convert(0, EWeightUnit.Kilogram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Convert_SameUnit()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.Convert(42, EWeightUnit.Gram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public void Convert_UnsupportedUnit()
    {
        // Arrange
        var converter = new WeightConverter();
        var unsupported = (EWeightUnit)(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            converter.Convert(100, EWeightUnit.Gram, unsupported);
        });
    }

    [Test]
    public void AddWeights()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.AddWeights(500, EWeightUnit.Gram, 0.5, EWeightUnit.Kilogram, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void AddWeights_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.AddWeights(-100, EWeightUnit.Gram, 200, EWeightUnit.Gram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void SubtractWeights()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.SubtractWeights(1000, EWeightUnit.Gram, 0.5, EWeightUnit.Kilogram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(500));
    }

    [Test]
    public void SubtractWeights_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.SubtractWeights(-1, EWeightUnit.Gram, 1, EWeightUnit.Gram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void ScaleWeight()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.ScaleWeight(2, EWeightUnit.Kilogram, 3, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(6000));
    }

    [Test]
    public void ScaleWeight_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.ScaleWeight(-1, EWeightUnit.Kilogram, 3, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void WeightDifference()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.WeightDifference(1, EWeightUnit.Kilogram, 500, EWeightUnit.Gram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(500));
    }

    [Test]
    public void WeightDifference_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.WeightDifference(-1, EWeightUnit.Kilogram, 500, EWeightUnit.Gram, EWeightUnit.Gram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
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

    [Test]
    public void WeightAsPercentage_NegativeValue()
    {
        // Arrange
        var converter = new WeightConverter();

        // Act
        var result = converter.WeightAsPercentage(-500, EWeightUnit.Gram, 2, EWeightUnit.Kilogram);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void ConvertToKilogram_ThrowsException()
    {
        // Arrange
        var converter = new WeightConverter();
        var unsupported = (EWeightUnit)(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            converter.AddWeights(1, unsupported, 1, EWeightUnit.Kilogram, EWeightUnit.Kilogram);
        });
    }

    [Test]
    public void ConvertFromKilogram_ThrowsException()
    {
        // Arrange
        var converter = new WeightConverter();
        var unsupported = (EWeightUnit)(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            converter.AddWeights(1, EWeightUnit.Kilogram, 1, EWeightUnit.Kilogram, unsupported);
        });
    }
}
