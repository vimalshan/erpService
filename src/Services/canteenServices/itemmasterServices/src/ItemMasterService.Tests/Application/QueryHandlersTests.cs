using FluentAssertions;
using ItemMasterService.Application.CQRS.Handlers;
using Xunit;
using ItemMasterService.Application.CQRS.Queries;
using ItemMasterService.Domain.Entities;
using ItemMasterService.Domain.Interfaces;
using Moq;

namespace ItemMasterService.Tests.Application;

// ════════════════════════════════════════════════════════════════════════════
// CanteenItem Query Handlers
// ════════════════════════════════════════════════════════════════════════════

public class GetCanteenItemByIdQueryHandlerTests
{
    private readonly Mock<ICanteenItemRepository> _repoMock = new();
    private readonly GetCanteenItemByIdQueryHandler _handler;

    public GetCanteenItemByIdQueryHandlerTests()
    {
        _handler = new GetCanteenItemByIdQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenItemFound_ReturnsDto()
    {
        var item = CanteenItemMaster.Create(1001, 5, "Rice Meal", "F", "RICE01", "admin");
        _repoMock.Setup(r => r.GetByIdAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(item);

        var query = new GetCanteenItemByIdQuery(1001, 5);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.CanteenUnitCode.Should().Be(1001);
        result.ItemCode.Should().Be(5);
        result.ItemDescription.Should().Be("Rice Meal");
        result.ItemType.Should().Be("F");
        result.ItemReference.Should().Be("RICE01");
        result.EnteredBy.Should().Be("admin");
    }

    [Fact]
    public async Task Handle_WhenItemNotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1001, 99, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenItemMaster?)null);

        var query = new GetCanteenItemByIdQuery(1001, 99);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}

public class GetAllCanteenItemsQueryHandlerTests
{
    private readonly Mock<ICanteenItemRepository> _repoMock = new();
    private readonly GetAllCanteenItemsQueryHandler _handler;

    public GetAllCanteenItemsQueryHandlerTests()
    {
        _handler = new GetAllCanteenItemsQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenItemsExist_ReturnsMappedDtos()
    {
        var items = new[]
        {
            CanteenItemMaster.Create(1001, 1, "Rice Meal", "F", "RICE01", "admin"),
            CanteenItemMaster.Create(1001, 2, "Chicken Curry", "F", "CHKN01", "admin"),
        };
        _repoMock.Setup(r => r.GetAllAsync(1001, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(items);

        var query = new GetAllCanteenItemsQuery(1001);
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        result.Should().HaveCount(2);
        result[0].ItemCode.Should().Be(1);
        result[1].ItemCode.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenNoItems_ReturnsEmptyCollection()
    {
        _repoMock.Setup(r => r.GetAllAsync(1001, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Enumerable.Empty<CanteenItemMaster>());

        var query = new GetAllCanteenItemsQuery(1001);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}

// ════════════════════════════════════════════════════════════════════════════
// ItemPrice Query Handlers
// ════════════════════════════════════════════════════════════════════════════

public class GetItemPriceQueryHandlerTests
{
    private readonly Mock<ICanteenItemPriceRepository> _repoMock = new();
    private readonly GetItemPriceQueryHandler _handler;

    public GetItemPriceQueryHandlerTests()
    {
        _handler = new GetItemPriceQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenActivePriceFound_ReturnsDto()
    {
        var effectiveDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var priceEntity = CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, effectiveDate, "admin");
        _repoMock.Setup(r => r.GetActiveAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(priceEntity);

        var query = new GetItemPriceQuery(1001, 5);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.CanteenUnitCode.Should().Be(1001);
        result.ItemCode.Should().Be(5);
        result.EmployeeContribution.Should().Be(25m);
        result.EmployerContribution.Should().Be(50m);
        result.EffectiveDate.Should().Be(effectiveDate);
    }

    [Fact]
    public async Task Handle_WhenNoPriceFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetActiveAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenItemPriceMaster?)null);

        var query = new GetItemPriceQuery(1001, 5);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}

public class GetItemPriceHistoryQueryHandlerTests
{
    private readonly Mock<ICanteenItemPriceRepository> _repoMock = new();
    private readonly GetItemPriceHistoryQueryHandler _handler;

    public GetItemPriceHistoryQueryHandlerTests()
    {
        _handler = new GetItemPriceHistoryQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMappedPriceHistoryDtos()
    {
        var prices = new[]
        {
            CanteenItemPriceMaster.Create(1001, 5, 20m, 40m, new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), "admin"),
            CanteenItemPriceMaster.Create(1001, 5, 25m, 50m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), "admin"),
        };
        _repoMock.Setup(r => r.GetHistoryAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(prices);

        var query = new GetItemPriceHistoryQuery(1001, 5);
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        result.Should().HaveCount(2);
        result[0].EmployeeContribution.Should().Be(20m);
        result[1].EmployeeContribution.Should().Be(25m);
    }

    [Fact]
    public async Task Handle_WhenNoHistory_ReturnsEmptyCollection()
    {
        _repoMock.Setup(r => r.GetHistoryAsync(1001, 5, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Enumerable.Empty<CanteenItemPriceMaster>());

        var query = new GetItemPriceHistoryQuery(1001, 5);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEmpty();
    }
}

// ════════════════════════════════════════════════════════════════════════════
// GradeItemPrice Query Handlers
// ════════════════════════════════════════════════════════════════════════════

public class GetGradeItemPriceQueryHandlerTests
{
    private readonly Mock<ICanteenGradeItemPriceRepository> _repoMock = new();
    private readonly GetGradeItemPriceQueryHandler _handler;

    public GetGradeItemPriceQueryHandlerTests()
    {
        _handler = new GetGradeItemPriceQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenFound_ReturnsDto()
    {
        var entity = CanteenGradeItemPrice.Create(
            1001, 5, 20m, 40m, null,
            new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc), "admin", "A");
        _repoMock.Setup(r => r.GetByUnitCodeAsync(1001, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(entity);

        var query = new GetGradeItemPriceQuery(1001);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result!.CanteenUnitCode.Should().Be(1001);
        result.EmployeeContribution.Should().Be(20m);
        result.GradeType.Should().Be("A");
    }

    [Fact]
    public async Task Handle_WhenNotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByUnitCodeAsync(9999, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((CanteenGradeItemPrice?)null);

        var query = new GetGradeItemPriceQuery(9999);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}

public class GetAllGradeItemPricesQueryHandlerTests
{
    private readonly Mock<ICanteenGradeItemPriceRepository> _repoMock = new();
    private readonly GetAllGradeItemPricesQueryHandler _handler;

    public GetAllGradeItemPricesQueryHandlerTests()
    {
        _handler = new GetAllGradeItemPricesQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMappedDtos()
    {
        var entities = new[]
        {
            CanteenGradeItemPrice.Create(1001, null, 20m, 40m, null, DateTime.UtcNow.AddMonths(6), "admin", "A"),
            CanteenGradeItemPrice.Create(1002, null, 25m, 50m, null, DateTime.UtcNow.AddMonths(6), "admin", "B"),
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(entities);

        var result = (await _handler.Handle(new GetAllGradeItemPricesQuery(), CancellationToken.None)).ToList();

        result.Should().HaveCount(2);
        result[0].CanteenUnitCode.Should().Be(1001);
        result[1].CanteenUnitCode.Should().Be(1002);
    }

    [Fact]
    public async Task Handle_WhenNoGradePrices_ReturnsEmptyCollection()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Enumerable.Empty<CanteenGradeItemPrice>());

        var result = await _handler.Handle(new GetAllGradeItemPricesQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }
}
