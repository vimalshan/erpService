namespace PromotionService.Domain.Entities;

// ────────────────────────────────────────────────────────────────
// Base audit info shared by most aggregates
// ────────────────────────────────────────────────────────────────
public abstract class AuditableEntity
{
    public decimal ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

// ────────────────────────────────────────────────────────────────
// DD_APPRAISALAMOUNT  – Band/Grade appraisal amount matrix
// ────────────────────────────────────────────────────────────────
public class AppraisalAmount
{
    public decimal SerialNo { get; set; }          // DD_SRL_NO  PK
    public decimal? BandId { get; set; }           // DD_BND_ID
    public string? VtcRating { get; set; }         // DD_BND_APR  (H1/H2A/H3…)
    public decimal? Amount { get; set; }           // DD_BND_AMT
    public decimal? BandMaxAmount { get; set; }    // DD_BND_MAX
    public decimal? BandMinAmount { get; set; }    // DD_BND_MIN
    public DateTime? AppraisalPeriodFrom { get; set; }  // DD_BND_EFF
    public DateTime? AppraisalPeriodTo { get; set; }    // DD_BND_END
    public decimal? BandPercentage { get; set; }   // DD_BND_PER
    public decimal? MinCtc { get; set; }           // DD_MIN_CTC
    public decimal? MinPercent { get; set; }       // DD_MIN_PER
    public string? GradeCode { get; set; }         // DD_GRADECODE
    public decimal? GradeId { get; set; }          // DD_GRADEID
    public decimal? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

// ────────────────────────────────────────────────────────────────
// DD_CTGPROMOTION  – CTG (Category) Promotion requests
// ────────────────────────────────────────────────────────────────
public class CTGPromotion
{
    public decimal RequestNumber { get; set; }     // DD_REQ_NUM  PK
    public decimal? ApprSysId { get; set; }        // DD_APPRSYSID
    public decimal? QuotationNo { get; set; }      // DD_QTNNO
    public string? AppType { get; set; }           // DD_APPTYPE
    public string? Answer1 { get; set; }           // DD_ANS1
    public string? Answer2 { get; set; }           // DD_ANS2
    public decimal? LevelId { get; set; }          // DD_LEVELID
    public decimal? NewGradeId { get; set; }       // DD_NEWGRADEID
    public decimal? LastUpdatedBy { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
    public string? PromotionRemarks { get; set; }  // DD_PROMO_REMARKS
}

// ────────────────────────────────────────────────────────────────
// DD_GRADE_INCTYPE  – Grade increment type configuration
// ────────────────────────────────────────────────────────────────
public class GradeIncrementType
{
    public decimal? GradeId { get; set; }          // DD_GRADEID
    public string? FormCategory { get; set; }      // DD_FORMCAT
    public decimal? YearId { get; set; }           // DD_YEARID
    public string? IncrementType { get; set; }     // DD_INCTYPE
    public string? GradeCode { get; set; }         // DD_GRADECODE (3 char)
    public string? ProbationRating { get; set; }   // DD_PROBRATING
    public decimal? VtcPercent { get; set; }       // DD_VPPER
    public decimal? HorizontalPercent { get; set; }// DD_HPPER
}

// ────────────────────────────────────────────────────────────────
// DD_HORIZONTAL  – Horizontal promotion transactions
// ────────────────────────────────────────────────────────────────
public class HorizontalPromotion
{
    public decimal TransactionId { get; set; }     // PROMOTION_TRANID  PK
    public decimal? EmployeeSystemId { get; set; } // PROMOTION_EMPSYSID
    public decimal? PromotionScore { get; set; }   // PROMOTION_SCORE
    public decimal? GradeId { get; set; }          // PROMOTION_GRADE
    public decimal? CurrentLevelId { get; set; }   // PROMOTION_CURLEVELID
    public decimal? NewLevelId { get; set; }       // PROMOTION_NEWLEVELID
    public DateTime? EffectiveFrom { get; set; }   // PROMOTION_EFFFROM
    public decimal? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public decimal? PositionId { get; set; }
    public string? OldPositionName { get; set; }
    public string? OldPositionDesignation { get; set; }
    public string? NewPositionName { get; set; }
    public string? NewPositionDesignation { get; set; }
    public decimal? PosUpdatedBy { get; set; }
    public DateTime? PosUpdatedOn { get; set; }
    public string? ConfirmHrms { get; set; }       // Y/N
}

// ────────────────────────────────────────────────────────────────
// DD_HORIZONTAL_POSITION  – Horizontal position mapping
// ────────────────────────────────────────────────────────────────
public class HorizontalPosition
{
    public decimal EmployeeSystemId { get; set; }  // PK part
    public decimal YearId { get; set; }            // PK part
    public decimal PositionId { get; set; }        // PK part
    public string OldPositionName { get; set; } = string.Empty;
    public string OldPositionDesignation { get; set; } = string.Empty;
    public string NewPositionName { get; set; } = string.Empty;
    public string NewPositionDesignation { get; set; } = string.Empty;
    public decimal UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string ConfirmHrms { get; set; } = "N";
}

// ────────────────────────────────────────────────────────────────
// DD_INCDIRECT  – Direct salary increments
// ────────────────────────────────────────────────────────────────
public class DirectIncrement
{
    public decimal IncrementId { get; set; }       // DDINC_ID  PK
    public decimal EmployeeSystemId { get; set; }  // DDINC_EMPSYSID
    public decimal YearId { get; set; }            // DDINC_YEARID
    public decimal Amount { get; set; }            // DDINC_AMOUNT
    public string SalaryType { get; set; } = string.Empty;  // DDINC_SALTYPE
    public decimal UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public decimal? RatingAmount { get; set; }     // DDINC_RATAMT
    public decimal? PromotionAmount { get; set; }  // DDINC_PROMAMNT
    public decimal? Percent { get; set; }          // DDINC_PER
}

// ────────────────────────────────────────────────────────────────
// DD_PERFORMANCERATING  – Performance rating details
// ────────────────────────────────────────────────────────────────
public class PerformanceRating
{
    public decimal? RequestNumber { get; set; }    // PER_REQNUM
    public decimal? Rating { get; set; }           // PER_RATING  avg
    public decimal? PinNumber { get; set; }        // PER_PIN_NUM
    public string? Comments { get; set; }          // PER_COMMENTS
    public string? Rating1 { get; set; }           // PER_RATING1  (text label)
    public string? UserId { get; set; }            // PER_USERID
    public decimal? SerialNo { get; set; }         // PER_SRLNO
    public decimal? MeanRating { get; set; }       // PER_MEAN_RATING
    public string? MeanRemarks { get; set; }       // PER_MEAN_REMARKS
    public decimal? AchievementRating { get; set; }// PER_ACH_RATING  overall
    public decimal? ResultAvg { get; set; }        // PER_RESULT_AVG
    public decimal? ApproachAvg { get; set; }      // PER_APPROACH_AVG
}

// ────────────────────────────────────────────────────────────────
// DD_PROMOTIONLETTER  – Promotion letter details
// ────────────────────────────────────────────────────────────────
public class PromotionLetter
{
    public decimal? PinNumber { get; set; }        // DD_PIN_NUM
    public decimal? CrtPin { get; set; }           // DD_CRT_PIN
    public string? AppraiserName { get; set; }     // DD_APR_NAM
    public string? SignatoryName { get; set; }     // DD_SIG_NAM
    public string? SignatoryDesignation { get; set; } // DD_SIG_DSG
    public string? AppraiseeBusiness { get; set; } // DD_APR_BUS
    public string? Para1 { get; set; }             // DD_APR_PR1
    public string? Para2 { get; set; }             // DD_APR_PR2
    public string? Para3 { get; set; }             // DD_APR_PR3
    public string? Para4 { get; set; }             // DD_APR_PR4
    public string? Para5 { get; set; }             // DD_APR_PR5
    public string? Para6 { get; set; }             // DD_APR_PR6
    public DateTime? PrintDate { get; set; }       // DD_PRN_DAT
    public string? AppraiserSign { get; set; }     // DD_APR_SIN
    public string? AppraiserDesignation { get; set; } // DD_APR_DSG
    public string? AppraiserBand { get; set; }     // DD_APR_BND
    public decimal? AppraisalIncrement { get; set; } // DD_APR_INC
    public decimal? AppraisalPay { get; set; }     // DD_APR_PAY
    public decimal? AppraisalFlexPay { get; set; } // DD_APR_FLX
    public DateTime? EffectiveDate { get; set; }   // DD_EFF_DAT
}

// ────────────────────────────────────────────────────────────────
// DD_PROMOTIONPERIOD  – Promotion period lookup
// ────────────────────────────────────────────────────────────────
public class PromotionPeriod
{
    public decimal? PromotionId { get; set; }      // DD_PRM_ID
    public string? Description { get; set; }       // DD_PRD_DSC
}

// ────────────────────────────────────────────────────────────────
// DD_RATING  – VTC Rating per employee per year
// ────────────────────────────────────────────────────────────────
public class DDRating
{
    public DateTime? RatingFrom { get; set; }      // DD_RAT_FROM
    public DateTime? RatingTo { get; set; }        // DD_RAT_TO
    public decimal? PinNumber { get; set; }        // DD_RAT_PIN
    public string? UserId { get; set; }            // DD_RAT_USR  (25 chars)
    public string? FinalRating { get; set; }       // DD_RAT_FIN  – HR suggested
    public string? PromotionFlag { get; set; }     // DD_RAT_PRO  (1 char)
    public decimal? RequestNo { get; set; }        // DD_RAT_REQ
    public string? ChrRating { get; set; }         // DD_RAT_CHR
    public decimal? BandId { get; set; }           // DD_BND_ID
    public decimal? BasePay { get; set; }          // DD_BAS_AMT
    public decimal? CtcAmount { get; set; }        // DD_CTC_AMT
    public decimal? PromotionFlagNum { get; set; } // DD_PRM_FLG
    public decimal? SpecialSkill { get; set; }     // DD_SPL_SKL
    public decimal? FinalPromotionBand { get; set; } // DD_PRM_BND
    public string? NewPromoFlag { get; set; }      // NEW_PROMO_FLAG  Y/N
    public string? CashLevel { get; set; }         // CASH_LEVEL
    public decimal? CashAmount { get; set; }       // CASH_AMOUNT
    public string? CashReason { get; set; }        // CASH_REASON
    public string? CashOutcome { get; set; }       // CSH_OUTCOME
    public string? BltPerformanceRating { get; set; } // DD_BLT_PER
    public string? BltCompetencyRating { get; set; }  // DD_BLT_COMP
    public string? CltPerformanceRating { get; set; } // DD_CLT_PER
    public string? CltCompetencyRating { get; set; }  // DD_CLT_COMP
    public string? RationalizationFlag { get; set; }  // DD_RAT_FLAG
    public string? NewCashFlag { get; set; }       // NEW_CASH_FLAG
    public decimal? PositionId { get; set; }       // DD_POSITIONID
    public decimal? HorizontalLevelId { get; set; }// DD_PRMHORLEVELID
    public string? Payroll { get; set; }           // DD_PAYROLL
    public decimal? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

// ────────────────────────────────────────────────────────────────
// DD_SUBLEVEL_INC  – Sub-level increment configuration
// ────────────────────────────────────────────────────────────────
public class SubLevelIncrement
{
    public decimal? SubLevelIncId { get; set; }    // SLINC_ID
    public decimal YearId { get; set; }            // SLINC_YEARID
    public DateTime EndDate { get; set; }          // SLINC_ENDDATE
    public decimal GradeId { get; set; }           // SLINC_GRADEID
    public decimal LevelId { get; set; }           // SLINC_LEVELID
    public string Rating { get; set; } = string.Empty; // SLINC_RATING (5 chars)
    public decimal? RateAmount { get; set; }       // SLINC_RATEAMT
    public decimal ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public decimal? MinAmount { get; set; }        // SLINC_MINAMT
    public decimal? MaxAmount { get; set; }        // SLINC_MAXAMT
}

// ────────────────────────────────────────────────────────────────
// DD_VTCCORRECTION  – VTC rating correction (approval workflow)
// ────────────────────────────────────────────────────────────────
public class VTCCorrection
{
    public decimal RateId { get; set; }            // VTC_RATEID  PK
    public decimal EmployeeSystemId { get; set; }  // VTC_EMPSYSID
    public decimal FinancialYearId { get; set; }   // VTC_FINYEARID
    public string Status { get; set; } = string.Empty; // VTC_STATUS  (1 char)
    public decimal GradeId { get; set; }           // VTC_GRADEID
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
    public DateTime CreatedOn { get; set; }
    public decimal? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public decimal? ApprovedBy { get; set; }
    public DateTime? ApprovedOn { get; set; }
}

// ────────────────────────────────────────────────────────────────
// DD_VTCDETERREM  – VTC deterrence/remarks per band+value
// ────────────────────────────────────────────────────────────────
public class VTCDeterrem
{
    public string? BandName { get; set; }          // DE_BND_NAM
    public string? ValueName { get; set; }         // DE_VAL_NAM  (H1,H2A,H3…)
    public string? ValueDescription { get; set; }  // DD_VAL_DSC
    public DateTime? FinancialYear { get; set; }   // DD_FIN_YEAR
}

// ────────────────────────────────────────────────────────────────
// DD_VTCINCLIST  – VTC increment list (summary per employee-year)
// ────────────────────────────────────────────────────────────────
public class VTCIncList
{
    public decimal? YearId { get; set; }           // VTC_DDYEARID
    public decimal? RequestNumber { get; set; }    // VTC_REQ_NUM
    public string? DDType { get; set; }            // VTC_DDTYPE
    public string? SalaryType { get; set; }        // VTC_SALTYPE
    public string? EmployeeUserId { get; set; }    // VTC_REQ_USERID  (25 chars)
    public decimal? EmployeeSystemId { get; set; } // VTC_REQ_EMPSYSID
    public string? EmployeeName { get; set; }      // VTC_REQ_NAM
    public decimal? UnitId { get; set; }
    public decimal? BusinessId { get; set; }
    public decimal? GradeId { get; set; }
    public string? GradeCode { get; set; }
    public string? BandLevel { get; set; }
    public decimal? LevelId { get; set; }
    public DateTime? GroupDoj { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public decimal? PromotionScore { get; set; }
    public decimal? CurrentCtc { get; set; }
    public string? RatingReviewDD { get; set; }
    public string? RatingBlt { get; set; }
    public string? RatingClt { get; set; }
    public decimal? BandId { get; set; }
    public string? RatingBand { get; set; }
    public decimal? IncrementAmount { get; set; }
    public decimal? PreHorizontalPoints { get; set; }
    public decimal? RatingPoints { get; set; }
    public decimal? NewHorizontalPoints { get; set; }
    public string? HorizontalPromotionEligible { get; set; }
    public string? HorizontalPromotionBlt { get; set; }
    public string? VerticalPromotionBlt { get; set; }
    public decimal? PromotionBand { get; set; }
    public decimal? PromotionIncrementAmount { get; set; }
    public string? RatifyFlag { get; set; }
    public decimal? RatifyAmount { get; set; }
    public decimal? EmployeeNumber { get; set; }
    public decimal? PinNo { get; set; }
    public string? Unit { get; set; }
    public string? Business { get; set; }
    public decimal? RevisedCtc { get; set; }
    public decimal? PercentIncrease { get; set; }
    public decimal? OldPercent { get; set; }
    public string? OldRating { get; set; }
    public decimal? ExperienceMonths { get; set; }
    public string? MyPromotion { get; set; }
    public string? ProbationFlag { get; set; }
    public DateTime? LastVerticalPromoDate { get; set; }
    public string? IncrementType { get; set; }
    public string? SameAppraisalReview { get; set; }
    public string? PromoFlag { get; set; }
    public string? PromoReviewDD { get; set; }
    public string? PromoLevelType { get; set; }
    public decimal? PromoLevelId { get; set; }
    public decimal? PromoGradeId { get; set; }
    public string? PromoLevelTypeBlt { get; set; }
    public decimal? PromoLevelIdBlt { get; set; }
    public decimal? PromoSubLevelIdBlt { get; set; }
    public string? DDPayroll { get; set; }
    public decimal? LogEmployeeSystemId { get; set; }
    public string? LogUserId { get; set; }
    public DateTime? LogRunOn { get; set; }
    public string? NewGrade2017 { get; set; }
    public decimal? NewGradeId { get; set; }
    public decimal? IncrementPercent { get; set; }
    public string? ConfirmPayFlag { get; set; }
    public decimal? ConfirmPayAmount { get; set; }
    public decimal? VerticalPercent { get; set; }
    public decimal? HorizontalPercent { get; set; }
    public decimal? OrgId { get; set; }
    public decimal? BusId { get; set; }
}

// ────────────────────────────────────────────────────────────────
// DD_REQNUM_COMPE_INDPROM – Competency indicator for promotion
// ────────────────────────────────────────────────────────────────
public class CompetencyIndicatorPromotion
{
    public decimal? RequestNumber { get; set; }    // REQNUM
    public decimal? CompetencyNumber { get; set; } // COMPNUM
    public decimal? IndicatorNumber { get; set; }  // INDNUM
    public string? Flag { get; set; }              // FLAG (1 char)
    public decimal? PinNumber { get; set; }        // PINNUM
}

// ────────────────────────────────────────────────────────────────
// Rating  (service-layer aggregate – not DD_ prefixed)
// ────────────────────────────────────────────────────────────────
public class Rating
{
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public decimal AppraisalScore { get; set; }
    public decimal CompetencyScore { get; set; }
    public decimal GoalCompletionScore { get; set; }
    public decimal FinalRating { get; set; }
    public string RatingGrade { get; set; } = string.Empty;   // A/B/C/D
    public string? RatingCategory { get; set; }               // Exceptional/Normal/Below
    public DateTime RatedOn { get; set; }
    public string Status { get; set; } = "P";                 // P=Pending, F=Finalized
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public ICollection<PromotionRecommendation> Promotions { get; set; } = new List<PromotionRecommendation>();
    public ICollection<IncrementRequest> Increments { get; set; } = new List<IncrementRequest>();

