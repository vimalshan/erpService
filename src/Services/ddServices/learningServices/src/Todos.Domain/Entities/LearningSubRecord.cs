using Todos.Domain.Abstractions;
using Todos.Domain.ValueObjects;

namespace Todos.Domain.Entities;

/// <summary>
/// Represents a sub-learning record (training module detail)
/// </summary>
public class LearningSubRecord : Entity
{
    /// <summary>
    /// Gets the sub-record ID (from LET_MODID)
    /// </summary>
    public decimal SubId { get; private set; }

    /// <summary>
    /// Gets the main learning record ID
    /// </summary>
    public Guid LearningRecordId { get; private set; }

    /// <summary>
    /// Gets the DD request number
    /// </summary>
    public RequestNumber RequestNumber { get; private set; } = null!;

    /// <summary>
    /// Gets the development mode identifier
    /// </summary>
    public decimal DevelopmentModeId { get; private set; }

    /// <summary>
    /// Gets the training identifier
    /// </summary>
    public TrainingId TrainingId { get; private set; } = null!;

    /// <summary>
    /// Gets the training detail/description
    /// </summary>
    public string? TrainingDetail { get; private set; }

    /// <summary>
    /// Gets remarks about the training
    /// </summary>
    public string? Remarks { get; private set; }

    /// <summary>
    /// Gets the learning development identifier
    /// </summary>
    public decimal DevelopmentId { get; private set; }

    /// <summary>
    /// Gets the final review status
    /// </summary>
    public string? FinalReview { get; private set; }

    /// <summary>
    /// Initializes a new instance of the LearningSubRecord class
    /// </summary>
    protected LearningSubRecord() { }

    /// <summary>
    /// Creates a new learning sub-record
    /// </summary>
    public static LearningSubRecord Create(
        decimal subId,
        Guid learningRecordId,
        RequestNumber requestNumber,
        decimal developmentModeId,
        TrainingId trainingId,
        string? trainingDetail,
        string? remarks,
        decimal developmentId)
    {
        return new LearningSubRecord
        {
            SubId = subId,
            LearningRecordId = learningRecordId,
            RequestNumber = requestNumber,
            DevelopmentModeId = developmentModeId,
            TrainingId = trainingId,
            TrainingDetail = trainingDetail,
            Remarks = remarks,
            DevelopmentId = developmentId
        };
    }

    /// <summary>
    /// Updates the training detail
    /// </summary>
    public void UpdateTrainingDetail(string? trainingDetail, string? remarks, string? finalReview)
    {
        TrainingDetail = trainingDetail;
        Remarks = remarks;
        FinalReview = finalReview;
    }
}
