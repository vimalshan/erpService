using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Exceptions;
using BatchAndEnvelopeService.Domain.Events;
using FluentAssertions;

namespace BatchAndEnvelopeService.Tests.Domain;

public class EnvelopeAggregateTests
{
    [Fact]
    public void Create_ShouldSetProperties_AndRaiseEnvelopeCreatedEvent()
    {
        var envelope = EnvelopeAggregate.Create(200, "REG", 1, 101);

        envelope.Id.Should().Be(200);
        envelope.EnvelopeType.Should().Be("REG");
        envelope.CreatedBy.Should().Be(1);
        envelope.LocationId.Should().Be(101);
        envelope.SummaryFlag.Should().Be("N");
        envelope.DomainEvents.Should().ContainSingle(e => e is EnvelopeCreatedDomainEvent);
    }

    [Fact]
    public void Confirm_ShouldSetSummaryFlag_AndRaiseEnvelopeConfirmedEvent()
    {
        var envelope = EnvelopeAggregate.Create(200, "REG", 1, 101);

        envelope.Confirm(88);

        envelope.ConfirmedBy.Should().Be(88);
        envelope.SummaryFlag.Should().Be("Y");
        envelope.DomainEvents.Should().Contain(e => e is EnvelopeConfirmedDomainEvent);
    }

    [Fact]
    public void Cancel_WhenAlreadyConfirmed_ShouldThrowEnvelopeDomainException()
    {
        var envelope = EnvelopeAggregate.Create(200, "REG", 1, 101);
        envelope.Confirm(88);

        var act = () => envelope.Cancel(5);

        act.Should().Throw<EnvelopeDomainException>();
    }
}
