using TourServices.Domain.Common;
using TourServices.Domain.Events;
using TourServices.Domain.Exceptions;
using TourServices.Domain.ValueObjects;

namespace TourServices.Domain.Entities;

public sealed class TourRegistration : AuditableEntity
{
    public long RegistrationId { get; private set; }
    public long TourId { get; private set; }
    public long ParticipantId { get; private set; }
    public DateOnly RegistrationDate { get; private set; }
    public RegistrationStatus RegistrationStatus { get; private set; } = RegistrationStatus.Active;

    private TourRegistration() { }

    internal static TourRegistration Create(
        long tourId,
        long participantId,
        DateOnly registrationDate,
        long registeredBy)
    {
        var reg = new TourRegistration
        {
            TourId = tourId,
            ParticipantId = participantId,
            RegistrationDate = registrationDate,
            RegistrationStatus = RegistrationStatus.Active,
            CreatedBy = registeredBy,
            CreatedOn = DateTime.UtcNow
        };

        reg.AddDomainEvent(new ParticipantRegisteredEvent(
            Guid.NewGuid(), DateTime.UtcNow,
            0, tourId, participantId, registrationDate));

        return reg;
    }

    internal void Cancel(long cancelledBy)
    {
        if (RegistrationStatus == RegistrationStatus.Cancelled)
            return;

        var oldStatus = RegistrationStatus;
        RegistrationStatus = RegistrationStatus.Cancelled;
        ModifiedBy = cancelledBy;
        ModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new RegistrationCancelledEvent(
            Guid.NewGuid(), DateTime.UtcNow,
            RegistrationId, TourId, ParticipantId, cancelledBy));
    }

    internal void MoveToWaitlist(long updatedBy)
    {
        RegistrationStatus = RegistrationStatus.Waitlist;
        ModifiedBy = updatedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
