using Moq;
using TdsService.Application.Common.Exceptions;
using TdsService.Application.Vendors.Commands.CreateTdsVendor;
using TdsService.Application.Vendors.Commands.DeleteTdsVendor;
using TdsService.Application.Vendors.Commands.UpdateTdsVendor;
using TdsService.Domain.Entities;
using TdsService.Domain.Repositories;
using FluentAssertions;
using Xunit;

namespace TdsService.Application.Tests.Vendors;

public sealed class VendorCommandTests
{
    [Fact]
    public async Task CreateVendorCommand_ShouldAddAndSave()
    {
        var repoMock = new Mock<ITdsVendorRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<TdsVendor>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateTdsVendorCommandHandler(repoMock.Object);
        var command = new CreateTdsVendorCommand(1, "New Vendor", "nv@example.com", "ABCDE1234F");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(1);
        repoMock.Verify(r => r.AddAsync(It.IsAny<TdsVendor>(), It.IsAny<CancellationToken>()), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteVendorCommand_WhenVendorNotFound_ShouldThrowNotFoundException()
    {
        var repoMock = new Mock<ITdsVendorRepository>();
        repoMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TdsVendor?)null);

        var handler = new DeleteTdsVendorCommandHandler(repoMock.Object);

        var act = async () => await handler.Handle(new DeleteTdsVendorCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateVendorCommand_ShouldUpdateAndSave()
    {
        var vendor = TdsVendor.Create(1, "Old Name", null, null);
        vendor.ClearDomainEvents();

        var repoMock = new Mock<ITdsVendorRepository>();
        repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vendor);
        repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new UpdateTdsVendorCommandHandler(repoMock.Object);
        await handler.Handle(new UpdateTdsVendorCommand(1, "New Name", "new@example.com", null), CancellationToken.None);

        vendor.VendorName.Should().Be("New Name");
        repoMock.Verify(r => r.Update(It.IsAny<TdsVendor>()), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
