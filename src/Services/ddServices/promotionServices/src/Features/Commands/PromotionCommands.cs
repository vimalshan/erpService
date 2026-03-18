using MediatR;
using PromotionService.DTOs;

namespace PromotionService.Features.Commands;

#region Rating Commands
public class CreateRatingCommand : IRequest<RatingDto>
{
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
}

public class UpdateRatingCommand : IRequest<bool>
{
    public long RatingId { get; set; }
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
}

public class FinalizeRatingCommand : IRequest<bool>
{
    public long RatingId { get; set; }
    public string ApprovedBySystemId { get; set; }
}

public class DeleteRatingCommand : IRequest<bool>
{
    public long RatingId { get; set; }
}
#endregion

#region PromotionRecommendation Commands
public class CreatePromotionRecommendationCommand : IRequest<PromotionRecommendationDto>
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

public class UpdatePromotionRecommendationCommand : IRequest<bool>
{
    public long PromotionId { get; set; }
    public string ProposedDesignation { get; set; }
    public string ProposedGrade { get; set; }
    public DateTime PromotionEffectiveDate { get; set; }
    public decimal ProposedSalaryIncrease { get; set; }
    public string PromotionReason { get; set; }
}

public class ApprovePromotionRecommendationCommand : IRequest<bool>
{
    public long PromotionId { get; set; }
    public string ApprovedBySystemId { get; set; }
}

public class RejectPromotionRecommendationCommand : IRequest<bool>
{
    public long PromotionId { get; set; }
    public string ReasonForRejection { get; set; }
}

public class HoldPromotionRecommendationCommand : IRequest<bool>
{
    public long PromotionId { get; set; }
    public string ReasonForHold { get; set; }
}

public class DeletePromotionRecommendationCommand : IRequest<bool>
{
    public long PromotionId { get; set; }
}
#endregion

#region IncrementRequest Commands
public class CreateIncrementRequestCommand : IRequest<IncrementRequestDto>
{
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string IncrementType { get; set; }
    public decimal CurrentBaseSalary { get; set; }
    public decimal ProposedBaseSalary { get; set; }
    public string IncrementReason { get; set; }
    public DateTime EffectiveFromDate { get; set; }
}

public class UpdateIncrementRequestCommand : IRequest<bool>
{
    public long IncrementId { get; set; }
    public decimal ProposedBaseSalary { get; set; }
    public string IncrementReason { get; set; }
    public DateTime EffectiveFromDate { get; set; }
}

public class ApproveIncrementRequestCommand : IRequest<bool>
{
    public long IncrementId { get; set; }
    public string ApprovedBySystemId { get; set; }
}

public class RejectIncrementRequestCommand : IRequest<bool>
{
    public long IncrementId { get; set; }
    public string ReasonForRejection { get; set; }
}

public class DeleteIncrementRequestCommand : IRequest<bool>
{
    public long IncrementId { get; set; }
}
#endregion

#region VTCAssessment Commands
public class CreateVTCAssessmentCommand : IRequest<VTCAssessmentDto>
{
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public int Quarter { get; set; }
    public decimal Score { get; set; }
}

public class UpdateVTCAssessmentCommand : IRequest<bool>
{
    public long VTCAssessmentId { get; set; }
    public decimal Score { get; set; }
}

public class DeleteVTCAssessmentCommand : IRequest<bool>
{
    public long VTCAssessmentId { get; set; }
}
#endregion

#region HorizontalPromotion Commands
public class CreateHorizontalPromotionCommand : IRequest<HorizontalPromotionDto>
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
    public decimal UpdatedBy { get; set; }
}

public class ConfirmHorizontalPromotionCommand : IRequest<bool>
{
    public decimal TransactionId { get; set; }
}

public class DeleteHorizontalPromotionCommand : IRequest<bool>
{
    public decimal TransactionId { get; set; }
}
#endregion

#region VTCCorrection Commands
public class CreateVTCCorrectionCommand : IRequest<VTCCorrectionDto>
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

public class ApproveVTCCorrectionCommand : IRequest<bool>
{
    public decimal RateId { get; set; }
    public decimal ApprovedBy { get; set; }
}

public class RejectVTCCorrectionCommand : IRequest<bool>
{
    public decimal RateId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
#endregion

#region DirectIncrement Commands
public class CreateDirectIncrementCommand : IRequest<DirectIncrementDto>
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

public class DeleteDirectIncrementCommand : IRequest<bool>
{
    public decimal IncrementId { get; set; }
}
#endregion

