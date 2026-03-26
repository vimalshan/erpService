using FluentAssertions;
using ItemMasterService.Domain.Entities;
using Xunit;
using ItemMasterService.Domain.Events;

namespace ItemMasterService.Tests.Domain;

public class CanteenItemMasterTests
{
    // ── Create ───────────────────────────────────────────────────────────────

    [Fact]
    public void Create_WithValidInputs_SetsAllProperties()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");

        item.CanteenUnitCode.Should().Be(1001);
        item.ItemCode.Should().Be(5);
        item.ItemDescription.Should().Be("Rice Meal");
        item.ItemType.Should().Be("F");
        item.ItemReference.Should().Be("RICE01");
        item.EnteredBy.Should().Be("admin");
        item.EnteredOn.Should().NotBeNull();
        item.EnteredOn!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_WithDescriptionLongerThan50Chars_TruncatesToFiftyChars()
    {
        var longDescription = new string('A', 60);

        var item = CanteenItemMaster.Create(1001, 1, longDescription, "F", "REF", "user");

        item.ItemDescription.Should().HaveLength(50);
        item.ItemDescription.Should().Be(new string('A', 50));
    }

    [Fact]
    public void Create_WithItemTypeLongerThanOneChar_TruncatesToOneChar()
    {
        var item = CanteenItemMaster.Create(1001, 1, "Desc", "FOOD", "REF", "user");

        item.ItemType.Should().Be("F");
    }

    [Fact]
    public void Create_WithReferenceLongerThan10Chars_TruncatesToTenChars()
    {
        var longReference = new string('R', 15);

        var item = CanteenItemMaster.Create(1001, 1, "Desc", "F", longReference, "user");

        item.ItemReference.Should().HaveLength(10);
    }

    [Fact]
    public void Create_WithEnteredByLongerThan50Chars_TruncatesToFiftyChars()
    {
        var longEnteredBy = new string('U', 60);

        var item = CanteenItemMaster.Create(1001, 1, "Desc", "F", "REF", longEnteredBy);

        item.EnteredBy.Should().HaveLength(50);
    }

    [Fact]
    public void Create_WithNullItemType_SetsItemTypeToNull()
    {
        var item = CanteenItemMaster.Create(1001, 1, "Desc", null, "REF", "user");

        item.ItemType.Should().BeNull();
    }

    [Fact]
    public void Create_AddsCanteenItemCreatedDomainEvent()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");

        item.DomainEvents.Should().ContainSingle();
        item.DomainEvents.Single().Should().BeOfType<CanteenItemCreatedEvent>();
    }

    [Fact]
    public void Create_IncrementsVersionToOne()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");

        item.Version.Should().Be(1);
    }

    [Fact]
    public void Create_SetsCreatedAtToApproximatelyNow()
    {
        var before = DateTime.UtcNow;
        var item = CanteenItemMaster.Create(1001, 1, "Desc", "F", "REF", "user");
        var after = DateTime.UtcNow;

        item.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    // ── Update ───────────────────────────────────────────────────────────────

    [Fact]
    public void Update_WithNewValues_UpdatesProperties()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        item.ClearDomainEvents();

        item.Update("Chicken Curry", "B", "CHICK1");

        item.ItemDescription.Should().Be("Chicken Curry");
        item.ItemType.Should().Be("B");
        item.ItemReference.Should().Be("CHICK1");
    }

    [Fact]
    public void Update_SetsUpdatedAt()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        item.ClearDomainEvents();

        item.Update("New Desc", "C", "NEWREF");

        item.UpdatedAt.Should().NotBeNull();
        item.UpdatedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_AddsCanteenItemUpdatedDomainEvent()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        item.ClearDomainEvents();

        item.Update("New Desc", "C", "NEWREF");

        item.DomainEvents.Should().ContainSingle();
        item.DomainEvents.Single().Should().BeOfType<CanteenItemUpdatedEvent>();
    }

    [Fact]
    public void Update_WithDescriptionLongerThan50Chars_TruncatesToFiftyChars()
    {
        var item = CanteenItemMaster.Create(1001, 1, "Desc", "F", "REF", "user");
        item.ClearDomainEvents();
        var longDescription = new string('B', 60);

        item.Update(longDescription, "F", "REF");

        item.ItemDescription.Should().HaveLength(50);
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");

        item.ClearDomainEvents();

        item.DomainEvents.Should().BeEmpty();
    }
}
