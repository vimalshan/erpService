using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using VendorService.Application.DTOs;
using VendorService.Application.Mappings;
using VendorService.Application.Queries;
using VendorService.Domain.Entities;
using VendorService.Domain.Interfaces;

namespace VendorService.UnitTests.Queries;

public sealed class GetVendorByIdQueryHandlerTests
{
    private readonly Mock<IVendorRepository> _repositoryMock = new();
    private readonly IMapper _mapper;

    public GetVendorByIdQueryHandlerTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(cfg => cfg.AddProfile<VendorMappingProfile>());
        var sp = services.BuildServiceProvider();
        _mapper = sp.GetRequiredService<IMapper>();
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsVendorDto()
    {
        // Arrange
        var vendor = VendorMaster.Create(1, 2, 3, "Acme Corp", "acme@example.com", "100 Main St", 1);
        _repositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vendor);

        var handler = new GetVendorByIdQueryHandler(_repositoryMock.Object, _mapper);

        // Act
        var result = await handler.Handle(new GetVendorByIdQuery(1), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Acme Corp");
        result.CategoryId.Should().Be(2);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VendorMaster?)null);

        var handler = new GetVendorByIdQueryHandler(_repositoryMock.Object, _mapper);

        // Act
        var result = await handler.Handle(new GetVendorByIdQuery(99), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
