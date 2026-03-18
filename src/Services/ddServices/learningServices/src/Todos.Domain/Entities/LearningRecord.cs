using Todos.Domain.Abstractions;
using Todos.Domain.ValueObjects;

namespace Todos.Domain.Entities;

/// <summary>
/// Represents a learning and training (LET) record aggregate root
/// </summary>
public class LearningRecord : AggregateRoot
{
    /// <summary>
    /// Gets the learning record ID (from LET_ID)
    /// </summary>
    public decimal LetId { get; private set; }

    /// <summary>
    /// Gets the DD request number (from LET_DD_REQNO)
    /// </summary>
    public RequestNumber RequestNumber { get; private set; } = null!;

    /// <summary>
    /// Gets the employee ID who applied for learning
    /// </summary>
    public EmployeeId? EmployeeId { get; private set; }

    /// <summary>
    /// Gets the development source identifier
    /// </summary>
    public decimal? DevelopmentSourceId { get; private set; }

    /// <summary>
    /// Gets the specific learning need
    /// </summary>
    public string? SpecificNeed { get; private set; }

    /// <summary>
    /// Gets the performance indicator
    /// </summary>
    public string? Indicator { get; private set; }

    /// <summary>
    /// Gets the area of development
    /// </summary>
    public string? DevelopmentArea { get; private set; }

    /// <summary>
    /// Gets the post-training expected outcomes
    /// </summary>
    public string? ExpectedPostTraining { get; private set; }

    /// <summary>
    /// Gets the BHR approval status
    /// </summary>
    public BHRStatus? BhrStatus { get; private set; }

    /// <summary>
    /// Gets the reviewer comments
    /// </summary>
    public string? ReviewerComments { get; private set; }

    /// <summary>
    /// Gets the appraisee opinion
    /// </summary>
    public string? AppraiseeOpinion { get; private set; }

    /// <summary>
    /// Gets the appraiser comments
    /// </summary>
    public string? AppraiserComments { get; private set; }

    /// <summary>
    /// Gets the user who last modified this record
    /// </summary>
    public decimal ModifiedBy { get; private set; }

    /// <summary>
    /// Gets the sub-learning records (training modules)
    /// </summary>
    private readonly List<LearningSubRecord> _subRecords = [];
    public IReadOnlyList<LearningSubRecord> SubRecords => _subRecords.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the LearningRecord class
    /// </summary>
    protected LearningRecord() { }

    /// <summary>
    /// Creates a new learning record
    /// </summary>
    public static LearningRecord Create(
        decimal letId,
        RequestNumber requestNumber,
        EmployeeId? employeeId,
        string? specificNeed,
        decimal modifiedBy)
    {
        var record = new LearningRecord
        {
            LetId = letId,
            RequestNumber = requestNumber,
            EmployeeId = employeeId,
            SpecificNeed = specificNeed,
            ModifiedBy = modifiedBy,
            CreatedAt = DateTime.UtcNow,
            Version = 1
        };

        record.RaiseDomainEvent(new Events.LearningCreatedEvent
        {
            AggregateId = record.Id,
            RequestNumber = requestNumber.Value,
            EmployeeId = employeeId?.Value,
            SpecificNeed = specificNeed,
            CreatedAt = record.CreatedAt
        });

        return record;
    }

    /// <summary>
    /// Updates the learning record
    /// </summary>
    public void Update(
        string? specificNeed,
        string? indicator,
        string? developmentArea,
        string? expectedPostTraining,
        BHRStatus? bhrStatus,
        decimal modifiedBy)
    {
        SpecificNeed = specificNeed;
        Indicator = indicator;
        DevelopmentArea = developmentArea;
        ExpectedPostTraining = expectedPostTraining;
        BhrStatus = bhrStatus;
        ModifiedBy = modifiedBy;
        UpdatedAt = DateTime.UtcNow;
        Version++;

        RaiseDomainEvent(new Events.LearningUpdatedEvent
        {
            AggregateId = Id,
            RequestNumber = RequestNumber.Value,
            EmployeeId = EmployeeId?.Value,
            UpdatedAt = UpdatedAt.Value
        });
    }

    /// <summary>
    /// Identifies a learning need
    /// </summary>
    public void IdentifyLearningNeed(string developmentArea, string indicator)
    {
        DevelopmentArea = developmentArea;
        Indicator = indicator;
        UpdatedAt = DateTime.UtcNow;
        Version++;

        RaiseDomainEvent(new Events.LearningNeedIdentifiedEvent
        {
            AggregateId = Id,
            RequestNumber = RequestNumber.Value,
            DevelopmentArea = developmentArea,
            Indicator = indicator,
            IdentifiedAt = UpdatedAt.Value
        });
    }

    /// <summary>
    /// Adds a sub-learning record (training module)
    /// </summary>
    public void AddSubRecord(LearningSubRecord subRecord)
    {
        if (!_subRecords.Any(x => x.Id == subRecord.Id))
        {
            _subRecords.Add(subRecord);
            Version++;
        }
    }

    /// <summary>
    /// Removes a sub-learning record
    /// </summary>
    public void RemoveSubRecord(Guid subRecordId)
    {
        var subRecord = _subRecords.FirstOrDefault(x => x.Id == subRecordId);
        if (subRecord != null)
        {
            _subRecords.Remove(subRecord);
            Version++;
        }
    }
}
