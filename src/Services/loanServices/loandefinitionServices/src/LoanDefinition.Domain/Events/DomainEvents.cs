using MediatR;

namespace LoanDefinition.Domain.Events;

public record LoanTypeCreatedEvent(long LoanTypeId, string LoanName) : INotification;
public record LoanTypeUpdatedEvent(long LoanTypeId, string LoanName) : INotification;
public record LoanMasterCreatedEvent(long LoanId, string LoanName) : INotification;
public record LoanMasterUpdatedEvent(long LoanId, string LoanName) : INotification;
public record LoanMasterClosedEvent(long LoanId, DateTime ClosureDate) : INotification;
public record InterestRateChangedEvent(long LoanId, long RateId, int NewRate) : INotification;
public record FestivalCreatedEvent(long FestivalId, string Description) : INotification;
