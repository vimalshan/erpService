using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Exceptions;
using BatchAndEnvelopeService.Domain.Events;
using FluentAssertions;

namespace BatchAndEnvelopeService.Tests.Domain;

public class BatchAggregateTests
{
    [Fact]
    public void Create_ShouldSetProperties_AndRaiseBatchCreatedEvent()
    {
        var batch = BatchAggregate.Create(1001, 1, 101, 2, "POD-001", "FastCourier");

        batch.Id.Should().Be(1001);
        batch.CreatedBy.Should().Be(1);
        batch.LocationId.Should().Be(101);
        batch.ReceivedBy.Should().Be(2);
        batch.PodNo.Should().Be("POD-001");
        batch.CourierName.Should().Be("FastCourier");
        batch.SummaryFlag.Should().Be("N");
        batch.DomainEvents.Should().ContainSingle(e => e is BatchCreatedDomainEvent);
    }

    [Fact]
    public void Confirm_ShouldSetSummaryFlag_AndRaiseBatchConfirmedEvent()
    {
        var batch = BatchAggregate.Create(1001, 1, 101, 2, "POD-001");

        batch.Confirm(99);

        batch.ConfirmedBy.Should().Be(99);
        batch.SummaryFlag.Should().Be("Y");
        batch.DomainEvents.Should().Contain(e => e is BatchConfirmedDomainEvent);
    }

    [Fact]
    public void Confirm_WhenAlreadyCancelled_ShouldThrowBatchDomainException()
    {
        var batch = BatchAggregate.Create(1001, 1, 101, 2, "POD-001");
        batch.Cancel(5);

        var act = () => batch.Confirm(99);

        act.Should().Throw<BatchDomainException>();
    }

    [Fact]
    public void Cancel_ShouldSetCancelFields_AndRaiseBatchCancelledEvent()
    {
        var batch = BatchAggregate.Create(1001, 1, 101, 2, "POD-001");

        batch.Cancel(7);

        batch.CancelBy.Should().Be(7);
        batch.CancelDate.Should().NotBeNull();
        batch.DomainEvents.Should().Contain(e => e is BatchCancelledDomainEvent);
    }

    [Fact]
    public void Cancel_WhenAlreadyConfirmed_ShouldThrowBatchDomainException()
    {
        var batch = BatchAggregate.Create(1001, 1, 101, 2, "POD-001");
        batch.Confirm(9);

        var act = () => batch.Cancel(7);

        act.Should().Throw<BatchDomainException>();
    }
}
