using FluentAssertions;
using Moq;
using MassTransit;
using Stationery.Application.Features.Requests.Commands;
using Stationery.Domain.Entities;
using Stationery.Domain.Events;
using Stationery.Domain.Interfaces;
using Xunit;

namespace Stationery.UnitTests.Application.Requests;

public class ApproveRequestCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<RequestSub>> _repositoryMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;
    private readonly ApproveRequestCommandHandler _handler;

    public ApproveRequestCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<IRepository<RequestSub>>();
        _publishEndpointMock = new Mock<IPublishEndpoint>();

        _unitOfWorkMock.Setup(u => u.Repository<RequestSub>()).Returns(_repositoryMock.Object);
        _handler = new ApproveRequestCommandHandler(_unitOfWorkMock.Object, _publishEndpointMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldApprove_WhenRequestSubIsPending()
    {
        var requestSub = new RequestSub
        {
            Id = 1,
            RequestId = 10,
            StationaryId = 1,
            DeptId = 100,
            Status = "P",
            RequestedQty = 5,
            ExpectedDate = DateTime.UtcNow.AddDays(7),
            UpdatedBy = 1,
            UpdatedOn = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(requestSub);

        var command = new ApproveRequestCommand(RequestSubId: 1, ApprovedQty: 3, ApproverSysId: 201);

        await _handler.Handle(command, CancellationToken.None);

        requestSub.Status.Should().Be("A");
        requestSub.ApprovedQty.Should().Be(3);
        requestSub.ApproverSysId.Should().Be(201);
        _repositoryMock.Verify(r => r.Update(requestSub), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        _publishEndpointMock.Verify(p => p.Publish(It.IsAny<RequestApprovedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRequestSubNotFound()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((RequestSub?)null);

        var command = new ApproveRequestCommand(RequestSubId: 999, ApprovedQty: 3, ApproverSysId: 201);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRequestSubIsAlreadyApproved()
    {
        var requestSub = new RequestSub { Id = 1, Status = "A", UpdatedBy = 1, UpdatedOn = DateTime.UtcNow };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(requestSub);

        var command = new ApproveRequestCommand(RequestSubId: 1, ApprovedQty: 3, ApproverSysId: 201);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
