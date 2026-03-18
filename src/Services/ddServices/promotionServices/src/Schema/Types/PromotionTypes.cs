using HotChocolate;
using PromotionService.DTOs;

namespace PromotionService.Types;

/// <summary>GraphQL type for Rating entity</summary>
[ObjectType("Rating")]
public class RatingType
{
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
    public decimal FinalRating { get; set; }
    public string? RatingGrade { get; set; }
    public string? RatingCategory { get; set; }
    public string? Status { get; set; } // P=Pending, F=Finalized
    public DateTime RatedOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

/// <summary>GraphQL type for Promotion Recommendation entity</summary>
[ObjectType("PromotionRecommendation")]
public class PromotionRecommendationType
{
    public long PromotionId { get; set; }
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string? CurrentDesignation { get; set; }
    public string? CurrentGrade { get; set; }
    public string? ProposedDesignation { get; set; }
    public string? ProposedGrade { get; set; }
    public DateTime PromotionEffectiveDate { get; set; }
    public decimal ProposedSalaryIncrease { get; set; }
    public string? PromotionReason { get; set; }
    public string? Status { get; set; } // P=Pending, A=Approved, R=Rejected, H=On Hold
    public DateTime CreatedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBySystemId { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

/// <summary>GraphQL type for Increment Request entity</summary>
[ObjectType("IncrementRequest")]
public class IncrementRequestType
{
    public long IncrementId { get; set; }
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string? IncrementType { get; set; } // Annual, Special, Merit
    public decimal CurrentBaseSalary { get; set; }
    public decimal ProposedBaseSalary { get; set; }
    public decimal IncrementAmount { get; set; }
    public decimal IncrementPercentage { get; set; }
    public string? IncrementReason { get; set; }
    public DateTime EffectiveFromDate { get; set; }
    public string? Status { get; set; } // P=Pending, A=Approved, R=Rejected
    public DateTime CreatedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBySystemId { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

/// <summary>GraphQL type for VTC Assessment entity</summary>
[ObjectType("VTCAssessment")]
public class VTCAssessmentType
{
    public long AssessmentId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string? Quarter { get; set; }
    public int Year { get; set; }
    public decimal Score { get; set; }
    public string? Remarks { get; set; }
    public DateTime AssessedOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

/// <summary>Payload type for promotion operations</summary>
[ObjectType("PromotionPayload")]
public class PromotionPayloadType
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public RatingType? Rating { get; set; }
    public PromotionRecommendationType? Promotion { get; set; }
    public IncrementRequestType? Increment { get; set; }
    public VTCAssessmentType? Assessment { get; set; }
}

/// <summary>Payload type for list operations with pagination</summary>
[ObjectType("PromotionListPayload")]
public class PromotionListPayloadType
{
    public IEnumerable<RatingType>? Ratings { get; set; }
    public IEnumerable<PromotionRecommendationType>? Promotions { get; set; }
    public IEnumerable<IncrementRequestType>? Increments { get; set; }
    public IEnumerable<VTCAssessmentType>? Assessments { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
