using FluentAssertions;
using UnitService.Domain.Entities;
using UnitService.Domain.Events;

namespace UnitService.Tests.Domain;

public class EquipmentStatusTests
{
    [Fact]
    public void Create_ShouldCreateStatus_WithDomainEvent()
    {
        var status = EquipmentStatus.Create(1, 100, "Active", "ACT", "Initial", 500, 1);

        status.StatusId.Should().Be(1);
        status.EquipmentId.Should().Be(100);
        status.StatusDescription.Should().Be("Active");
        status.StatusCode.Should().Be("ACT");
        status.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<EquipmentStatusChangedEvent>();
    }

    [Fact]
    public void Close_ShouldSetCloseDate()
    {
        var status = EquipmentStatus.Create(1, 100, "Active", "ACT", null, null, 1);

        status.Close();

        status.CloseDate.Should().NotBeNull();
    }
}