    // Domain Events (collected in memory, dispatched by UoW)
    private readonly List<object> _domainEvents = new();
    public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(object @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

// ────────────────────────────────────────────────────────────────
// PromotionRecommendation  (service-layer aggregate)
// ────────────────────────────────────────────────────────────────
public class PromotionRecommendation
{
    public long PromotionId { get; set; }
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string CurrentDesignation { get; set; } = string.Empty;
    public string CurrentGrade { get; set; } = string.Empty;
    public string ProposedDesignation { get; set; } = string.Empty;
    public string ProposedGrade { get; set; } = string.Empty;
    public DateTime PromotionEffectiveDate { get; set; }
    public decimal ProposedSalaryIncrease { get; set; }
    public string? PromotionReason { get; set; }
    public string? RejectionReason { get; set; }
    public string Status { get; set; } = "P";  // P/A/R/H
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBySystemId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public Rating? Rating { get; set; }

    private readonly List<object> _domainEvents = new();
    public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(object @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

// ────────────────────────────────────────────────────────────────
// IncrementRequest  (service-layer aggregate)
// ────────────────────────────────────────────────────────────────
public class IncrementRequest
{
    public long IncrementId { get; set; }
    public long RatingId { get; set; }
    public long EmployeeSystemId { get; set; }
    public string IncrementType { get; set; } = string.Empty;  // Annual/Special/Merit
    public decimal CurrentBaseSalary { get; set; }
    public decimal ProposedBaseSalary { get; set; }
    public decimal IncrementAmount { get; set; }
    public decimal IncrementPercentage { get; set; }
    public string IncrementReason { get; set; } = string.Empty;
    public DateTime EffectiveFromDate { get; set; }
    public string Status { get; set; } = "P";  // P/A/R
    public string? RejectionReason { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public long? ApprovedBySystemId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public Rating? Rating { get; set; }

    private readonly List<object> _domainEvents = new();
    public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(object @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

// ────────────────────────────────────────────────────────────────
// VTCAssessment  (service-layer)
// ────────────────────────────────────────────────────────────────
public class VTCAssessment
{
    public long VTCAssessmentId { get; set; }
    public long EmployeeSystemId { get; set; }
    public int DDYear { get; set; }
    public int Quarter { get; set; }
    public decimal Score { get; set; }
    public string? Status { get; set; }
    public DateTime AssessedOn { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
}

