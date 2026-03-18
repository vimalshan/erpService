using DealTicketing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DealTicketing.Infrastructure.Persistence.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("DEAL_BANKMASTER");
        builder.HasKey(b => b.BankId);
        builder.Property(b => b.BankId).HasColumnName("BANK_ID");
        builder.Property(b => b.BankName).HasColumnName("BANK_NAME").HasMaxLength(50).IsRequired();
        builder.Property(b => b.BankEffDate).HasColumnName("BANK_EFFDATE");
        builder.Property(b => b.BankClsDate).HasColumnName("BANK_CLSDATE");
        builder.Property(b => b.BankModifiedBy).HasColumnName("BANK_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(b => b.BankModifiedOn).HasColumnName("BANK_MODIFIEDON");
        builder.Ignore(b => b.DomainEvents);
    }
}

public class CategoryMasterConfiguration : IEntityTypeConfiguration<CategoryMaster>
{
    public void Configure(EntityTypeBuilder<CategoryMaster> builder)
    {
        builder.ToTable("DEAL_CATEGORYMASTER");
        builder.HasKey(c => c.CategoryId);
        builder.Property(c => c.CategoryId).HasColumnName("CATEGORY_ID");
        builder.Property(c => c.CategoryName).HasColumnName("CATEGORY_NAME").HasMaxLength(50).IsRequired();
        builder.Property(c => c.CategoryType).HasColumnName("CATEGORY_TYPE").HasColumnType("char(1)");
        builder.Property(c => c.CategoryModifiedOn).HasColumnName("CATEGORY_MODIFIEDON");
        builder.Property(c => c.CategoryModifiedBy).HasColumnName("CATEGORY_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Ignore(c => c.DomainEvents);
    }
}

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("DEAL_LOVMASTER");
        builder.HasKey(l => l.LovId);
        builder.Property(l => l.LovId).HasColumnName("LOV_ID");
        builder.Property(l => l.LovType).HasColumnName("LOV_TYPE").HasMaxLength(10).IsRequired();
        builder.Property(l => l.LovName).HasColumnName("LOV_NAME").HasMaxLength(150).IsRequired();
        builder.Ignore(l => l.DomainEvents);
    }
}

