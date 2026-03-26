using FluentAssertions;
using ItemMasterService.Domain.Entities;
using Xunit;

namespace ItemMasterService.Tests.Domain;

public class CanteenItemPriceMasterTests
{
    // ── Create ───────────────────────────────────────────────────────────────

    [Fact]
    public void Create_WithValidInputs_SetsAllProperties()
    {
        var effectiveDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var price = CanteenItemPriceMaster.Create(1001, 5, 25.50m, 50.00m, effectiveDate, "admin");

        price.CanteenUnitCode.Should().Be(1001);
        price.ItemCode.Should().Be(5);
        price.EmployeeContribution.Should().Be(25.50m);
        price.EmployerContribution.Should().Be(50.00m);
        price.EffectiveDate.Should().Be(effectiveDate);
        price.ClosureDate.Should().BeNull();
        price.EnteredBy.Should().Be("admin");
        price.EnteredOn.Should().NotBeNull();
    }

    [Fact]
    public void Create_WithNullContributions_SetsContributionsToNull()
    {
        var effectiveDate = DateTime.UtcNow;

        var price = CanteenItemPriceMaster.Create(1001, 5, null, null, effectiveDate, "admin");

        price.EmployeeContribution.Should().BeNull();
        price.EmployerContribution.Should().BeNull();
    }

    [Fact]
    public void Create_WithEnteredByLongerThan50Chars_TruncatesToFiftyChars()
    {
        var longEnteredBy = new string('U', 60);

        var price = CanteenItemPriceMaster.Create(1001, 5, 10m, 20m, DateTime.UtcNow, longEnteredBy);

        price.EnteredBy.Should().HaveLength(50);
    }

    [Fact]
    public void Create_SetsEnteredOnToApproximatelyNow()
    {
        var before = DateTime.UtcNow;
        var price = CanteenItemPriceMaster.Create(1001, 5, 10m, 20m, DateTime.UtcNow, "user");
        var after = DateTime.UtcNow;

        price.EnteredOn.Should().NotBeNull();
        price.EnteredOn!.Value.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    // ── Close ────────────────────────────────────────────────────────────────

    [Fact]
    public void Close_SetsClosureDate()
    {
        var price = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, DateTime.UtcNow, "admin");
        var closureDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        price.Close(closureDate);

        price.ClosureDate.Should().Be(closureDate);
    }

    [Fact]
    public void Close_SetsUpdatedAt()
    {
        var price = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, DateTime.UtcNow, "admin");

        price.Close(DateTime.UtcNow.AddDays(30));

        price.UpdatedAt.Should().NotBeNull();
        price.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    // ── UpdatePrice ──────────────────────────────────────────────────────────

    [Fact]
    public void UpdatePrice_WithNewContributions_UpdatesContributions()
    {
        var price = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, DateTime.UtcNow, "admin");

        price.UpdatePrice(30m, 60m);

        price.EmployeeContribution.Should().Be(30m);
        price.EmployerContribution.Should().Be(60m);
    }

    [Fact]
    public void UpdatePrice_SetsUpdatedAt()
    {
        var price = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, DateTime.UtcNow, "admin");

        price.UpdatePrice(30m, 60m);

        price.UpdatedAt.Should().NotBeNull();
    }
}
