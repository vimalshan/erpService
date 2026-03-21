using FluentAssertions;
using UnitService.Domain.Entities;
using UnitService.Domain.Events;

namespace UnitService.Tests.Domain;

public class EquipmentMasterTests
{
    [Fact]
    public void Create_ShouldCreateEquipment_WithDomainEvent()
    {
        var equipment = EquipmentMaster.Create(1, "CNC Machine", "MCH", "Machinery", 100);

        equipment.EquipmentId.Should().Be(1);
        equipment.EquipmentName.Should().Be("CNC Machine");
        equipment.UnitCode.Value.Should().Be("MCH");
        equipment.Category.Should().Be("Machinery");
        equipment.CloseDate.Should().BeNull();
        equipment.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<EquipmentRegisteredEvent>();
    }

    [Fact]
    public void Update_ShouldUpdateProperties()
    {
        var equipment = EquipmentMaster.Create(1, "CNC Machine", "MCH", "Machinery", 100);
        equipment.ClearDomainEvents();

        equipment.Update("Updated CNC Machine", "Heavy Machinery", 200);

        equipment.EquipmentName.Should().Be("Updated CNC Machine");
        equipment.Category.Should().Be("Heavy Machinery");
        equipment.LastModifiedBy.Should().Be(200);
    }

    [Fact]
    public void Close_ShouldSetCloseDate()
    {
        var equipment = EquipmentMaster.Create(1, "CNC Machine", "MCH", "Machinery", 100);

        equipment.Close(200);

        equipment.CloseDate.Should().NotBeNull();
        equipment.LastModifiedBy.Should().Be(200);
    }
}