public class DealBatchConfiguration : IEntityTypeConfiguration<DealBatch>
{
    public void Configure(EntityTypeBuilder<DealBatch> builder)
    {
        builder.ToTable("DEALTICKET_BATCH");
        builder.HasKey(b => b.DealBatchId);
        builder.Property(b => b.DealBatchId).HasColumnName("DEAL_BATCHID");
        builder.Property(b => b.DealDate).HasColumnName("DEAL_DATE");
        builder.Property(b => b.DealDerType).HasColumnName("DEAL_DERTYPE");
        builder.Property(b => b.DealScreenshot).HasColumnName("DEAL_SCREENSHOT").HasMaxLength(50);
        builder.Property(b => b.DealBookedBy).HasColumnName("DEAL_BOOKEDBY");
        builder.Property(b => b.DealBankTrader).HasColumnName("DEAL_BANKTRADER").HasMaxLength(50);
        builder.Property(b => b.DealBankId).HasColumnName("DEAL_BANKID");
        builder.Property(b => b.DealOptionType).HasColumnName("DEAL_OPTIONTYPE");
        builder.Property(b => b.DealBusinessId).HasColumnName("DEAL_BUSINESSID").HasColumnType("decimal(38,0)");
        builder.Property(b => b.DealRejStatus).HasColumnName("DEAL_REJSTATUS").HasColumnType("char(1)");
        builder.Property(b => b.DealRejReason).HasColumnName("DEAL_REJREASON").HasMaxLength(50);
        builder.Property(b => b.DealErrRemarks).HasColumnName("DEAL_ERRREMARKS").HasMaxLength(50);
        builder.Property(b => b.DealModifiedBy).HasColumnName("DEAL_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(b => b.DealModifiedOn).HasColumnName("DEAL_MODIFIEDON");
        builder.Property(b => b.DealUnitId).HasColumnName("DEAL_UNITID").HasColumnType("decimal(38,0)");

        builder.HasOne(b => b.Bank)
            .WithMany(bk => bk.DealBatches)
            .HasForeignKey(b => b.DealBankId)
            .HasConstraintName("FK_DEALTICKET_BATCH_BANKMASTER");

        builder.HasIndex(b => b.DealDate).HasDatabaseName("IX_DEALTICKET_BATCH_DATE");
        builder.HasIndex(b => b.DealBankId).HasDatabaseName("IX_DEALTICKET_BATCH_BANKID");

        builder.Ignore(b => b.DomainEvents);
    }
}

public class DealDetailConfiguration : IEntityTypeConfiguration<DealDetail>
{
    public void Configure(EntityTypeBuilder<DealDetail> builder)
    {
        builder.ToTable("DEALTICKET_DET");
        builder.HasKey(d => d.DealId);
        builder.Property(d => d.DealId).HasColumnName("DEAL_ID");
        builder.Property(d => d.DealNo).HasColumnName("DEAL_NO");
        builder.Property(d => d.DealVersionId).HasColumnName("DEAL_VERSIONID");
        builder.Property(d => d.DealBatchId).HasColumnName("DEAL_BATCHID");
        builder.Property(d => d.DealTranType).HasColumnName("DEAL_TRANTYPE").HasColumnType("char(1)");
        builder.Property(d => d.DealPosition).HasColumnName("DEAL_POSITION").HasColumnType("char(2)");
        builder.Property(d => d.DealEntryDate).HasColumnName("DEAL_ENTRYDATE");
        builder.Property(d => d.DealAmount).HasColumnName("DEAL_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealBankId).HasColumnName("DEAL_BANKID");
        builder.Property(d => d.DealCurrency1).HasColumnName("DEAL_CURRENCY1");
        builder.Property(d => d.DealCurrency2).HasColumnName("DEAL_CURRENCY2");
        builder.Property(d => d.DealSpotRate).HasColumnName("DEAL_SPOTRATE").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealForPoints).HasColumnName("DEAL_FORPOINTS").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealBankMargin).HasColumnName("DEAL_BANKMARGIN").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealBookRate).HasColumnName("DEAL_BOOKRATE").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealMatDate).HasColumnName("DEAL_MATDATE");
        builder.Property(d => d.DealDealType).HasColumnName("DEAL_DEALTYPE");
        builder.Property(d => d.DealBusiness).HasColumnName("DEAL_BUSINESS");
        builder.Property(d => d.DealCategory).HasColumnName("DEAL_CATEGORY");
        builder.Property(d => d.DealStrikePrice).HasColumnName("DEAL_STRIKEPRICE");
        builder.Property(d => d.DealPplMitOut).HasColumnName("DEAL_PPLMITOUT");
        builder.Property(d => d.DealAppStatus).HasColumnName("DEAL_APPSTATUS").HasColumnType("char(1)");
        builder.Property(d => d.DealAppRemarks).HasColumnName("DEAL_APPREMARKS").HasMaxLength(200);
        builder.Property(d => d.DealErrRemarks).HasColumnName("DEAL_ERRREMARKS").HasMaxLength(200);
        builder.Property(d => d.DealCorrectness).HasColumnName("DEAL_CORRECTNESS").HasMaxLength(200);
        builder.Property(d => d.DealSigned).HasColumnName("DEAL_SIGNED").HasColumnType("char(1)");
        builder.Property(d => d.DealAppBusiness).HasColumnName("DEAL_APPBUSINESS");
        builder.Property(d => d.DealDealConfNo).HasColumnName("DEAL_DEALCONFNO").HasMaxLength(50);
        builder.Property(d => d.DealModifiedOn).HasColumnName("DEAL_MODIFIEDON");
        builder.Property(d => d.DealModifiedBy).HasColumnName("DEAL_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealRemarks).HasColumnName("DEAL_REMARKS").HasMaxLength(500);
        builder.Property(d => d.DealIrLoan).HasColumnName("DEAL_IRLOAN").HasMaxLength(500);
        builder.Property(d => d.DealIrType).HasColumnName("DEAL_IRTYPE").HasColumnType("char(3)");
        builder.Property(d => d.DealStartDate).HasColumnName("DEAL_STARTDATE");
        builder.Property(d => d.DealNotPrincipal).HasColumnName("DEAL_NOTPRINCIPAL").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealIrsType).HasColumnName("DEAL_IRSTYPE").HasMaxLength(25);
        builder.Property(d => d.DealToPay).HasColumnName("DEAL_TOPAY").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealToRec).HasColumnName("DEAL_TOREC").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealRateScreenshot).HasColumnName("DEAL_RATESCREENSHOT").HasMaxLength(500);
        builder.Property(d => d.DealRatePer).HasColumnName("DEAL_RATEPER");
        builder.Property(d => d.DealLoanAmt).HasColumnName("DEAL_LOANAMT").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealLoanCurrency).HasColumnName("DEAL_LOANCURRENCY");
        builder.Property(d => d.DealSetAmt).HasColumnName("DEAL_SETAMT").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealCanAmt).HasColumnName("DEAL_CANAMT").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealRollAmt).HasColumnName("DEAL_ROLLAMT").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealSetStatus).HasColumnName("DEAL_SETSTATUS").HasColumnType("char(1)");
        builder.Property(d => d.DealUnitId).HasColumnName("DEAL_UNITID").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealNetBasisPoint).HasColumnName("DEAL_NETBASISPOINT").HasColumnType("decimal(19,0)");
        builder.Property(d => d.DealRolloverDealNo).HasColumnName("DEAL_ROLLOVERDEALNO").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealBookingCharges).HasColumnName("DEAL_BOOKINGCHARGES").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DealSentToBank).HasColumnName("DEAL_SENTOBANK").HasColumnType("char(1)");

        builder.HasOne(d => d.DealBatch)
            .WithMany(b => b.DealDetails)
            .HasForeignKey(d => d.DealBatchId)
            .HasConstraintName("FK_DEALTICKET_DET_BATCH");

        builder.HasOne(d => d.Bank)
            .WithMany(bk => bk.DealDetails)
            .HasForeignKey(d => d.DealBankId)
            .HasConstraintName("FK_DEALTICKET_DET_BANKMASTER");

        builder.HasIndex(d => d.DealBatchId).HasDatabaseName("IX_DEALTICKET_DET_BATCHID");
        builder.HasIndex(d => d.DealId).HasDatabaseName("IX_DEALTICKET_DET_DEALID");

        builder.Ignore(d => d.DomainEvents);
    }
}

