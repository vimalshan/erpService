using ContributionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContributionService.Infrastructure.Persistence;

public class ContributionDbContext : DbContext
{
    public ContributionDbContext(DbContextOptions<ContributionDbContext> options) : base(options) { }

    public DbSet<ContributionMain> ContributionMain => Set<ContributionMain>();
    public DbSet<ContributionDetail> ContributionDetails => Set<ContributionDetail>();
    public DbSet<ContributionBreakup> ContributionBreakups => Set<ContributionBreakup>();
    public DbSet<ContributionTemp> ContributionTemp => Set<ContributionTemp>();
    public DbSet<SuperannuationContribution> SuperannuationContributions => Set<SuperannuationContribution>();
    public DbSet<SuperannuationBatch> SuperannuationBatches => Set<SuperannuationBatch>();
    public DbSet<SuperannuationBreakup> SuperannuationBreakups => Set<SuperannuationBreakup>();
    public DbSet<SuperannuationRate> SuperannuationRates => Set<SuperannuationRate>();
    public DbSet<SuperannuationTrustName> SuperannuationTrustNames => Set<SuperannuationTrustName>();
    public DbSet<ContributionProcessLog> ContributionProcessLogs => Set<ContributionProcessLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ContributionMain
        modelBuilder.Entity<ContributionMain>(e =>
        {
            e.ToTable("CONTRIBUTION_MAIN");
            e.HasKey(x => x.ContributionBatchNo);
            e.Property(x => x.ContributionBatchNo).HasColumnName("CONTRIBUTION_BATCH_NO");
            e.Property(x => x.ContributionTrustCode).HasColumnName("CONTRIBUTION_TRUST_CODE").HasMaxLength(3).IsFixedLength();
            e.Property(x => x.ContributionCategory).HasColumnName("CONTRIBUTION_CATEGORY").HasMaxLength(3).IsFixedLength();
            e.Property(x => x.ContributionPayunitCode).HasColumnName("CONTRIBUTION_PAYUNIT_CODE").HasMaxLength(3).IsFixedLength();
            e.Property(x => x.ContributionPayMonthStart).HasColumnName("CONTRIBUTION_PAY_MONTHSTART").HasPrecision(3);
            e.Property(x => x.ContributionPayMonthEnd).HasColumnName("CONTRIBUTION_PAY_MONTHEND").HasPrecision(3);
            e.Property(x => x.ContributionStatus).HasColumnName("CONTRIBUTION_STATUS").HasMaxLength(2).IsFixedLength();
            e.Property(x => x.ContributionJvNo).HasColumnName("CONTRIBUTION_JVNO").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionRecActranNo).HasColumnName("CONTRIBUTION_REC_ACTRAN_NO").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionEntOn).HasColumnName("CONTRIBUTION_ENT_ON").HasPrecision(3);
            e.Property(x => x.ContributionRefNo).HasColumnName("CONTRIBUTION_REFNO");
            e.Ignore(x => x.Details);
            e.Ignore(x => x.DomainEvents);
        });

        // ContributionDetail
        modelBuilder.Entity<ContributionDetail>(e =>
        {
            e.ToTable("CONTRIBUTION_DETAILS");
            e.HasKey(x => x.ContributionId);
            e.Property(x => x.ContributionBatchNo).HasColumnName("CONTRIBUTION_BATCH_NO").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionId).HasColumnName("CONTRIBUTION_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionMemberNo).HasColumnName("CONTRIBUTION_MEMBER_NO").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionUnitCode).HasColumnName("CONTRIBUTION_UNIT_CODE").HasMaxLength(1).IsFixedLength();
            e.Property(x => x.ContributionEmployeeNo).HasColumnName("CONTRIBUTION_EMPLOYEE_NO").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionReferenceNo).HasColumnName("CONTRIBUTION_REFERENCE_NO").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionReferenceRemarks).HasColumnName("CONTRIBUTION_REFERENCE_REMARKS").HasMaxLength(255);
            e.Property(x => x.ContributionBasicAmount).HasColumnName("CONTRIBUTION_BASIC_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionFpsBasicAmount).HasColumnName("CONTRIBUTION_FPSBASIC_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionEeAmount).HasColumnName("CONTRIBUTION_EE_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionErAmount).HasColumnName("CONTRIBUTION_ER_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionVeAmount).HasColumnName("CONTRIBUTION_VE_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionFpAmount).HasColumnName("CONTRIBUTION_FP_AMOUNT").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionLoanPrincipal).HasColumnName("CONTRIBUTION_LOAN_PRINCIPAL").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionLoanInterest).HasColumnName("CONTRIBUTION_LOAN_INTEREST").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionEntByUserId).HasColumnName("CONTRIBUTION_ENT_BY_USER_ID").HasMaxLength(255);
            e.Property(x => x.ContributionEntEmpSysId).HasColumnName("CONTRIBUTION_ENT_EMP_SYS_ID").HasColumnType("decimal(38,0)");
            e.Property(x => x.ContributionEntOn).HasColumnName("CONTRIBUTION_ENT_ON").HasPrecision(3);
            e.Property(x => x.ContributionTypeCode).HasColumnName("CONTRIBUTION_TYPE_CODE").HasMaxLength(1).IsFixedLength();
            e.Property(x => x.ContributionEmpSysId).HasColumnName("CONTRIBUTION_EMP_SYSID").HasColumnType("decimal(38,0)");
            e.Ignore(x => x.Batch);
            e.Ignore(x => x.Breakups);
            e.Ignore(x => x.DomainEvents);
        });

        // ContributionBreakup
        modelBuilder.Entity<ContributionBreakup>(e =>
        {
            e.ToTable("CONTRIBUTION_BREAKUP");
            e.HasKey(x => new { x.ContributionPayTranNo, x.ContributionBatchNo, x.ContributionId });
            e.Property(x => x.ContributionBatchNo).HasColumnName("CONTRIBUTION_BATCH_NO");
            e.Property(x => x.ContributionId).HasColumnName("CONTRIBUTION_ID");
            e.Property(x => x.ContributionPayTranNo).HasColumnName("CONTRIBUTION_PAYTRANNO");
            e.Property(x => x.ContributionEdCode).HasColumnName("CONTRIBUTION_EDCODE").HasMaxLength(6).IsFixedLength();
            e.Property(x => x.ContributionPayAmount).HasColumnName("CONTRIBUTION_PAYAMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionEeAmount).HasColumnName("CONTRIBUTION_EEAMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionErAmount).HasColumnName("CONTRIBUTION_ERAMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionComCod).HasColumnName("CONTRIBUTION_COM_COD").HasMaxLength(3).IsFixedLength();
        });

        // ContributionTemp
        modelBuilder.Entity<ContributionTemp>(e =>
        {
            e.ToTable("CONTRIBUTION_TEMP");
            e.HasKey(x => x.ContributionId);
            e.Property(x => x.ContributionBatchNo).HasColumnName("CONTRIBUTION_BATCH_NO");
            e.Property(x => x.ContributionId).HasColumnName("CONTRIBUTION_ID");
            e.Property(x => x.ContributionMemberNo).HasColumnName("CONTRIBUTION_MEMBER_NO");
            e.Property(x => x.ContributionUnitCode).HasColumnName("CONTRIBUTION_UNIT_CODE").HasMaxLength(3).IsFixedLength();
            e.Property(x => x.ContributionEmployeeNo).HasColumnName("CONTRIBUTION_EMPLOYEE_NO");
            e.Property(x => x.ContributionReferenceNo).HasColumnName("CONTRIBUTION_REFERENCE_NO");
            e.Property(x => x.ContributionReferenceRemarks).HasColumnName("CONTRIBUTION_REFERENCE_REMARKS").HasMaxLength(200);
            e.Property(x => x.ContributionBasicAmount).HasColumnName("CONTRIBUTION_BASIC_AMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionFpsBasicAmount).HasColumnName("CONTRIBUTION_FPSBASIC_AMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionEeAmount).HasColumnName("CONTRIBUTION_EE_AMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionErAmount).HasColumnName("CONTRIBUTION_ER_AMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionVeAmount).HasColumnName("CONTRIBUTION_VE_AMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionFpAmount).HasColumnName("CONTRIBUTION_FP_AMOUNT").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionLoanPrincipal).HasColumnName("CONTRIBUTION_LOAN_PRINCIPAL").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionLoanInterest).HasColumnName("CONTRIBUTION_LOAN_INTEREST").HasColumnType("decimal(19,0)");
            e.Property(x => x.ContributionEntByUserId).HasColumnName("CONTRIBUTION_ENT_BY_USER_ID").HasMaxLength(25);
            e.Property(x => x.ContributionEntEmpSysId).HasColumnName("CONTRIBUTION_ENT_EMP_SYS_ID");
            e.Property(x => x.ContributionEntOn).HasColumnName("CONTRIBUTION_ENT_ON").HasPrecision(3);
            e.Property(x => x.ContributionTypeCode).HasColumnName("CONTRIBUTION_TYPE_CODE").HasMaxLength(1).IsFixedLength();
        });

        // SuperannuationContribution
        modelBuilder.Entity<SuperannuationContribution>(e =>
        {
            e.ToTable("SUPERANN_CONTRIBUTION");
            e.HasKey(x => x.SnSlrNum);
            e.Property(x => x.SnSlrNum).HasColumnName("SN_SLR_NUM");
            e.Property(x => x.SnFinYer).HasColumnName("SN_FIN_YER");
            e.Property(x => x.SnPinNum).HasColumnName("SN_PIN_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.SnEmpNam).HasColumnName("SN_EMP_NAM").HasMaxLength(100);
            e.Property(x => x.SnFudNum).HasColumnName("SN_FUD_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.SnConDat).HasColumnName("SN_CON_DAT").HasPrecision(3);
            e.Property(x => x.SnUntNos).HasColumnName("SN_UNT_NOS").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnNavAmt).HasColumnName("SN_NAV_AMT").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnConAmt).HasColumnName("SN_CON_AMT").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnConTyp).HasColumnName("SN_CON_TYP").HasMaxLength(1).IsFixedLength();
            e.Property(x => x.SnEntDat).HasColumnName("SN_ENT_DAT").HasPrecision(3);
            e.Ignore(x => x.DomainEvents);
        });

        // SuperannuationBatch
        modelBuilder.Entity<SuperannuationBatch>(e =>
        {
            e.ToTable("SUPERANN_BATCH");
            e.HasKey(x => x.SnBatchNo);
            e.Property(x => x.SnBatchNo).HasColumnName("SN_BATCH_NO");
            e.Property(x => x.SnTrustCode).HasColumnName("SN_TRUST_CODE");
            e.Property(x => x.SnCategory).HasColumnName("SN_CATEGORY").HasMaxLength(3).IsFixedLength();
            e.Property(x => x.SnPayunitCode).HasColumnName("SN_PAYUNIT_CODE").HasMaxLength(3).IsFixedLength();
            e.Property(x => x.SnPayMonthStart).HasColumnName("SN_PAY_MONTHSTART").HasMaxLength(255);
            e.Property(x => x.SnPayMonthEnd).HasColumnName("SN_PAY_MONTHEND").HasPrecision(3);
            e.Property(x => x.SnStatus).HasColumnName("SN_STATUS").HasMaxLength(1).IsFixedLength();
            e.Property(x => x.SnEntOn).HasColumnName("SN_ENT_ON").HasMaxLength(255);
            e.Property(x => x.SnConAmt).HasColumnName("SN_CON_AMT").HasMaxLength(255);
            e.Property(x => x.SnPayDate).HasColumnName("SN_PAY_DATE").HasPrecision(3);
            e.Ignore(x => x.DomainEvents);
        });

        // SuperannuationBreakup (no PK in SQL - use shadow property)
        modelBuilder.Entity<SuperannuationBreakup>(e =>
        {
            e.ToTable("SUPERANN_BREAKUP");
            e.HasNoKey();
            e.Property(x => x.SnFinYer).HasColumnName("SN_FIN_YER");
            e.Property(x => x.SnPinNum).HasColumnName("SN_PIN_NUM");
            e.Property(x => x.SnEmpNam).HasColumnName("SN_EMP_NAM").HasMaxLength(100);
            e.Property(x => x.SnFudNum).HasColumnName("SN_FUD_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.SnConDat).HasColumnName("SN_CON_DAT").HasPrecision(3);
            e.Property(x => x.SnTrsAmt).HasColumnName("SN_TRS_AMT").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnExgAmt).HasColumnName("SN_EXG_AMT").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnConTyp).HasColumnName("SN_CON_TYP").HasMaxLength(1).IsFixedLength();
            e.Property(x => x.SnEntDat).HasColumnName("SN_ENT_DAT").HasPrecision(3);
            e.Property(x => x.SnBatNo).HasColumnName("SN_BAT_NO");
            e.Property(x => x.SnGrsAmt).HasColumnName("SN_GRS_AMT").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnActAmt).HasColumnName("SN_ACT_AMT").HasColumnType("decimal(19,0)");
            e.Property(x => x.SnPayAmt).HasColumnName("SN_PAY_AMT").HasColumnType("decimal(19,0)");
        });

        // SuperannuationRate (no PK in SQL)
        modelBuilder.Entity<SuperannuationRate>(e =>
        {
            e.ToTable("SUPERANN_RATE");
            e.HasNoKey();
            e.Property(x => x.SnFudNum).HasColumnName("SN_FUD_NUM");
            e.Property(x => x.SnMonth).HasColumnName("SN_MONTH").HasPrecision(3);
            e.Property(x => x.SnRate).HasColumnName("SN_RATE").HasColumnType("decimal(19,0)");
        });

        // SuperannuationTrustName
        modelBuilder.Entity<SuperannuationTrustName>(e =>
        {
            e.ToTable("SUPERANN_TRUSTNAME");
            e.HasKey(x => x.StFndNum);
            e.Property(x => x.StFndNum).HasColumnName("ST_FND_NUM").HasColumnType("decimal(38,0)");
            e.Property(x => x.StFndNam).HasColumnName("ST_FND_NAM").HasMaxLength(100);
        });

        // ContributionProcessLog
        modelBuilder.Entity<ContributionProcessLog>(e =>
        {
            e.ToTable("CONTRIBUTION_PROCESS_LOG");
            e.HasKey(x => x.LogId);
            e.Property(x => x.LogId).HasColumnName("LOG_ID").UseIdentityColumn();
            e.Property(x => x.LogType).HasColumnName("LOG_TYPE").HasMaxLength(20);
            e.Property(x => x.LogMessage).HasColumnName("LOG_MESSAGE");
            e.Property(x => x.ProcessDate).HasColumnName("PROCESS_DATE").HasPrecision(3);
            e.Property(x => x.UserId).HasColumnName("USER_ID");
        });
    }
}
