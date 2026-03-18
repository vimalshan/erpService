using MediatR;
using PromotionService.DTOs;

namespace PromotionService.Features.Queries;

#region Rating Queries
public class GetRatingByIdQuery : IRequest<RatingDto>
{
    public long RatingId { get; set; }
}

public class GetRatingsByEmployeeQuery : IRequest<IEnumerable<RatingDto>>
{
    public long EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllRatingsQuery : IRequest<IEnumerable<RatingDto>>
{
    public int? DDYear { get; set; }
    public string Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPendingRatingsQuery : IRequest<IEnumerable<RatingDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetHighPerformerRatingsQuery : IRequest<IEnumerable<RatingDto>>
{
    public int DDYear { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

#region PromotionRecommendation Queries
public class GetPromotionByIdQuery : IRequest<PromotionRecommendationDto>
{
    public long PromotionId { get; set; }
}

public class GetPromotionsByEmployeeQuery : IRequest<IEnumerable<PromotionRecommendationDto>>
{
    public long EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllPromotionsQuery : IRequest<IEnumerable<PromotionRecommendationDto>>
{
    public string Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPendingPromotionsQuery : IRequest<IEnumerable<PromotionRecommendationDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPromotionsByRatingQuery : IRequest<IEnumerable<PromotionRecommendationDto>>
{
    public long RatingId { get; set; }
}
#endregion

#region IncrementRequest Queries
public class GetIncrementByIdQuery : IRequest<IncrementRequestDto>
{
    public long IncrementId { get; set; }
}

public class GetIncrementsByEmployeeQuery : IRequest<IEnumerable<IncrementRequestDto>>
{
    public long EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllIncrementsQuery : IRequest<IEnumerable<IncrementRequestDto>>
{
    public string IncrementType { get; set; }
    public string Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPendingIncrementsQuery : IRequest<IEnumerable<IncrementRequestDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetIncrementsByRatingQuery : IRequest<IEnumerable<IncrementRequestDto>>
{
    public long RatingId { get; set; }
}
#endregion

#region VTCAssessment Queries
public class GetVTCAssessmentByIdQuery : IRequest<VTCAssessmentDto>
{
    public long VTCAssessmentId { get; set; }
}

public class GetVTCAssessmentsByEmployeeQuery : IRequest<IEnumerable<VTCAssessmentDto>>
{
    public long EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetVTCAssessmentsByYearQuery : IRequest<IEnumerable<VTCAssessmentDto>>
{
    public int DDYear { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

#region HorizontalPromotion Queries
public class GetHorizontalPromotionByIdQuery : IRequest<HorizontalPromotionDto>
{
    public decimal TransactionId { get; set; }
}

public class GetHorizontalPromotionsByEmployeeQuery : IRequest<IEnumerable<HorizontalPromotionDto>>
{
    public decimal EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllHorizontalPromotionsQuery : IRequest<IEnumerable<HorizontalPromotionDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

#region AppraisalAmount Queries
public class GetAppraisalAmountByIdQuery : IRequest<AppraisalAmountDto>
{
    public decimal SerialNo { get; set; }
}

public class GetAppraisalAmountsByBandQuery : IRequest<IEnumerable<AppraisalAmountDto>>
{
    public decimal BandId { get; set; }
}

public class GetAllAppraisalAmountsQuery : IRequest<IEnumerable<AppraisalAmountDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

#region VTCCorrection Queries
public class GetVTCCorrectionByIdQuery : IRequest<VTCCorrectionDto>
{
    public decimal RateId { get; set; }
}

public class GetVTCCorrectionsByEmployeeQuery : IRequest<IEnumerable<VTCCorrectionDto>>
{
    public decimal EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPendingVTCCorrectionsQuery : IRequest<IEnumerable<VTCCorrectionDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

#region DirectIncrement Queries
public class GetDirectIncrementsByEmployeeQuery : IRequest<IEnumerable<DirectIncrementDto>>
{
    public decimal EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetDirectIncrementsByYearQuery : IRequest<IEnumerable<DirectIncrementDto>>
{
    public decimal YearId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

