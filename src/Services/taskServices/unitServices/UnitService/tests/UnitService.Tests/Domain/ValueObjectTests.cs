using FluentAssertions;
using UnitService.Domain.ValueObjects;

namespace UnitService.Tests.Domain;

public class ValueObjectTests
{
    [Theory]
    [InlineData("MCH")]
    [InlineData("A")]
    [InlineData("AB")]
    public void UnitCode_ValidValues_ShouldCreate(string code)
    {
        var unitCode = UnitCode.From(code);
        unitCode.Value.Should().Be(code.ToUpperInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("ABCD")]
    public void UnitCode_InvalidValues_ShouldThrow(string code)
    {
        var act = () => UnitCode.From(code);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("R")]
    [InlineData("W")]
    [InlineData("A")]
    public void AccessType_ValidValues_ShouldCreate(string type)
    {
        var accessType = AccessType.From(type);
        accessType.Value.Should().Be(type);
    }

    [Fact]
    public void AccessType_InvalidValue_ShouldThrow()
    {
        var act = () => AccessType.From("X");
        act.Should().Throw<ArgumentException>();
    }
}
