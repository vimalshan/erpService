using FluentAssertions;
using ItemMasterService.API.Controllers;
using Xunit;
using ItemMasterService.Application.CQRS.Commands;
using ItemMasterService.Application.CQRS.Queries;
using ItemMasterService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ItemMasterService.Tests.API;

// ════════════════════════════════════════════════════════════════════════════
// CanteenItemMasterController Tests
// ════════════════════════════════════════════════════════════════════════════

public class CanteenItemMasterControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly CanteenItemMasterController _controller;

    private static readonly CanteenItemMasterDto SampleDto = new(
        CanteenUnitCode: 1001,
        ItemCode: 5,
        ItemDescription: "Rice Meal",
        ItemType: "F",
        ItemReference: "RICE01",
        EnteredOn: DateTime.UtcNow,
        EnteredBy: "admin");

    public CanteenItemMasterControllerTests()
    {
        _controller = new CanteenItemMasterController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithItems()
    {
        var items = new[] { SampleDto };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCanteenItemsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(items);

        var result = await _controller.GetAll(1001, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(items);
    }

    [Fact]
    public async Task GetById_WhenItemFound_ReturnsOkWithDto()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCanteenItemByIdQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(SampleDto);

        var result = await _controller.GetById(1001, 5, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(SampleDto);
    }

    [Fact]
    public async Task GetById_WhenItemNotFound_ReturnsNotFound()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCanteenItemByIdQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CanteenItemMasterDto?)null);

        var result = await _controller.GetById(1001, 99, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionWithDto()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCanteenItemCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(SampleDto);

        var command = new CreateCanteenItemCommand(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        var result = await _controller.Create(command, CancellationToken.None);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.ActionName.Should().Be(nameof(CanteenItemMasterController.GetById));
        created.Value.Should().BeEquivalentTo(SampleDto);
    }

    [Fact]
    public async Task Update_WithMatchingRouteAndBodyIds_ReturnsOkWithDto()
    {
        var updatedDto = SampleDto with { ItemDescription = "Updated Meal" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCanteenItemCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(updatedDto);

        var command = new UpdateCanteenItemCommand(1001, 5, "Updated Meal", "F", "RICE01");
        var result = await _controller.Update(1001, 5, command, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updatedDto);
    }

    [Fact]
    public async Task Update_WithMismatchedCanteenUnitCode_ReturnsBadRequest()
    {
        var command = new UpdateCanteenItemCommand(9999, 5, "Updated", "F", "REF");

        var result = await _controller.Update(1001, 5, command, CancellationToken.None);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Update_WithMismatchedItemCode_ReturnsBadRequest()
    {
        var command = new UpdateCanteenItemCommand(1001, 99, "Updated", "F", "REF");

        var result = await _controller.Update(1001, 5, command, CancellationToken.None);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCanteenItemCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var result = await _controller.Delete(1001, 5, CancellationToken.None);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetAll_PassesCorrectCanteenUnitCodeToMediator()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCanteenItemsQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Enumerable.Empty<CanteenItemMasterDto>());

        await _controller.GetAll(2002, CancellationToken.None);

        _mediatorMock.Verify(m => m.Send(
            It.Is<GetAllCanteenItemsQuery>(q => q.CanteenUnitCode == 2002),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

// ════════════════════════════════════════════════════════════════════════════
// CanteenItemPriceController Tests
// ════════════════════════════════════════════════════════════════════════════

public class CanteenItemPriceControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly CanteenItemPriceController _controller;

    private static readonly CanteenItemPriceMasterDto SamplePriceDto = new(
        CanteenUnitCode: 1001,
        ItemCode: 5,
        EmployeeContribution: 25m,
        EmployerContribution: 50m,
        EffectiveDate: new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        ClosureDate: null,
        EnteredOn: DateTime.UtcNow,
        EnteredBy: "admin");

    public CanteenItemPriceControllerTests()
    {
        _controller = new CanteenItemPriceController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetActivePrice_WhenFound_ReturnsOkWithDto()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetItemPriceQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(SamplePriceDto);

        var result = await _controller.GetActivePrice(1001, 5, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(SamplePriceDto);
    }

    [Fact]
    public async Task GetActivePrice_WhenNotFound_ReturnsNotFound()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetItemPriceQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CanteenItemPriceMasterDto?)null);

        var result = await _controller.GetActivePrice(1001, 99, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetPriceHistory_ReturnsOkWithHistory()
    {
        var history = new[] { SamplePriceDto };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetItemPriceHistoryQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(history);

        var result = await _controller.GetPriceHistory(1001, 5, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(history);
    }

    [Fact]
    public async Task CreatePrice_ReturnsCreatedAtActionWithDto()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateItemPriceCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(SamplePriceDto);

        var command = new CreateItemPriceCommand(
            1001, 5, 25m, 50m,
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "admin");
        var result = await _controller.CreatePrice(command, CancellationToken.None);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.ActionName.Should().Be(nameof(CanteenItemPriceController.GetActivePrice));
        created.Value.Should().BeEquivalentTo(SamplePriceDto);
    }

    [Fact]
    public async Task ClosePrice_ReturnsNoContent()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<CloseItemPriceCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var result = await _controller.ClosePrice(1001, 5, DateTime.UtcNow, CancellationToken.None);

        result.Should().BeOfType<NoContentResult>();
    }
}

// ════════════════════════════════════════════════════════════════════════════
// CanteenGradeItemPriceController Tests
// ════════════════════════════════════════════════════════════════════════════

public class CanteenGradeItemPriceControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly CanteenGradeItemPriceController _controller;

    private static readonly CanteenGradeItemPriceDto SampleGradeDto = new(
        CanteenUnitCode: 1001,
        ItemCode: 5,
        EmployeeContribution: 20m,
        EmployerContribution: 40m,
        EffectiveDate: null,
        ClosureDate: new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
        EnteredOn: DateTime.UtcNow,
        EnteredBy: "admin",
        GradeType: "A");

    public CanteenGradeItemPriceControllerTests()
    {
        _controller = new CanteenGradeItemPriceController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithGradePrices()
    {
        var items = new[] { SampleGradeDto };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllGradeItemPricesQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(items);

        var result = await _controller.GetAll(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(items);
    }

    [Fact]
    public async Task GetByUnit_WhenFound_ReturnsOkWithDto()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetGradeItemPriceQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(SampleGradeDto);

        var result = await _controller.GetByUnit(1001, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(SampleGradeDto);
    }

    [Fact]
    public async Task GetByUnit_WhenNotFound_ReturnsNotFound()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetGradeItemPriceQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((CanteenGradeItemPriceDto?)null);

        var result = await _controller.GetByUnit(9999, CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionWithDto()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateGradeItemPriceCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(SampleGradeDto);

        var command = new CreateGradeItemPriceCommand(
            1001, 5, 20m, 40m, null,
            new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), "admin", "A");
        var result = await _controller.Create(command, CancellationToken.None);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.ActionName.Should().Be(nameof(CanteenGradeItemPriceController.GetByUnit));
        created.Value.Should().BeEquivalentTo(SampleGradeDto);
    }

    [Fact]
    public async Task Update_WithMatchingCanteenUnitCode_ReturnsOkWithDto()
    {
        var updatedDto = SampleGradeDto with { EmployeeContribution = 30m };
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateGradeItemPriceCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(updatedDto);

        var command = new UpdateGradeItemPriceCommand(
            1001, 30m, 60m, new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc));
        var result = await _controller.Update(1001, command, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updatedDto);
    }

    [Fact]
    public async Task Update_WithMismatchedCanteenUnitCode_ReturnsBadRequest()
    {
        var command = new UpdateGradeItemPriceCommand(
            9999, 30m, 60m, new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc));

        var result = await _controller.Update(1001, command, CancellationToken.None);

        result.Should().BeOfType<BadRequestResult>();
    }
}
