namespace PromotionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PromotionService.Domain.Entities;

public class PromotionDbContext : DbContext
{
    public PromotionDbContext(DbContextOptions<PromotionDbContext> options) : base(options) { }

    // ── Service-layer aggregates ─────────────────────────────────
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<PromotionRecommendation> PromotionRecommendations { get; set; }
    public DbSet<IncrementRequest> IncrementRequests { get; set; }
    public DbSet<VTCAssessment> VTCAssessments { get; set; }

    // ── DD_ SQL-schema entities ──────────────────────────────────
    public DbSet<AppraisalAmount> AppraisalAmounts { get; set; }
    public DbSet<CTGPromotion> CTGPromotions { get; set; }
    public DbSet<GradeIncrementType> GradeIncrementTypes { get; set; }
    public DbSet<HorizontalPromotion> HorizontalPromotions { get; set; }
    public DbSet<HorizontalPosition> HorizontalPositions { get; set; }
    public DbSet<DirectIncrement> DirectIncrements { get; set; }
    public DbSet<PerformanceRating> PerformanceRatings { get; set; }
    public DbSet<PromotionLetter> PromotionLetters { get; set; }
    public DbSet<PromotionPeriod> PromotionPeriods { get; set; }
    public DbSet<DDRating> DDRatings { get; set; }
    public DbSet<SubLevelIncrement> SubLevelIncrements { get; set; }
    public DbSet<VTCCorrection> VTCCorrections { get; set; }
    public DbSet<VTCDeterrem> VTCDeterrems { get; set; }
    public DbSet<VTCIncList> VTCIncLists { get; set; }
    public DbSet<CompetencyIndicatorPromotion> CompetencyIndicatorPromotions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Service aggregates ───────────────────────────────────
        modelBuilder.Entity<Rating>(e =>
        {
            e.HasKey(x => x.RatingId);
            e.Property(x => x.RatingId).ValueGeneratedOnAdd();
            e.HasIndex(x => new { x.EmployeeSystemId, x.DDYear }).IsUnique();
            e.HasIndex(x => x.RatingGrade);
            e.HasIndex(x => x.Status);
            e.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<PromotionRecommendation>(e =>
        {
            e.HasKey(x => x.PromotionId);
            e.Property(x => x.PromotionId).ValueGeneratedOnAdd();
            e.HasIndex(x => x.EmployeeSystemId);
            e.HasIndex(x => x.Status);
            e.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<IncrementRequest>(e =>
        {
            e.HasKey(x => x.IncrementId);
            e.Property(x => x.IncrementId).ValueGeneratedOnAdd();
            e.HasIndex(x => x.EmployeeSystemId);
            e.HasIndex(x => x.Status);
            e.Ignore(x => x.DomainEvents);
        });

        modelBuilder.Entity<VTCAssessment>(e =>
        {
            e.HasKey(x => x.VTCAssessmentId);
            e.Property(x => x.VTCAssessmentId).ValueGeneratedOnAdd();
        });

        // ── DD_ schema tables ────────────────────────────────────
        modelBuilder.Entity<AppraisalAmount>(e =>
        {
            e.ToTable("DD_APPRAISALAMOUNT");
            e.HasKey(x => x.SerialNo);
            e.Property(x => x.SerialNo).HasColumnName("DD_SRL_NO").HasColumnType("decimal(38,0)");
            e.Property(x => x.BandId).HasColumnName("DD_BND_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.VtcRating).HasColumnName("DD_BND_APR").HasMaxLength(5);
            e.Property(x => x.Amount).HasColumnName("DD_BND_AMT").HasColumnType("decimal(38,0)");
            e.Property(x => x.BandMaxAmount).HasColumnName("DD_BND_MAX").HasColumnType("decimal(38,0)");
            e.Property(x => x.BandMinAmount).HasColumnName("DD_BND_MIN").HasColumnType("decimal(38,0)");
            e.Property(x => x.AppraisalPeriodFrom).HasColumnName("DD_BND_EFF");
            e.Property(x => x.AppraisalPeriodTo).HasColumnName("DD_BND_END");
            e.Property(x => x.BandPercentage).HasColumnName("DD_BND_PER").HasColumnType("decimal(38,0)");
            e.Property(x => x.MinCtc).HasColumnName("DD_MIN_CTC").HasColumnType("decimal(38,0)");
            e.Property(x => x.MinPercent).HasColumnName("DD_MIN_PER").HasColumnType("decimal(38,0)");
            e.Property(x => x.GradeCode).HasColumnName("DD_GRADECODE").HasMaxLength(3);
            e.Property(x => x.GradeId).HasColumnName("DD_GRADEID").HasColumnType("decimal(38,0)");
            e.Property(x => x.ModifiedBy).HasColumnName("DD_MODIFIEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.ModifiedOn).HasColumnName("DD_MODIFIEDON");
        });

        modelBuilder.Entity<CTGPromotion>(e =>
        {
            e.ToTable("DD_CTGPROMOTION");
            e.HasKey(x => x.RequestNumber);
            e.Property(x => x.RequestNumber).HasColumnName("DD_REQ_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.ApprSysId).HasColumnName("DD_APPRSYSID").HasColumnType("decimal(38,0)");
            e.Property(x => x.QuotationNo).HasColumnName("DD_QTNNO").HasColumnType("decimal(38,0)");
            e.Property(x => x.AppType).HasColumnName("DD_APPTYPE").HasMaxLength(3);
            e.Property(x => x.Answer1).HasColumnName("DD_ANS1").HasMaxLength(100);
            e.Property(x => x.Answer2).HasColumnName("DD_ANS2").HasMaxLength(100);
            e.Property(x => x.LevelId).HasColumnName("DD_LEVELID").HasColumnType("decimal(38,0)");
            e.Property(x => x.NewGradeId).HasColumnName("DD_NEWGRADEID").HasColumnType("decimal(38,0)");
            e.Property(x => x.LastUpdatedBy).HasColumnName("DD_LASTUPDATEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.LastUpdatedOn).HasColumnName("DD_LASTUPDATEDON");
            e.Property(x => x.PromotionRemarks).HasColumnName("DD_PROMO_REMARKS").HasMaxLength(1000);
        });

        modelBuilder.Entity<GradeIncrementType>(e =>
        {
            e.ToTable("DD_GRADE_INCTYPE");
            e.HasNoKey();
            e.Property(x => x.GradeId).HasColumnName("DD_GRADEID").HasColumnType("decimal(38,0)");
            e.Property(x => x.FormCategory).HasColumnName("DD_FORMCAT").HasMaxLength(5);
            e.Property(x => x.YearId).HasColumnName("DD_YEARID").HasColumnType("decimal(38,0)");
            e.Property(x => x.IncrementType).HasColumnName("DD_INCTYPE").HasMaxLength(5);
            e.Property(x => x.GradeCode).HasColumnName("DD_GRADECODE").HasMaxLength(3);
            e.Property(x => x.ProbationRating).HasColumnName("DD_PROBRATING").HasMaxLength(5);
            e.Property(x => x.VtcPercent).HasColumnName("DD_VPPER").HasColumnType("decimal(38,0)");
            e.Property(x => x.HorizontalPercent).HasColumnName("DD_HPPER").HasColumnType("decimal(38,0)");
        });

        modelBuilder.Entity<HorizontalPromotion>(e =>
        {
            e.ToTable("DD_HORIZONTAL");
            e.HasKey(x => x.TransactionId);
            e.Property(x => x.TransactionId).HasColumnName("PROMOTION_TRANID").HasColumnType("decimal(38,0)");
            e.Property(x => x.EmployeeSystemId).HasColumnName("PROMOTION_EMPSYSID").HasColumnType("decimal(38,0)");
            e.Property(x => x.PromotionScore).HasColumnName("PROMOTION_SCORE").HasColumnType("decimal(38,0)");
            e.Property(x => x.GradeId).HasColumnName("PROMOTION_GRADE").HasColumnType("decimal(38,0)");
            e.Property(x => x.CurrentLevelId).HasColumnName("PROMOTION_CURLEVELID").HasColumnType("decimal(38,0)");
            e.Property(x => x.NewLevelId).HasColumnName("PROMOTION_NEWLEVELID").HasColumnType("decimal(38,0)");
            e.Property(x => x.EffectiveFrom).HasColumnName("PROMOTION_EFFFROM");
            e.Property(x => x.UpdatedBy).HasColumnName("PROMOTION_UPDATEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.UpdatedOn).HasColumnName("PROMOTION_UPDATEDON");
            e.Property(x => x.PositionId).HasColumnName("PROMOTION_POSITIONID").HasColumnType("decimal(38,0)");
            e.Property(x => x.OldPositionName).HasColumnName("PROMOTION_OLDPOSNAME").HasMaxLength(100);
            e.Property(x => x.OldPositionDesignation).HasColumnName("PROMOTION_OLDPOSDESG").HasMaxLength(100);
            e.Property(x => x.NewPositionName).HasColumnName("PROMOTION_NEWPOSNAME").HasMaxLength(100);
            e.Property(x => x.NewPositionDesignation).HasColumnName("PROMOTION_NEWPOSDESG").HasMaxLength(100);
            e.Property(x => x.PosUpdatedBy).HasColumnName("PROMOTION_POSUPDATEBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.PosUpdatedOn).HasColumnName("PROMOTION_POSUPDATEDON");
            e.Property(x => x.ConfirmHrms).HasColumnName("PROMOTION_CONFIRM_HRMS").HasMaxLength(1);
        });

        modelBuilder.Entity<HorizontalPosition>(e =>
        {
            e.ToTable("DD_HORIZONTAL_POSITION");
            e.HasKey(x => new { x.EmployeeSystemId, x.YearId, x.PositionId });
            e.Property(x => x.EmployeeSystemId).HasColumnName("PROMOTION_EMPSYSID").HasColumnType("decimal(38,0)");
            e.Property(x => x.YearId).HasColumnName("PROMOTION_DDYEARID").HasColumnType("decimal(38,0)");
            e.Property(x => x.PositionId).HasColumnName("PROMOTION_POSITIONID").HasColumnType("decimal(38,0)");
            e.Property(x => x.OldPositionName).HasColumnName("PROMOTION_OLDPOSNAME").HasMaxLength(100);
            e.Property(x => x.OldPositionDesignation).HasColumnName("PROMOTION_OLDPOSDESG").HasMaxLength(100);
            e.Property(x => x.NewPositionName).HasColumnName("PROMOTION_NEWPOSNAME").HasMaxLength(100);
            e.Property(x => x.NewPositionDesignation).HasColumnName("PROMOTION_NEWPOSDESG").HasMaxLength(100);
            e.Property(x => x.UpdatedBy).HasColumnName("PROMOTION_POSUPDATEBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.UpdatedOn).HasColumnName("PROMOTION_POSUPDATEDON");
            e.Property(x => x.ConfirmHrms).HasColumnName("PROMOTION_CONFIRM_HRMS").HasMaxLength(1);
        });

        modelBuilder.Entity<DirectIncrement>(e =>
        {
            e.ToTable("DD_INCDIRECT");
            e.HasKey(x => x.IncrementId);
            e.Property(x => x.IncrementId).HasColumnName("DDINC_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.EmployeeSystemId).HasColumnName("DDINC_EMPSYSID").HasColumnType("decimal(38,0)");
            e.Property(x => x.YearId).HasColumnName("DDINC_YEARID").HasColumnType("decimal(38,0)");
            e.Property(x => x.Amount).HasColumnName("DDINC_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.SalaryType).HasColumnName("DDINC_SALTYPE").HasMaxLength(3);
            e.Property(x => x.UpdatedBy).HasColumnName("DDINC_UPDATEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.UpdatedOn).HasColumnName("DDINC_UPDATEDON");
            e.Property(x => x.RatingAmount).HasColumnName("DDINC_RATAMT").HasColumnType("decimal(38,0)");
            e.Property(x => x.PromotionAmount).HasColumnName("DDINC_PROMAMNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.Percent).HasColumnName("DDINC_PER").HasColumnType("decimal(38,0)");
        });

        modelBuilder.Entity<PerformanceRating>(e =>
        {
            e.ToTable("DD_PERFORMANCERATING");
            e.HasNoKey();
            e.Property(x => x.RequestNumber).HasColumnName("PER_REQNUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.Rating).HasColumnName("PER_RATING").HasColumnType("decimal(38,0)");
            e.Property(x => x.PinNumber).HasColumnName("PER_PIN_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.Comments).HasColumnName("PER_COMMENTS").HasMaxLength(4000);
            e.Property(x => x.Rating1).HasColumnName("PER_RATING1").HasMaxLength(50);
            e.Property(x => x.UserId).HasColumnName("PER_USERID").HasMaxLength(100);
            e.Property(x => x.SerialNo).HasColumnName("PER_SRLNO").HasColumnType("decimal(38,0)");
            e.Property(x => x.MeanRating).HasColumnName("PER_MEAN_RATING").HasColumnType("decimal(38,0)");
            e.Property(x => x.MeanRemarks).HasColumnName("PER_MEAN_REMARKS").HasMaxLength(4000);
            e.Property(x => x.AchievementRating).HasColumnName("PER_ACH_RATING").HasColumnType("decimal(38,0)");
            e.Property(x => x.ResultAvg).HasColumnName("PER_RESULT_AVG").HasColumnType("decimal(38,0)");
            e.Property(x => x.ApproachAvg).HasColumnName("PER_APPROACH_AVG").HasColumnType("decimal(38,0)");
        });

        modelBuilder.Entity<PromotionLetter>(e =>
        {
            e.ToTable("DD_PROMOTIONLETTER");
            e.HasNoKey();
            e.Property(x => x.PinNumber).HasColumnName("DD_PIN_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.CrtPin).HasColumnName("DD_CRT_PIN").HasColumnType("decimal(38,0)");
            e.Property(x => x.AppraiserName).HasColumnName("DD_APR_NAM").HasMaxLength(150);
            e.Property(x => x.SignatoryName).HasColumnName("DD_SIG_NAM").HasMaxLength(150);
            e.Property(x => x.SignatoryDesignation).HasColumnName("DD_SIG_DSG").HasMaxLength(100);
            e.Property(x => x.AppraiseeBusiness).HasColumnName("DD_APR_BUS").HasMaxLength(100);
            e.Property(x => x.Para1).HasColumnName("DD_APR_PR1").HasMaxLength(1000);
            e.Property(x => x.Para2).HasColumnName("DD_APR_PR2").HasMaxLength(1000);
            e.Property(x => x.Para3).HasColumnName("DD_APR_PR3").HasMaxLength(1000);
            e.Property(x => x.Para4).HasColumnName("DD_APR_PR4").HasMaxLength(1000);
            e.Property(x => x.Para5).HasColumnName("DD_APR_PR5").HasMaxLength(1000);
            e.Property(x => x.Para6).HasColumnName("DD_APR_PR6").HasMaxLength(1000);
            e.Property(x => x.PrintDate).HasColumnName("DD_PRN_DAT");
            e.Property(x => x.AppraiserSign).HasColumnName("DD_APR_SIN").HasMaxLength(150);
            e.Property(x => x.AppraiserDesignation).HasColumnName("DD_APR_DSG").HasMaxLength(100);
            e.Property(x => x.AppraiserBand).HasColumnName("DD_APR_BND").HasMaxLength(50);
            e.Property(x => x.AppraisalIncrement).HasColumnName("DD_APR_INC").HasColumnType("decimal(38,0)");
            e.Property(x => x.AppraisalPay).HasColumnName("DD_APR_PAY").HasColumnType("decimal(38,0)");
            e.Property(x => x.AppraisalFlexPay).HasColumnName("DD_APR_FLX").HasColumnType("decimal(38,0)");
            e.Property(x => x.EffectiveDate).HasColumnName("DD_EFF_DAT");
        });

        modelBuilder.Entity<PromotionPeriod>(e =>
        {
            e.ToTable("DD_PROMOTIONPERIOD");
            e.HasNoKey();
            e.Property(x => x.PromotionId).HasColumnName("DD_PRM_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.Description).HasColumnName("DD_PRD_DSC").HasMaxLength(25);
        });

        modelBuilder.Entity<DDRating>(e =>
        {
            e.ToTable("DD_RATING");
            e.HasNoKey();
            e.Property(x => x.RatingFrom).HasColumnName("DD_RAT_FROM");
            e.Property(x => x.RatingTo).HasColumnName("DD_RAT_TO");
            e.Property(x => x.PinNumber).HasColumnName("DD_RAT_PIN").HasColumnType("decimal(38,0)");
            e.Property(x => x.UserId).HasColumnName("DD_RAT_USR").HasMaxLength(25);
            e.Property(x => x.FinalRating).HasColumnName("DD_RAT_FIN").HasMaxLength(4);
            e.Property(x => x.PromotionFlag).HasColumnName("DD_RAT_PRO").HasMaxLength(1);
            e.Property(x => x.RequestNo).HasColumnName("DD_RAT_REQ").HasColumnType("decimal(38,0)");
            e.Property(x => x.ChrRating).HasColumnName("DD_RAT_CHR").HasMaxLength(4);
            e.Property(x => x.BandId).HasColumnName("DD_BND_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.BasePay).HasColumnName("DD_BAS_AMT").HasColumnType("decimal(38,0)");
            e.Property(x => x.CtcAmount).HasColumnName("DD_CTC_AMT").HasColumnType("decimal(38,0)");
            e.Property(x => x.PromotionFlagNum).HasColumnName("DD_PRM_FLG").HasColumnType("decimal(38,0)");
            e.Property(x => x.SpecialSkill).HasColumnName("DD_SPL_SKL").HasColumnType("decimal(38,0)");
            e.Property(x => x.FinalPromotionBand).HasColumnName("DD_PRM_BND").HasColumnType("decimal(38,0)");
            e.Property(x => x.NewPromoFlag).HasColumnName("NEW_PROMO_FLAG").HasMaxLength(1);
            e.Property(x => x.CashLevel).HasColumnName("CASH_LEVEL").HasMaxLength(1);
            e.Property(x => x.CashAmount).HasColumnName("CASH_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.CashReason).HasColumnName("CASH_REASON").HasMaxLength(400);
            e.Property(x => x.CashOutcome).HasColumnName("CSH_OUTCOME").HasMaxLength(400);
            e.Property(x => x.BltPerformanceRating).HasColumnName("DD_BLT_PER").HasMaxLength(50);
            e.Property(x => x.BltCompetencyRating).HasColumnName("DD_BLT_COMP").HasMaxLength(50);
            e.Property(x => x.CltPerformanceRating).HasColumnName("DD_CLT_PER").HasMaxLength(50);
            e.Property(x => x.CltCompetencyRating).HasColumnName("DD_CLT_COMP").HasMaxLength(50);
            e.Property(x => x.RationalizationFlag).HasColumnName("DD_RAT_FLAG").HasMaxLength(1);
            e.Property(x => x.NewCashFlag).HasColumnName("NEW_CASH_FLAG").HasMaxLength(1);
            e.Property(x => x.PositionId).HasColumnName("DD_POSITIONID").HasColumnType("decimal(38,0)");
            e.Property(x => x.HorizontalLevelId).HasColumnName("DD_PRMHORLEVELID").HasColumnType("decimal(38,0)");
            e.Property(x => x.Payroll).HasColumnName("DD_PAYROLL").HasMaxLength(1);
            e.Property(x => x.UpdatedBy).HasColumnName("DD_UPDATEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.UpdatedOn).HasColumnName("DD_UPDATEDON");
        });

        modelBuilder.Entity<SubLevelIncrement>(e =>
        {
            e.ToTable("DD_SUBLEVEL_INC");
            e.HasNoKey();
            e.Property(x => x.SubLevelIncId).HasColumnName("SLINC_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.YearId).HasColumnName("SLINC_YEARID").HasColumnType("decimal(38,0)");
            e.Property(x => x.EndDate).HasColumnName("SLINC_ENDDATE");
            e.Property(x => x.GradeId).HasColumnName("SLINC_GRADEID").HasColumnType("decimal(38,0)");
            e.Property(x => x.LevelId).HasColumnName("SLINC_LEVELID").HasColumnType("decimal(38,0)");
            e.Property(x => x.Rating).HasColumnName("SLINC_RATING").HasMaxLength(5);
            e.Property(x => x.RateAmount).HasColumnName("SLINC_RATEAMT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ModifiedBy).HasColumnName("SLINC_MODIFIEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.ModifiedOn).HasColumnName("SLINC_MODIFIEDON");
            e.Property(x => x.MinAmount).HasColumnName("SLINC_MINAMT").HasColumnType("decimal(38,0)");
            e.Property(x => x.MaxAmount).HasColumnName("SLINC_MAXAMT").HasColumnType("decimal(38,0)");
        });

        modelBuilder.Entity<VTCCorrection>(e =>
        {
            e.ToTable("DD_VTCCORRECTION");
            e.HasKey(x => x.RateId);
            e.Property(x => x.RateId).HasColumnName("VTC_RATEID").HasColumnType("decimal(38,0)");
            e.Property(x => x.EmployeeSystemId).HasColumnName("VTC_EMPSYSID").HasColumnType("decimal(38,0)");
            e.Property(x => x.FinancialYearId).HasColumnName("VTC_FINYEARID").HasColumnType("decimal(38,0)");
            e.Property(x => x.Status).HasColumnName("VTC_STATUS").HasMaxLength(1);
            e.Property(x => x.GradeId).HasColumnName("VTC_GRADEID").HasColumnType("decimal(38,0)");
            e.Property(x => x.OldRating).HasColumnName("VTC_OLDRATING").HasMaxLength(20);
            e.Property(x => x.NewRating).HasColumnName("VTC_NEWRATING").HasMaxLength(20);
            e.Property(x => x.OldCash).HasColumnName("VTC_OLDCASH").HasMaxLength(3);
            e.Property(x => x.NewCash).HasColumnName("VTC_NEWCASH").HasMaxLength(3);
            e.Property(x => x.OldPromotion).HasColumnName("VTC_OLDPROMO").HasMaxLength(3);
            e.Property(x => x.NewPromotion).HasColumnName("VTC_NEWPROMO").HasMaxLength(3);
            e.Property(x => x.OldRationalization).HasColumnName("VTC_OLDRATIONAL").HasMaxLength(3);
            e.Property(x => x.NewRationalization).HasColumnName("VTC_NEWRATIONAL").HasMaxLength(3);
            e.Property(x => x.Reason).HasColumnName("VTC_REASON").HasMaxLength(200);
            e.Property(x => x.CreatedBy).HasColumnName("VTC_CREATEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.CreatedOn).HasColumnName("VTC_CREATEDON");
            e.Property(x => x.ModifiedBy).HasColumnName("VTC_MODIFIEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.ModifiedOn).HasColumnName("VTC_MODIFIEDON");
            e.Property(x => x.ApprovedBy).HasColumnName("VTC_APPROVEDBY").HasColumnType("decimal(38,0)");
            e.Property(x => x.ApprovedOn).HasColumnName("VTC_APPROVEDON");
        });

        modelBuilder.Entity<VTCDeterrem>(e =>
        {
            e.ToTable("DD_VTCDETERREM");
            e.HasNoKey();
            e.Property(x => x.BandName).HasColumnName("DE_BND_NAM").HasMaxLength(20);
            e.Property(x => x.ValueName).HasColumnName("DE_VAL_NAM").HasMaxLength(3);
            e.Property(x => x.ValueDescription).HasColumnName("DD_VAL_DSC").HasMaxLength(1000);
            e.Property(x => x.FinancialYear).HasColumnName("DD_FIN_YEAR");
        });

        modelBuilder.Entity<VTCIncList>(e =>
        {
            e.ToTable("DD_VTCINCLIST");
            e.HasNoKey();
            e.Property(x => x.YearId).HasColumnName("VTC_DDYEARID").HasColumnType("decimal(38,0)");
            e.Property(x => x.RequestNumber).HasColumnName("VTC_REQ_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.DDType).HasColumnName("VTC_DDTYPE").HasMaxLength(10);
            e.Property(x => x.SalaryType).HasColumnName("VTC_SALTYPE").HasMaxLength(10);
            e.Property(x => x.EmployeeUserId).HasColumnName("VTC_REQ_USERID").HasMaxLength(25);
            e.Property(x => x.EmployeeSystemId).HasColumnName("VTC_REQ_EMPSYSID").HasColumnType("decimal(38,0)");
            e.Property(x => x.EmployeeName).HasColumnName("VTC_REQ_NAM").HasMaxLength(150);
        });

        modelBuilder.Entity<CompetencyIndicatorPromotion>(e =>
        {
            e.ToTable("DD_REQNUM_COMPE_INDPROM");
            e.HasNoKey();
            e.Property(x => x.RequestNumber).HasColumnName("REQNUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.CompetencyNumber).HasColumnName("COMPNUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.IndicatorNumber).HasColumnName("INDNUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.Flag).HasColumnName("FLAG").HasMaxLength(1);
            e.Property(x => x.PinNumber).HasColumnName("PINNUM").HasColumnType("decimal(38,0)");
        });
    }
}

