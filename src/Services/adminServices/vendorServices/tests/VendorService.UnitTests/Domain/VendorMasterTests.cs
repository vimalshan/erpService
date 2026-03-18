using FluentAssertions;
using VendorService.Domain.Entities;
using VendorService.Domain.Exceptions;
using VendorService.Domain.Events;

namespace VendorService.UnitTests.Domain;

public sealed class VendorMasterTests
{
    [Fact]
    public void Create_WithValidData_RaisesVendorCreatedEvent()
    {
        // Arrange & Act
        var vendor = VendorMaster.Create(1, 2, 3, "ACME", null, "456 Lane", 1);

        // Assert
        vendor.DomainEvents.Should().ContainSingle(e => e is VendorCreatedEvent);
        vendor.Name.Value.Should().Be("ACME");
        vendor.CategoryId.Should().Be(2);
        vendor.LocationId.Should().Be(3);
        vendor.LiveStatus.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithInvalidCategoryId_ThrowsDomainException()
    {
        // Arrange & Act
        Action act = () => VendorMaster.Create(1, 0, 3, "ACME", null, "456 Lane", 1);

        // Assert
        act.Should().Throw<VendorDomainException>()
            .WithMessage("*Category ID*");
    }

    [Fact]
    public void Update_ChangesStatus_RaisesStatusChangedEvent()
    {
        // Arrange
        var vendor = VendorMaster.Create(1, 2, 3, "ACME", null, "456 Lane", 1);
        vendor.ClearDomainEvents();

        // Act
        vendor.Update(2, 3, "ACME Updated", null, "456 Lane", 1, 'I');

        // Assert
        vendor.DomainEvents.Should().Contain(e => e is VendorStatusChangedEvent);
        vendor.LiveStatus.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ThrowsDomainException()
    {
        // Arrange
        var vendor = VendorMaster.Create(1, 2, 3, "ACME", null, "456 Lane", 1, 'I');

        // Act
        Action act = () => vendor.Deactivate(1);

        // Assert
        act.Should().Throw<VendorDomainException>();
    }

    [Fact]
    public void Resilience_CircuitBreaker_PolicyIsCreated()
    {
        // Arrange & Act
        var pipeline = VendorService.Infrastructure.Resilience.ResiliencePolicies.DatabaseCircuitBreaker();

        // Assert
        pipeline.Should().NotBeNull();
    }
}
