using FluentAssertions;
using Moq;
using TdsService.Application.DTOs;
using TdsService.Application.Vendors.Queries.GetAllTdsVendors;
using TdsService.Domain.Entities;
using TdsService.Domain.Repositories;
using Xunit;

namespace TdsService.Application.Tests.Vendors;

public sealed class GetAllTdsVendorsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPagedVendors()
    {
        // Arrange
        var vendors = new List<TdsVendor>
        {
            TdsVendor.Create(1, "Vendor One", "v1@example.com", "ABCDE1234F"),
            TdsVendor.Create(2, "Vendor Two", "v2@example.com", "BCDEF2345G"),
        };

        foreach (var v in vendors) v.ClearDomainEvents();

        var repoMock = new Mock<ITdsVendorRepository>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(vendors.AsReadOnly());

        var handler = new GetAllTdsVendorsQueryHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetAllTdsVendorsQuery(1, 10), CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.First().VendorName.Should().Be("Vendor One");
    }

    [Fact]
    public async Task Handle_ShouldPaginateCorrectly()
    {
        var vendors = Enumerable.Range(1, 25)
            .Select(i => TdsVendor.Create(i, $"Vendor {i}", null, null))
            .ToList();

        foreach (var v in vendors) v.ClearDomainEvents();

        var repoMock = new Mock<ITdsVendorRepository>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(vendors.AsReadOnly());

        var handler = new GetAllTdsVendorsQueryHandler(repoMock.Object);

        var result = await handler.Handle(new GetAllTdsVendorsQuery(2, 10), CancellationToken.None);

        result.Items.Should().HaveCount(10);
        result.TotalCount.Should().Be(25);
        result.Page.Should().Be(2);
        result.TotalPages.Should().Be(3);
    }
}
