using FluentAssertions;
using Moq;
using VendorService.Application.Commands;
using VendorService.Domain.Interfaces;

namespace VendorService.UnitTests.Commands;

public sealed class CreateVendorCommandHandlerTests
{
    private readonly Mock<IVendorRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsNewVendorId()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddUpdateVendorSpAsync(
                null, 1, 1, "Test Vendor", null, "123 Street", 1, 'A', It.IsAny<CancellationToken>()))
            .ReturnsAsync(42L);

        var handler = new CreateVendorCommandHandler(_repositoryMock.Object);
        var command = new CreateVendorCommand(1, 1, "Test Vendor", null, "123 Street", 1, 'A');

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(42L);
        _repositoryMock.Verify(r => r.AddUpdateVendorSpAsync(
            null, 1, 1, "Test Vendor", null, "123 Street", 1, 'A', It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
