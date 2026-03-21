using FluentAssertions;
using Moq;
using UnitService.Application.Commands.RegisterEquipment;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;

namespace UnitService.Tests.Application;

public class RegisterEquipmentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IEquipmentRepository> _equipmentRepoMock = new();

    public RegisterEquipmentCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(u => u.Equipment).Returns(_equipmentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_NewEquipment_ShouldAddAndReturnId()
    {
        _equipmentRepoMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((EquipmentMaster?)null);

        var handler = new RegisterEquipmentCommandHandler(_unitOfWorkMock.Object);
        var command = new RegisterEquipmentCommand(1, "CNC Machine", "MCH", "Machinery", 100);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(1);
        _equipmentRepoMock.Verify(r => r.AddAsync(It.IsAny<EquipmentMaster>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
