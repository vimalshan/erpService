using TrainingDevelopment.Domain.Common;
using TrainingDevelopment.Domain.Events;

namespace TrainingDevelopment.Domain.Entities;

/// <summary>
/// Aggregate root for Training Detail — maps to TRAINING_DET table.
/// </summary>
public class TrainingDetail : AuditableEntity
{
    public decimal Id { get; private set; }                          // TR_ID
    public decimal FinancialYear { get; private set; }               // TR_FINYEAR
    public decimal EmployeeSysId { get; private set; }              // TR_EMPSYSID
    public string TrainingNeed { get; private set; } = default!;     // TR_NEED
    public string GapArea { get; private set; } = default!;          // TR_GAPS
    public decimal Mode { get; private set; }                        // TR_MODE
    public decimal ProgramId { get; private set; }                   // TR_PROGRAMID
    public string ProgramDescription { get; private set; } = default!; // TR_PROGRAMDESC
    public DateTime PlannedFrom { get; private set; }                // TR_PLANFROM
    public DateTime PlannedTo { get; private set; }                  // TR_PLANTO
    public string Status { get; private set; } = default!;           // TR_STATUS
    public DateTime? ActualFrom { get; private set; }                // TR_ACTFROM
    public DateTime? ActualTo { get; private set; }                  // TR_ACTTO
    public decimal? InstituteId { get; private set; }                // TR_INSTITUTEID
    public string? InstituteDescription { get; private set; }        // TR_INSTITUTEDESC
    public decimal? TrainerId { get; private set; }                  // TR_TRAINERID
    public string? TrainerDescription { get; private set; }          // TR_TRAINERDESC
    public decimal? PlaceId { get; private set; }                    // TR_PLACEID
    public string? Place { get; private set; }                       // TR_PLACE
    public decimal? Cost { get; private set; }                       // TR_COST
    public string? DroppedRemarks { get; private set; }              // TR_DROPREMARKS

    // EF Core constructor
    private TrainingDetail() { }

    public static TrainingDetail Create(
        decimal id,
        decimal financialYear,
        decimal employeeSysId,
        string trainingNeed,
        string gapArea,
        decimal mode,
        decimal programId,
        string programDescription,
        DateTime plannedFrom,
        DateTime plannedTo,
        decimal? lastModifiedBy = null)
    {
        var detail = new TrainingDetail
        {
            Id = id,
            FinancialYear = financialYear,
            EmployeeSysId = employeeSysId,
            TrainingNeed = trainingNeed,
            GapArea = gapArea,
            Mode = mode,
            ProgramId = programId,
            ProgramDescription = programDescription,
            PlannedFrom = plannedFrom,
            PlannedTo = plannedTo,
            Status = "P",
            LastModifiedBy = lastModifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        detail.AddDomainEvent(new TrainingCreatedEvent(detail));
        return detail;
    }

    public void MarkCompleted(DateTime actualFrom, DateTime actualTo,
        decimal? instituteId, string? instituteDesc,
        decimal? trainerId, string? trainerDesc,
        decimal? placeId, string? place, decimal? cost)
    {
        Status = "C";
        ActualFrom = actualFrom;
        ActualTo = actualTo;
        InstituteId = instituteId;
        InstituteDescription = instituteDesc;
        TrainerId = trainerId;
        TrainerDescription = trainerDesc;
        PlaceId = placeId;
        Place = place;
        Cost = cost;
        LastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new TrainingCompletedEvent(this));
    }

    public void Drop(string remarks)
    {
        Status = "D";
        DroppedRemarks = remarks;
        LastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new TrainingDroppedEvent(this));
    }

    public void Update(
        string trainingNeed,
        string gapArea,
        decimal mode,
        decimal programId,
        string programDescription,
        DateTime plannedFrom,
        DateTime plannedTo,
        decimal? lastModifiedBy)
    {
        TrainingNeed = trainingNeed;
        GapArea = gapArea;
        Mode = mode;
        ProgramId = programId;
        ProgramDescription = programDescription;
        PlannedFrom = plannedFrom;
        PlannedTo = plannedTo;
        LastModifiedBy = lastModifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
