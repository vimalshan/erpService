using Microsoft.EntityFrameworkCore;
using AppraisalService.Domain.Entities;
using AppraisalService.Domain;

namespace AppraisalService.Infrastructure.Persistence.Data;

/// <summary>
/// EF Core DbContext for AppraisalService
/// </summary>
public class AppraisalDbContext : DbContext
{
    public DbSet<AppraisalMainEntity> AppraisalMains { get; set; } = null!;
    public DbSet<AppraisalDetailsEntity> AppraisalDetails { get; set; } = null!;
    public DbSet<AppraisalBandEntity> AppraisalBands { get; set; } = null!;
    public DbSet<CompetencyAssessmentEntity> CompetencyAssessments { get; set; } = null!;
    public DbSet<EmployeeGoalEntity> EmployeeGoals { get; set; } = null!;

    public AppraisalDbContext(DbContextOptions<AppraisalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Exclude DomainEvent from model - it's not a table entity
        modelBuilder.Ignore<DomainEvent>();

        // AppraisalBand Configuration
        modelBuilder.Entity<AppraisalBandEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("DD_BND_ID");
            entity.Property(e => e.Description).HasColumnName("DD_BND_DSC").HasMaxLength(20);
            entity.Property(e => e.Designation).HasColumnName("DD_BND_DSG").HasMaxLength(200);
            entity.Property(e => e.SignatoryName).HasColumnName("DD_SIG_NAM").HasMaxLength(200);
            entity.Property(e => e.SignatoryDesignation).HasColumnName("DD_SIG_DSG").HasMaxLength(200);
            entity.Property(e => e.Code).HasColumnName("DD_BND_COD").HasMaxLength(3);
            entity.Property(e => e.FormFlag).HasColumnName("DD_FORMFLAG");
            entity.Property(e => e.GradeId).HasColumnName("DD_GRADEID");
            entity.ToTable("DD_APPRAISALBAND");
        });

        // AppraisalMain Configuration
        modelBuilder.Entity<AppraisalMainEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("AP_REQ_NUM").ValueGeneratedNever();
            entity.Property(e => e.UserCode).HasColumnName("AP_USR_COD").HasMaxLength(25).IsRequired();
            entity.Property(e => e.UserNumber).HasColumnName("AP_USR_NUM");
            entity.Property(e => e.PinNumber).HasColumnName("AP_PIN_NUM");
            entity.Property(e => e.EntryDate).HasColumnName("AP_ENT_DAT");
            entity.Property(e => e.GradeId).HasColumnName("AP_GRADEID");
            entity.Property(e => e.UnitId).HasColumnName("AP_UNITID");
            entity.Property(e => e.YearId).HasColumnName("AP_YEARID");
            entity.Property(e => e.CancellationRemarks).HasColumnName("AP_CAN_REM").HasMaxLength(4000);
            entity.Property(e => e.AppraisalStartDate).HasColumnName("AP_ST_FIN");
            entity.Property(e => e.AppraisalEndDate).HasColumnName("AP_ED_FIN");
            entity.Property(e => e.CompletedOn).HasColumnName("AP_FIN_DAT");
            entity.Property(e => e.AppraisalType).HasColumnName("AP_DD_TYPE").HasMaxLength(10);
            entity.Property(e => e.CancelledByApproverId).HasColumnName("AP_CAN_APPRID");
            entity.Property(e => e.CancelledDate).HasColumnName("AP_CANCELDATE");
            entity.Property(e => e.HasSubordinates).HasColumnName("AP_SUBORDINATE");
            entity.Property(e => e.Salute).HasColumnName("DD_USR_SLT").HasMaxLength(4);
            entity.Property(e => e.FirstName).HasColumnName("DD_USR_FNM").HasMaxLength(65);
            entity.Property(e => e.MiddleName).HasColumnName("DD_USR_MNM").HasMaxLength(65);
            entity.Property(e => e.LastName).HasColumnName("DD_USR_LNM").HasMaxLength(65);
            entity.Property(e => e.Designation).HasColumnName("DD_USR_DSG").HasMaxLength(100);
            entity.Property(e => e.SignatoryName).HasColumnName("DD_CEO_NAM").HasMaxLength(150);
            entity.Property(e => e.SignatoryDesignation).HasColumnName("DD_CEO_DSG").HasMaxLength(100);
            entity.Property(e => e.FinalVtcRating).HasColumnName("DD_VTC_RAT").HasMaxLength(4);
            entity.Property(e => e.PromotionBand).HasColumnName("DD_PRM_BND");
            entity.Property(e => e.EmployeeType).HasColumnName("DD_EMP_TYP").HasMaxLength(3);
            entity.Property(e => e.PayrollStatus).HasColumnName("DD_PAYROLL");
            entity.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
            entity.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON");

            // Handle AppraisalStatus as owned type
            entity.OwnsOne(e => e.Status, status =>
            {
                status.Property(s => s.Code)
                    .HasColumnName("AP_STS_COD")
                    .HasMaxLength(1);
            });

            // Handle CompensationDetails as owned type
            entity.OwnsOne(e => e.Compensation, comp =>
            {
                comp.Property(c => c.BasicOld).HasColumnName("AP_BASIC_OLD");
                comp.Property(c => c.BasicNew).HasColumnName("AP_BASIC_NEW");
                comp.Property(c => c.CtcOld).HasColumnName("AP_CTC_OLD");
                comp.Property(c => c.CtcNew).HasColumnName("AP_CTC_NEW");
                comp.Property(c => c.IncrementAmount).HasColumnName("AP_INC_AMOUNT");
                comp.Property(c => c.EffectiveFrom).HasColumnName("AP_EFF_FROM");
            });

            // Handle BenefitsAvailability as owned type
            entity.OwnsOne(e => e.Benefits, ben =>
            {
                ben.Property(b => b.IsGratuityAvailable).HasColumnName("AP_BENF_GRAT");
                ben.Property(b => b.IsSuperannuationAvailable).HasColumnName("AP_BENF_SUPER");
                ben.Property(b => b.IsPfAvailable).HasColumnName("AP_BENF_PF");
                ben.Property(b => b.NewFlexipay).HasColumnName("AP_NEWFLEXIPAY");
            });

            // RequestNumber == Id (same column AP_REQ_NUM); ignore to avoid EF Core
            // trying to map it to a non-existent 'RequestNumber' column.
            entity.Ignore(e => e.RequestNumber);

            entity.HasMany(e => e.CompetencyAssessments)
                .WithOne(c => c.AppraisalMain)
                .HasForeignKey(c => c.AppraisalMainRequestNumber)
                .IsRequired(false);

            entity.ToTable("DD_APPRAISALMAIN");
        });

        // AppraisalDetails Configuration
        modelBuilder.Entity<AppraisalDetailsEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).HasColumnName("DD_PIN_NUM");
            entity.Property(e => e.Designation).HasColumnName("DD_USR_DSG").HasMaxLength(100);
            entity.Property(e => e.EmployeeType).HasColumnName("DD_EMP_TYP").HasMaxLength(3);
            entity.Property(e => e.IncrementAmount).HasColumnName("DD_INC_AMT");
            entity.Property(e => e.BulletinPercentage).HasColumnName("DD_BLT_PERCENT");
            entity.Property(e => e.PromotionLevel).HasColumnName("DD_PRMLEVEL");
            entity.Property(e => e.NewGrade).HasColumnName("DD_NEWGRADE");
            entity.Property(e => e.PromotionBand).HasColumnName("DD_PRM_BND");
            entity.Property(e => e.EmployeeGradeId).HasColumnName("DD_EMPGRADEID");
            entity.Property(e => e.EmployeeLevelId).HasColumnName("DD_EMPLEVELID");
            entity.Property(e => e.EmployeeUnitId).HasColumnName("DD_EMPUNITID");
            entity.Property(e => e.YearId).HasColumnName("DD_YEARID");
            entity.Property(e => e.IncrementTemplateId).HasColumnName("DD_INCTEMPLATEID");
            entity.Property(e => e.RateTemplateId).HasColumnName("DD_RATETEMPLATEID");
            entity.Property(e => e.LetterFile).HasColumnName("DD_LETFILE").HasMaxLength(30);
            entity.Property(e => e.ExperienceMonths).HasColumnName("DD_EXPMONTHS");
            entity.ToTable("DD_APPRAISALDETAILS");
        });

        // CompetencyAssessment Configuration
        modelBuilder.Entity<CompetencyAssessmentEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).HasColumnName("AP_REQ_NUM");
            entity.Property(e => e.CompetencyNumber).HasColumnName("AP_CPD_NUM");
            entity.Property(e => e.SerialNumber).HasColumnName("AP_ASS_SRL");
            entity.Property(e => e.AssessmentRating).HasColumnName("AP_ASS_RAT");
            entity.Property(e => e.CompetencyRating).HasColumnName("AP_COMP_RATING");
            entity.Property(e => e.Remarks).HasColumnName("AP_REM_MRK").HasMaxLength(1000);
            entity.Property(e => e.SelfDevelopment).HasColumnName("AP_SLF_DEV").HasMaxLength(4000);
            entity.Property(e => e.JobDevelopment).HasColumnName("AP_JOB_DEV").HasMaxLength(4000);
            entity.Property(e => e.TrainingDevelopment).HasColumnName("AP_TRG_DEV").HasMaxLength(4000);
            entity.Property(e => e.AppraiserUserCode).HasColumnName("AP_USR_COD").HasMaxLength(25);
            entity.Property(e => e.AppraiserUserNumber).HasColumnName("AP_REF_SRLNO");
            entity.Property(e => e.PinNumber).HasColumnName("AP_PIN_NUM");
            entity.Property(e => e.Role).HasColumnName("AP_ROLE").HasMaxLength(20);
            entity.Property(e => e.CancellationDate).HasColumnName("AP_CAN_DAT");
            entity.Property(e => e.CancellationRemarks).HasColumnName("AP_CAN_REM").HasMaxLength(4000);
            entity.ToTable("DD_APPRAISERASSESS");
        });

        // EmployeeGoal Configuration
        modelBuilder.Entity<EmployeeGoalEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestNumber).HasColumnName("AG_REQ_NUM");
            entity.Property(e => e.SerialNumber).HasColumnName("AG_SRL_NUM");
            entity.Property(e => e.PinNumber).HasColumnName("AG_PIN_NUM");
            entity.Property(e => e.UserId).HasColumnName("AG_USR_ID").HasMaxLength(50);
            entity.Property(e => e.PersonDesignation).HasColumnName("AG_PER_DES").HasMaxLength(4000);
            entity.Property(e => e.UnitFrom).HasColumnName("AG_UNT_FRM").HasMaxLength(20);
            entity.Property(e => e.UnitTo).HasColumnName("AG_UNT_TO").HasMaxLength(20);
            entity.Property(e => e.Weightage).HasColumnName("AG_WEIGHTAGE");
            entity.Property(e => e.AppraiseeRemark).HasColumnName("AG_APP_RMK").HasMaxLength(4000);
            entity.Property(e => e.Remark).HasColumnName("AG_CAN_RMK").HasMaxLength(4000);
            entity.Property(e => e.FinancialStartDate).HasColumnName("AG_FIN_STR");
            entity.Property(e => e.FinancialEndDate).HasColumnName("AG_FIN_END");
            entity.Property(e => e.Category).HasColumnName("AG_CATEGORY").HasMaxLength(100);
            entity.Property(e => e.UnitOfMeasure).HasColumnName("AG_UOM").HasMaxLength(65);
            entity.Property(e => e.Status).HasColumnName("AG_APS_STS").HasMaxLength(1);
            entity.Property(e => e.Achievements).HasColumnName("AG_ACH").HasMaxLength(4000);
            entity.Property(e => e.Difficulties).HasColumnName("AG_DIFF").HasMaxLength(4000);
            entity.Property(e => e.ModifiedSerialNumber).HasColumnName("AG_MOD_SRLNO");
            entity.Property(e => e.ExperienceCode).HasColumnName("AG_EXPCOD").HasMaxLength(3);
            entity.Property(e => e.GoalFlag).HasColumnName("AG_GOL_FLG").HasMaxLength(3);
            entity.Property(e => e.AccountabilityId).HasColumnName("AG_ACCOUNTABILITYID");
            entity.ToTable("DD_APPRAISEEGOAL_CUR");
        });
    }
}
