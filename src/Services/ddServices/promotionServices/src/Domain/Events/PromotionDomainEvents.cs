namespace PromotionService.Domain.Events;

// ── Base ────────────────────────────────────────────────────────
public abstract record DomainEvent(DateTime OccurredOn) { }

// ── Rating Events ───────────────────────────────────────────────
public record RatingCreatedEvent(
    long RatingId,
    long EmployeeSystemId,
    int DDYear,
    decimal FinalRating,
    string RatingGrade,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record RatingFinalizedEvent(
    long RatingId,
    long EmployeeSystemId,
    int DDYear,
    string ApprovedBySystemId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record RatingDeletedEvent(
    long RatingId,
    long EmployeeSystemId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

// ── Promotion Events ────────────────────────────────────────────
public record PromotionRecommendationCreatedEvent(
    long PromotionId,
    long EmployeeSystemId,
    string CurrentGrade,
    string ProposedGrade,
    DateTime EffectiveDate,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record PromotionApprovedEvent(
    long PromotionId,
    long EmployeeSystemId,
    string ProposedGrade,
    decimal ProposedSalaryIncrease,
    string ApprovedBySystemId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record PromotionRejectedEvent(
    long PromotionId,
    long EmployeeSystemId,
    string Reason,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record PromotionHeldEvent(
    long PromotionId,
    long EmployeeSystemId,
    string Reason,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

// ── Increment Events ────────────────────────────────────────────
public record IncrementRequestCreatedEvent(
    long IncrementId,
    long EmployeeSystemId,
    string IncrementType,
    decimal Amount,
    decimal Percentage,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record IncrementApprovedEvent(
    long IncrementId,
    long EmployeeSystemId,
    decimal ApprovedAmount,
    string ApprovedBySystemId,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record IncrementRejectedEvent(
    long IncrementId,
    long EmployeeSystemId,
    string Reason,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

// ── VTC Correction Events ───────────────────────────────────────
public record VTCCorrectionSubmittedEvent(
    decimal RateId,
    decimal EmployeeSystemId,
    string OldRating,
    string NewRating,
    string Reason,
    DateTime OccurredOn) : DomainEvent(OccurredOn);

public record VTCCorrectionApprovedEvent(
    decimal RateId,
    decimal EmployeeSystemId,
    decimal ApprovedBy,
    DateTime OccurredOn) : DomainEvent(OccurredOn);