public class DealLoanScheduleConfiguration : IEntityTypeConfiguration<DealLoanSchedule>
{
    public void Configure(EntityTypeBuilder<DealLoanSchedule> builder)
    {
        builder.ToTable("DEALTICKET_LOANSCH");
        builder.HasKey(l => l.DealSchId);
        builder.Property(l => l.DealSchId).HasColumnName("DEAL_SCHID");
        builder.Property(l => l.DealId).HasColumnName("DEAL_ID");
        builder.Property(l => l.DealSchDate).HasColumnName("DEAL_SCHDATE");
        builder.Property(l => l.DealSchAmt).HasColumnName("DEAL_SCHAMT");

        builder.HasOne(l => l.DealDetail)
            .WithMany(d => d.LoanSchedules)
            .HasForeignKey(l => l.DealId)
            .HasConstraintName("FK_DEALTICKET_LOANSCH_DET");

        builder.Ignore(l => l.DomainEvents);
    }
}

public class DealSettlementConfiguration : IEntityTypeConfiguration<DealSettlement>
{
    public void Configure(EntityTypeBuilder<DealSettlement> builder)
    {
        builder.ToTable("DEALTICKET_SET");
        builder.HasKey(s => s.SetId);
        builder.Property(s => s.SetId).HasColumnName("SET_ID");
        builder.Property(s => s.SetDealId).HasColumnName("SET_DEALID");
        builder.Property(s => s.SetSpotRate).HasColumnName("SET_SPOTRATE").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetDate).HasColumnName("SET_DATE");
        builder.Property(s => s.SetMoneyType).HasColumnName("SET_MONEYTYPE").HasColumnType("char(3)");
        builder.Property(s => s.SetExcType).HasColumnName("SET_EXCTYPE").HasColumnType("char(1)");
        builder.Property(s => s.SetGainLossAmt).HasColumnName("SET_GAINLOSSAMT").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetType).HasColumnName("SET_TYPE").HasColumnType("char(3)");
        builder.Property(s => s.SetCanDate).HasColumnName("SET_CANDATE");
        builder.Property(s => s.SetPremiumRate).HasColumnName("SET_PREMIUMRATE").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetPremiumAmount).HasColumnName("SET_PREMIUMAMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetIrDays).HasColumnName("SET_IRDAYS");
        builder.Property(s => s.SetIrStartDate).HasColumnName("SET_IRSTARTDATE");
        builder.Property(s => s.SetIrAmount).HasColumnName("SET_IRAMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetWindFee).HasColumnName("SET_WINDFEE").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetWindRate).HasColumnName("SET_WINDRATE").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetAmount).HasColumnName("SET_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetCreditDebit).HasColumnName("SET_CREDITDEBIT").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetModifiedBy).HasColumnName("SET_MODIFIEDBY");
        builder.Property(s => s.SetModifiedOn).HasColumnName("SET_MODIFIEDON");
        builder.Property(s => s.SetExchangeRate).HasColumnName("SET_EXCHANGERATE").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetActGainLossAmt).HasColumnName("SET_ACTGAINLOSSAMT").HasColumnType("decimal(19,0)");
        builder.Property(s => s.SetDcDate).HasColumnName("SET_DCDATE");
        builder.Property(s => s.SetDcAmnt).HasColumnName("SET_DCAMNT").HasColumnType("decimal(38,0)");
        builder.Property(s => s.SetBankName).HasColumnName("SET_BANKNAME").HasMaxLength(100);
        builder.Property(s => s.SetBankAcNo).HasColumnName("SET_BANKACNO").HasMaxLength(20);

        builder.HasOne(s => s.DealDetail)
            .WithMany(d => d.Settlements)
            .HasForeignKey(s => s.SetDealId)
            .HasConstraintName("FK_DEALTICKET_SET_DET");

        builder.HasIndex(s => s.SetDealId).HasDatabaseName("IX_DEALTICKET_SET_DEALID");
        builder.Ignore(s => s.DomainEvents);
    }
}

