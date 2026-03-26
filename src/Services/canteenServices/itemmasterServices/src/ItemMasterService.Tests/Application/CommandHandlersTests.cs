using FluentAssertions;
using ItemMasterService.Application.CQRS.Commands;
using Xunit;
using ItemMasterService.Application.CQRS.Handlers;
using ItemMasterService.Domain.Entities;
using ItemMasterService.Domain.Exceptions;
using ItemMasterService.Domain.Interfaces;
using Moq;

namespace ItemMasterService.Tests.Application;

// ════════════════════════════════════════════════════════════════════════════
// CanteenItem Command Handlers
// ════════════════════════════════════════════════════════════════════════════

public class CreateCanteenItemCommandHandlerTests
{
    private readonly Mock<ICanteenItemRepository> _repoMock = new();
    private readonly CreateCanteenItemCommandHandler _handler;

    public CreateCanteenItemCommandHandlerTests()
    {
        _handler = new CreateCanteenItemCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenItemDoesNotExist_CreatesAndReturnsDto()
    {
        _repoMock.Setup(r => r.ExistsAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CanteenItemMaster>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new CreateCanteenItemCommand(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.CanteenUnitCode.Should().Be(1001);
        result.ItemCode.Should().Be(5);
        result.ItemDescription.Should().Be("Rice Meal");
        result.EnteredBy.Should().Be("admin");
    }

    [Fact]
    public async Task Handle_WhenItemAlreadyExists_ThrowsDuplicateItemException()
    {
        _repoMock.Setup(r => r.ExistsAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

        var command = new CreateCanteenItemCommand(1001, 5, "Rice Meal", "F", "RICE01", "admin");

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should().ThrowAsync<DuplicateItemException>()
                      .WithMessage("*5*1001*");
    }

    [Fact]
    public async Task Handle_WhenItemDoesNotExist_CallsAddAndSaveChanges()
    {
        _repoMock.Setup(r => r.ExistsAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CanteenItemMaster>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new CreateCanteenItemCommand(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<CanteenItemMaster>(), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class UpdateCanteenItemCommandHandlerTests
{
    private readonly Mock<ICanteenItemRepository> _repoMock = new();
    private readonly UpdateCanteenItemCommandHandler _handler;

    public UpdateCanteenItemCommandHandlerTests()
    {
        _handler = new UpdateCanteenItemCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenItemExists_UpdatesAndReturnsDto()
    {
        var existingItem = CanteenItemMaster.Create(1001, 5, "Old Description", "F", "REF01", "admin");
        _repoMock.Setup(r => r.GetByIdAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingItem);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new UpdateCanteenItemCommand(1001, 5, "New Description", "B", "NEWREF");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.ItemDescription.Should().Be("New Description");
        result.ItemType.Should().Be("B");
        result.ItemReference.Should().Be("NEWREF");
    }

    [Fact]
    public async Task Handle_WhenItemNotFound_ThrowsItemNotFoundException()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1001, 99, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenItemMaster?)null);

        var command = new UpdateCanteenItemCommand(1001, 99, "Updated", "F", "REF");

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should().ThrowAsync<ItemNotFoundException>()
                      .WithMessage("*99*1001*");
    }

    [Fact]
    public async Task Handle_WhenItemExists_CallsUpdateAndSaveChanges()
    {
        var existingItem = CanteenItemMaster.Create(1001, 5, "Old", "F", "REF", "admin");
        _repoMock.Setup(r => r.GetByIdAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingItem);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new UpdateCanteenItemCommand(1001, 5, "New", "B", "NREF");
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.Update(existingItem), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class DeleteCanteenItemCommandHandlerTests
{
    private readonly Mock<ICanteenItemRepository> _repoMock = new();
    private readonly DeleteCanteenItemCommandHandler _handler;

    public DeleteCanteenItemCommandHandlerTests()
    {
        _handler = new DeleteCanteenItemCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenItemExists_DeletesAndReturnsTrue()
    {
        var existingItem = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        _repoMock.Setup(r => r.GetByIdAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingItem);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new DeleteCanteenItemCommand(1001, 5);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenItemNotFound_ThrowsItemNotFoundException()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1001, 99, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenItemMaster?)null);

        var command = new DeleteCanteenItemCommand(1001, 99);

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenItemExists_CallsDeleteAndSaveChanges()
    {
        var existingItem = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        _repoMock.Setup(r => r.GetByIdAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingItem);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new DeleteCanteenItemCommand(1001, 5);
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.Delete(existingItem), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

// ════════════════════════════════════════════════════════════════════════════
// ItemPrice Command Handlers
// ════════════════════════════════════════════════════════════════════════════

public class CreateItemPriceCommandHandlerTests
{
    private readonly Mock<ICanteenItemPriceRepository> _repoMock = new();
    private readonly CreateItemPriceCommandHandler _handler;

    public CreateItemPriceCommandHandlerTests()
    {
        _handler = new CreateItemPriceCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_CreatesAndReturnsDto()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CanteenItemPriceMaster>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var effectiveDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var command = new CreateItemPriceCommand(1001, 5, 25m, 50m, effectiveDate, "admin");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.CanteenUnitCode.Should().Be(1001);
        result.ItemCode.Should().Be(5);
        result.EmployeeContribution.Should().Be(25m);
        result.EmployerContribution.Should().Be(50m);
        result.EffectiveDate.Should().Be(effectiveDate);
        result.EnteredBy.Should().Be("admin");
    }

    [Fact]
    public async Task Handle_CallsAddAndSaveChanges()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CanteenItemPriceMaster>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new CreateItemPriceCommand(1001, 5, 25m, 50m, DateTime.UtcNow, "admin");
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<CanteenItemPriceMaster>(), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class CloseItemPriceCommandHandlerTests
{
    private readonly Mock<ICanteenItemPriceRepository> _repoMock = new();
    private readonly CloseItemPriceCommandHandler _handler;

    public CloseItemPriceCommandHandlerTests()
    {
        _handler = new CloseItemPriceCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenActivePriceExists_ClosesAndReturnsTrue()
    {
        var activePriceEntity = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, DateTime.UtcNow.AddMonths(-1), "admin");
        _repoMock.Setup(r => r.GetActiveAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(activePriceEntity);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var closureDate = DateTime.UtcNow;
        var command = new CloseItemPriceCommand(1001, 5, closureDate);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        activePriceEntity.ClosureDate.Should().Be(closureDate);
    }

    [Fact]
    public async Task Handle_WhenNoActivePrice_ThrowsDomainException()
    {
        _repoMock.Setup(r => r.GetActiveAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenItemPriceMaster?)null);

        var command = new CloseItemPriceCommand(1001, 5, DateTime.UtcNow);

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should().ThrowAsync<DomainException>()
                      .WithMessage("*5*");
    }

    [Fact]
    public async Task Handle_WhenActivePriceExists_CallsUpdateAndSaveChanges()
    {
        var activePriceEntity = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, DateTime.UtcNow.AddMonths(-1), "admin");
        _repoMock.Setup(r => r.GetActiveAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(activePriceEntity);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new CloseItemPriceCommand(1001, 5, DateTime.UtcNow);
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.Update(activePriceEntity), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

// ════════════════════════════════════════════════════════════════════════════
// GradeItemPrice Command Handlers
// ════════════════════════════════════════════════════════════════════════════

public class CreateGradeItemPriceCommandHandlerTests
{
    private readonly Mock<ICanteenGradeItemPriceRepository> _repoMock = new();
    private readonly CreateGradeItemPriceCommandHandler _handler;

    public CreateGradeItemPriceCommandHandlerTests()
    {
        _handler = new CreateGradeItemPriceCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_CreatesAndReturnsDto()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CanteenGradeItemPrice>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var closureDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var command = new CreateGradeItemPriceCommand(1001, 5, 20m, 40m, null, closureDate, "admin", "A");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.CanteenUnitCode.Should().Be(1001);
        result.ItemCode.Should().Be(5);
        result.EmployeeContribution.Should().Be(20m);
        result.EmployerContribution.Should().Be(40m);
        result.ClosureDate.Should().Be(closureDate);
        result.GradeType.Should().Be("A");
    }

    [Fact]
    public async Task Handle_CallsAddAndSaveChanges()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CanteenGradeItemPrice>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new CreateGradeItemPriceCommand(1001, null, 20m, 40m, null, DateTime.UtcNow, "admin", "B");
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<CanteenGradeItemPrice>(), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class UpdateGradeItemPriceCommandHandlerTests
{
    private readonly Mock<ICanteenGradeItemPriceRepository> _repoMock = new();
    private readonly UpdateGradeItemPriceCommandHandler _handler;

    public UpdateGradeItemPriceCommandHandlerTests()
    {
        _handler = new UpdateGradeItemPriceCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenFound_UpdatesAndReturnsDto()
    {
        var existingEntity = CanteenGradeItemPrice.Create(
            1001, null, 20m, 40m, null,
            new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc), "admin", "A");

        _repoMock.Setup(r => r.GetByUnitCodeAsync(1001, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingEntity);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var newClosure = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var command = new UpdateGradeItemPriceCommand(1001, 30m, 60m, newClosure);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.EmployeeContribution.Should().Be(30m);
        result.EmployerContribution.Should().Be(60m);
        result.ClosureDate.Should().Be(newClosure);
    }

    [Fact]
    public async Task Handle_WhenNotFound_ThrowsDomainException()
    {
        _repoMock.Setup(r => r.GetByUnitCodeAsync(1001, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenGradeItemPrice?)null);

        var command = new UpdateGradeItemPriceCommand(1001, 30m, 60m, DateTime.UtcNow);

        await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                      .Should().ThrowAsync<DomainException>()
                      .WithMessage("*1001*");
    }

    [Fact]
    public async Task Handle_WhenFound_CallsUpdateAndSaveChanges()
    {
        var existingEntity = CanteenGradeItemPrice.Create(
            1001, null, 20m, 40m, null, DateTime.UtcNow.AddMonths(6), "admin", "A");
        _repoMock.Setup(r => r.GetByUnitCodeAsync(1001, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(existingEntity);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(1);

        var command = new UpdateGradeItemPriceCommand(1001, 30m, 60m, DateTime.UtcNow.AddMonths(12));
        await _handler.Handle(command, CancellationToken.None);

        _repoMock.Verify(r => r.Update(existingEntity), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
