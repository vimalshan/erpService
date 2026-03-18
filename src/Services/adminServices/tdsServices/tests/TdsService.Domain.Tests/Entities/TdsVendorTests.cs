using FluentAssertions;
using TdsService.Domain.Entities;
using TdsService.Domain.Events;
using TdsService.Domain.Exceptions;
using TdsService.Domain.ValueObjects;
using Xunit;

namespace TdsService.Domain.Tests.Entities;

public sealed class TdsVendorTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Act
        var vendor = TdsVendor.Create(1, "Acme Ltd", "acme@example.com", "ABCDE1234F");

        // Assert
        vendor.Id.Should().Be(1);
        vendor.VendorName.Should().Be("Acme Ltd");
        vendor.EmailAddress!.Value.Should().Be("acme@example.com");
        vendor.PanNumber!.Value.Should().Be("ABCDE1234F");
    }

    [Fact]
    public void Create_ShouldRaiseTdsVendorCreatedEvent()
    {
        var vendor = TdsVendor.Create(1, "Acme Ltd", null, "ABCDE1234F");

        vendor.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TdsVendorCreatedEvent>();
    }

    [Fact]
    public void Create_WithNullEmail_ShouldHaveNullEmailAddress()
    {
        var vendor = TdsVendor.Create(1, "Test Vendor", null, null);

        vendor.EmailAddress.Should().BeNull();
        vendor.PanNumber.Should().BeNull();
    }

    [Fact]
    public void Update_ShouldRaiseTdsVendorUpdatedEvent()
    {
        var vendor = TdsVendor.Create(1, "Old Name", null, null);
        vendor.ClearDomainEvents();

        vendor.Update("New Name", "new@example.com", "ZZZZZ9999Z");

        vendor.VendorName.Should().Be("New Name");
        vendor.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<TdsVendorUpdatedEvent>();
    }
}