public class DealAttachmentConfiguration : IEntityTypeConfiguration<DealAttachment>
{
    public void Configure(EntityTypeBuilder<DealAttachment> builder)
    {
        builder.ToTable("DEATICKET_ATTACHMENT");
        builder.HasKey(a => a.DealAttachmentId);
        builder.Property(a => a.DealAttachmentId).HasColumnName("DEAL_ATTACHMENTID");
        builder.Property(a => a.DealBatchId).HasColumnName("DEAL_BATCHID");
        builder.Property(a => a.DealId).HasColumnName("DEAL_ID");
        builder.Property(a => a.DealAttachmentType).HasColumnName("DEAL_ATTACHMENTTYPE").HasMaxLength(10);
        builder.Property(a => a.DealAttachmentFile).HasColumnName("DEAL_ATTACHMENTFILE").HasMaxLength(200);

        builder.HasOne(a => a.DealDetail)
            .WithMany(d => d.Attachments)
            .HasForeignKey(a => a.DealId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_DEATICKET_ATTACHMENT_DET");

        builder.HasOne(a => a.DealBatch)
            .WithMany()
            .HasForeignKey(a => a.DealBatchId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_DEATICKET_ATTACHMENT_BATCH");

        builder.HasIndex(a => a.DealId).HasDatabaseName("IX_DEATICKET_ATTACHMENT_DEALID");
        builder.Ignore(a => a.DomainEvents);
    }
}

public class DealSettlementAttachmentConfiguration : IEntityTypeConfiguration<DealSettlementAttachment>
{
    public void Configure(EntityTypeBuilder<DealSettlementAttachment> builder)
    {
        builder.ToTable("DEATICKETSET_ATTACHMENT");
        builder.HasKey(a => a.DealAttachmentId);
        builder.Property(a => a.DealAttachmentId).HasColumnName("DEAL_ATTACHMENTID");
        builder.Property(a => a.DealSetId).HasColumnName("DEAL_SETID");
        builder.Property(a => a.DealAttachmentType).HasColumnName("DEAL_ATTACHMENTTYPE").HasMaxLength(10);
        builder.Property(a => a.DealAttachmentFile).HasColumnName("DEAL_ATTACHMENTFILE").HasMaxLength(200);

        builder.HasOne(a => a.DealSettlement)
            .WithMany(s => s.Attachments)
            .HasForeignKey(a => a.DealSetId)
            .HasConstraintName("FK_DEATICKETSET_ATTACHMENT_SET");

        builder.Ignore(a => a.DomainEvents);
    }
}
