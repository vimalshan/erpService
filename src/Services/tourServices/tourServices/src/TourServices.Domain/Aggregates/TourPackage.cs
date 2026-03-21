using TourServices.Domain.Common;
using TourServices.Domain.Entities;
using TourServices.Domain.Events;
using TourServices.Domain.Exceptions;
using TourServices.Domain.ValueObjects;

namespace TourServices.Domain.Aggregates;

/// <summary>
/// TourPackage is the aggregate root for the Tour bounded context.
/// </summary>
public sealed class TourPackage : AuditableEntity
{
    private readonly List<TourRegistration> _registrations = new();

    public long TourId { get; private set; }
    public string TourName { get; private set; } = string.Empty;
    public string Destination { get; private set; } = string.Empty;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public Money TourPackageCost { get; private set; } = Money.Zero;
    public int MaxParticipants { get; private set; }
    public TourStatus TourStatus { get; private set; } = TourStatus.Planning;
    public IReadOnlyCollection<TourRegistration> Registrations => _registrations.AsReadOnly();

    private TourPackage() { }

    public static TourPackage Plan(
        string tourName,
        string destination,
        DateOnly startDate,
        DateOnly endDate,
        decimal totalCost,
        int maxParticipants,
        long plannerId)
    {
        if (string.IsNullOrWhiteSpace(tourName))
            throw new ArgumentException("Tour name is required.", nameof(tourName));
        if (string.IsNullOrWhiteSpace(destination))
            throw new ArgumentException("Destination is required.", nameof(destination));
        if (startDate >= endDate)
            throw new ArgumentException("Start date must be before end date.");
        if (maxParticipants <= 0)
            throw new ArgumentException("Max participants must be positive.", nameof(maxParticipants));

        var tour = new TourPackage
        {
            TourName = tourName,
            Destination = destination,
            StartDate = startDate,
            EndDate = endDate,
            TourPackageCost = new Money(totalCost),
            MaxParticipants = maxParticipants,
            TourStatus = TourStatus.Planning,
            CreatedBy = plannerId,
            CreatedOn = DateTime.UtcNow
        };

        tour.AddDomainEvent(new TourPackagePlannedEvent(
            Guid.NewGuid(), DateTime.UtcNow,
            0, tourName, destination, startDate, endDate, totalCost, maxParticipants, plannerId));

        return tour;
    }

    public void Activate(long updatedBy) => ChangeStatus(TourStatus.Active, updatedBy);
    public void Complete(long updatedBy) => ChangeStatus(TourStatus.Completed, updatedBy);
    public void Cancel(long updatedBy)
    {
        foreach (var reg in _registrations.Where(r => r.RegistrationStatus == RegistrationStatus.Active))
            reg.Cancel(updatedBy);
        ChangeStatus(TourStatus.Cancelled, updatedBy);
    }

    public TourRegistration RegisterParticipant(long participantId, DateOnly registrationDate, long registeredBy)
    {
        if (TourStatus != TourStatus.Active)
            throw new TourNotActiveException(TourId, TourStatus.Code);

        var activeCount = _registrations.Count(r => r.RegistrationStatus == RegistrationStatus.Active);
        if (activeCount >= MaxParticipants)
            throw new TourFullyBookedException(TourId);

        var registration = TourRegistration.Create(TourId, participantId, registrationDate, registeredBy);
        _registrations.Add(registration);
        return registration;
    }

    public void CancelRegistration(long registrationId, long cancelledBy)
    {
        var reg = _registrations.FirstOrDefault(r => r.RegistrationId == registrationId)
            ?? throw new InvalidOperationException($"Registration {registrationId} not found on tour {TourId}.");
        reg.Cancel(cancelledBy);
    }

    public Money CalculateCostPerPerson()
    {
        var activeCount = _registrations.Count(r => r.RegistrationStatus == RegistrationStatus.Active);
        return TourPackageCost.Divide(activeCount > 0 ? activeCount : 1);
    }

    public void Update(string tourName, string destination, DateOnly startDate, DateOnly endDate,
        decimal totalCost, int maxParticipants, long updatedBy)
    {
        TourName = tourName;
        Destination = destination;
        StartDate = startDate;
        EndDate = endDate;
        TourPackageCost = new Money(totalCost);
        MaxParticipants = maxParticipants;
        ModifiedBy = updatedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    private void ChangeStatus(TourStatus newStatus, long updatedBy)
    {
        var oldStatus = TourStatus.Code;
        TourStatus = newStatus;
        ModifiedBy = updatedBy;
        ModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new TourPackageStatusChangedEvent(
            Guid.NewGuid(), DateTime.UtcNow, TourId, oldStatus, newStatus.Code, updatedBy));
    }
}
