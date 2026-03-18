using FluentAssertions;
using TdsService.Domain.Exceptions;
using TdsService.Domain.ValueObjects;
using Xunit;

namespace TdsService.Domain.Tests.ValueObjects;

public sealed class PanNumberTests
{
    [Theory]
    [InlineData("ABCDE1234F")]
    [InlineData("ZZZZZ9999Z")]
    [InlineData("BCDEF2345G")]
    public void Create_WithValidPan_ShouldSucceed(string pan)
    {
        var panNumber = PanNumber.Create(pan);
        panNumber.Value.Should().Be(pan);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("ABCDE123")]   // too short
    [InlineData("ABCDE1234X1")]// too long
    [InlineData("abcde1234f")] // not uppercase (BUT Create normalises, so this SHOULD work)
    public void Create_WithInvalidPan_ShouldThrowOrNormalise(string? pan)
    {
        if (pan is null || pan.Length == 0)
        {
            var act = () => PanNumber.Create(pan);
            act.Should().Throw<InvalidPanNumberException>();
        }
        else if (pan.Length < 10 || pan.Length > 10)
        {
            var act = () => PanNumber.Create(pan);
            act.Should().Throw<InvalidPanNumberException>();
        }
        else
        {
            // Lowercase should be normalised
            var panNumber = PanNumber.Create(pan);
            panNumber.Value.Should().Be(pan.ToUpperInvariant());
        }
    }

    [Fact]
    public void TwoEquivalentPanNumbers_ShouldBeEqual()
    {
        var pan1 = PanNumber.Create("ABCDE1234F");
        var pan2 = PanNumber.Create("ABCDE1234F");

        pan1.Should().Be(pan2);
        (pan1 == pan2).Should().BeTrue();
    }
}
