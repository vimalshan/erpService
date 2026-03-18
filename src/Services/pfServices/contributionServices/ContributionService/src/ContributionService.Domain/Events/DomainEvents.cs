using MediatR;

namespace ContributionService.Domain.Events;

public record ContributionBatchCreatedEvent(long BatchNo, string TrustCode, string PayunitCode) : INotification;
public record ContributionBatchPostedEvent(long BatchNo, long PostedByUserId) : INotification;
public record ContributionStatusChangedEvent(long BatchNo, string NewStatus) : INotification;
public record ContributionDetailValidatedEvent(decimal ContributionId) : INotification;
public record SuperannuationBatchCreatedEvent(long BatchNo) : INotification;
public record MonthlyContributionProcessedEvent(string MonthYear, int RowsProcessed) : INotification;
