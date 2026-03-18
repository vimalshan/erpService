namespace PromotionService.DTOs;

#region Rating DTOs
public class RatingDto
{
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
    public decimal FinalRating { get; set; }
    public string RatingGrade { get; set; }
    public string RatingCategory { get; set; }
    public DateTime RatedOn { get; set; }
    public string Status { get; set; }
    public List<PromotionRecommendationDto> Promotions { get; set; }
    public List<IncrementRequestDto> Increments { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class CreateRatingDto
{
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
}

public class UpdateRatingDto
{
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
}

public class FinalizeRatingDto
{
    public string ApprovedBySystemId { get; set; }
}
#endregion

#region PromotionRecommendation DTOs
public class PromotionRecommendationDto
{
    public long PromotionId { get; set; }
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string CurrentDesignation { get; set; }
    public string CurrentGrade { get; set; }
    public string ProposedDesignation { get; set; }
    public string ProposedGrade { get; set; }
    public DateTime PromotionEffectiveDate { get; set; }
    public decimal ProposedSalaryIncrease { get; set; }
    public string PromotionReason { get; set; }
    public string Status { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBySystemId { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class CreatePromotionRecommendationDto
{
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string CurrentDesignation { get; set; }
    public string CurrentGrade { get; set; }
    public string ProposedDesignation { get; set; }
    public string ProposedGrade { get; set; }
    public DateTime PromotionEffectiveDate { get; set; }
    public decimal ProposedSalaryIncrease { get; set; }
    public string PromotionReason { get; set; }
}

public class UpdatePromotionRecommendationDto
{
    public string ProposedDesignation { get; set; }
    public string ProposedGrade { get; set; }
    public DateTime PromotionEffectiveDate { get; set; }
    public decimal ProposedSalaryIncrease { get; set; }
    public string PromotionReason { get; set; }
}

public class ApprovePromotionDto
{
    public string ApprovedBySystemId { get; set; }
}

public class RejectPromotionDto
{
    public string ReasonForRejection { get; set; }
}
#endregion

#region IncrementRequest DTOs
public class IncrementRequestDto
{
    public long IncrementId { get; set; }
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string IncrementType { get; set; }
    public decimal CurrentBaseSalary { get; set; }
    public decimal ProposedBaseSalary { get; set; }
    public decimal IncrementAmount { get; set; }
    public decimal IncrementPercentage { get; set; }
    public string IncrementReason { get; set; }
    public DateTime EffectiveFromDate { get; set; }
    public string Status { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class CreateIncrementRequestDto
{
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string IncrementType { get; set; }
    public decimal CurrentBaseSalary { get; set; }
    public decimal ProposedBaseSalary { get; set; }
    public string IncrementReason { get; set; }
    public DateTime EffectiveFromDate { get; set; }
}

public class UpdateIncrementRequestDto
{
    public decimal ProposedBaseSalary { get; set; }
    public string IncrementReason { get; set; }
    public DateTime EffectiveFromDate { get; set; }
}

public class ApproveIncrementDto
{
    public string ApprovedBySystemId { get; set; }
}
#endregion

#region VTCAssessment DTOs
public class VTCAssessmentDto
{
    public long VTCAssessmentId { get; set; }
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public int Quarter { get; set; }
    public decimal Score { get; set; }
    public string Status { get; set; }
    public DateTime AssessedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class CreateVTCAssessmentDto
{
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public int Quarter { get; set; }
    public decimal Score { get; set; }
}

public class UpdateVTCAssessmentDto
{
    public decimal Score { get; set; }
}
#endregion

#region AppraisalAmount DTOs
public class AppraisalAmountDto
{
    public decimal SerialNo { get; set; }
    public decimal? BandId { get; set; }
    public string? VtcRating { get; set; }
    public decimal? Amount { get; set; }
    public decimal? BandMaxAmount { get; set; }
    public decimal? BandMinAmount { get; set; }
    public decimal? BandPercentage { get; set; }
    public DateTime? AppraisalPeriodFrom { get; set; }
    public DateTime? AppraisalPeriodTo { get; set; }
    public string? GradeCode { get; set; }
    public decimal? GradeId { get; set; }
}
#endregion

#region HorizontalPromotion DTOs
public class HorizontalPromotionDto
{
    public decimal TransactionId { get; set; }
    public decimal? EmployeeSystemId { get; set; }
    public decimal? PromotionScore { get; set; }
    public decimal? GradeId { get; set; }
    public decimal? CurrentLevelId { get; set; }
    public decimal? NewLevelId { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public string? OldPositionName { get; set; }
    public string? NewPositionName { get; set; }
    public string? ConfirmHrms { get; set; }
}

public class CreateHorizontalPromotionDto
{
    public decimal EmployeeSystemId { get; set; }
    public decimal? PromotionScore { get; set; }
    public decimal? GradeId { get; set; }
    public decimal? CurrentLevelId { get; set; }
    public decimal? NewLevelId { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public decimal? PositionId { get; set; }
    public string? OldPositionName { get; set; }
    public string? OldPositionDesignation { get; set; }
    public string? NewPositionName { get; set; }
    public string? NewPositionDesignation { get; set; }
}
#endregion

#region VTCCorrection DTOs
public class VTCCorrectionDto
{
    public decimal RateId { get; set; }
    public decimal EmployeeSystemId { get; set; }
    public decimal FinancialYearId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal GradeId { get; set; }
    public string OldRating { get; set; } = string.Empty;
    public string NewRating { get; set; } = string.Empty;
    public string OldPromotion { get; set; } = string.Empty;
    public string NewPromotion { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public decimal CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal? ApprovedBy { get; set; }
    public DateTime? ApprovedOn { get; set; }
}

public class CreateVTCCorrectionDto
{
    public decimal EmployeeSystemId { get; set; }
    public decimal FinancialYearId { get; set; }
    public decimal GradeId { get; set; }
    public string OldRating { get; set; } = string.Empty;
    public string NewRating { get; set; } = string.Empty;
    public string? OldCash { get; set; }
    public string? NewCash { get; set; }
    public string OldPromotion { get; set; } = string.Empty;
    public string NewPromotion { get; set; } = string.Empty;
    public string OldRationalization { get; set; } = string.Empty;
    public string NewRationalization { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public decimal CreatedBy { get; set; }
}

public class ApproveVTCCorrectionDto
{
    public decimal ApprovedBy { get; set; }
}
#endregion

#region DirectIncrement DTOs
public class DirectIncrementDto
{
    public decimal IncrementId { get; set; }
    public decimal EmployeeSystemId { get; set; }
    public decimal YearId { get; set; }
    public decimal Amount { get; set; }
    public string SalaryType { get; set; } = string.Empty;
    public decimal? RatingAmount { get; set; }
    public decimal? PromotionAmount { get; set; }
    public decimal? Percent { get; set; }
}

public class CreateDirectIncrementDto
{
    public decimal EmployeeSystemId { get; set; }
    public decimal YearId { get; set; }
    public decimal Amount { get; set; }
    public string SalaryType { get; set; } = string.Empty;
    public decimal UpdatedBy { get; set; }
    public decimal? RatingAmount { get; set; }
    public decimal? PromotionAmount { get; set; }
    public decimal? Percent { get; set; }
}
#endregion

