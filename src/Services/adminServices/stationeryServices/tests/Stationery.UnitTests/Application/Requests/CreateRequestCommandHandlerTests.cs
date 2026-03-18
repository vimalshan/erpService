using FluentAssertions;
using Moq;
using MassTransit;
using Stationery.Application.Features.Requests.Commands;
using Stationery.Domain.Entities;
using Stationery.Domain.Interfaces;
using Stationery.Domain.Events;
using Xunit;

namespace Stationery.UnitTests.Application.Requests;

public class CreateRequestCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<RequestMain>> _repositoryMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly CreateRequestCommandHandler _handler;

    public CreateRequestCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<IRepository<RequestMain>>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _unitOfWorkMock.Setup(u => u.Repository<RequestMain>()).Returns(_repositoryMock.Object);

        _handler = new CreateRequestCommandHandler(_unitOfWorkMock.Object, _publishEndpointMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateRequest_And_PublishEvent()
    {
        // Arrange
        var command = new CreateRequestCommand(
            RequestedBy: 1,
            LocationId: 1,
            UnitCode: "ABC",
            Details: new List<RequestDetailDto>
            {
                new(StationaryId: 101, DeptId: 10, ExpectedDate: DateTime.UtcNow.AddDays(7), RequestedQty: 5)
            }
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<RequestMain>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<RequestCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().BeGreaterThanOrEqualTo(0);
    }
}
