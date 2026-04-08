using InvestmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentService.Infrastructure.Data.Configurations;

public class InvestmentConfiguration : IEntityTypeConfiguration<Investment>
{
    public void Configure(EntityTypeBuilder<Investment> builder)
    {
        builder.ToTable("INV_MAIN");
        builder.HasKey(e => e.InvNo);
        builder.Property(e => e.InvNo).HasColumnName("INV_NO").ValueGeneratedNever();
        builder.Property(e => e.GroupId).HasColumnName("INV_GRPID");
        builder.Property(e => e.CategoryId).HasColumnName("INV_CATID");
        builder.Property(e => e.SubCategoryId).HasColumnName("INV_SUBCATID");
        builder.Property(e => e.Tenure).HasColumnName("INV_TENURE");
        builder.Property(e => e.TenureDays).HasColumnName("INV_TENUREDAYS");
        builder.Property(e => e.InterestOption).HasColumnName("INV_INTOPTION").HasMaxLength(2);
        builder.Property(e => e.OriginalPurchaseDate).HasColumnName("INV_ORGPURDATE");
        builder.Property(e => e.MaturityDate).HasColumnName("INV_MATDATE");
        builder.Property(e => e.CallPutOption).HasColumnName("INV_CALLPUTOPTION").HasMaxLength(1);
        builder.Property(e => e.PurchaseDate).HasColumnName("INV_PURDATE");
        builder.Property(e => e.CallPercentage).HasColumnName("INV_CALLPER").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Units).HasColumnName("INV_UNITS").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PurchaseRate).HasColumnName("INV_PURRATE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.FaceValue).HasColumnName("INV_FACEVALUE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Premium).HasColumnName("INV_PREMIUM").HasColumnType("decimal(19,0)");
        builder.Property(e => e.IssuedInterestRate).HasColumnName("INV_ISSINTRATE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.RevisedInterestFrom).HasColumnName("INV_REVINTFROM");
        builder.Property(e => e.RevisedInterestRate).HasColumnName("INV_REVINTRATE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.InterestDenomination).HasColumnName("INV_INTDENOM").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PurchaseValue).HasColumnName("INV_PURVALUE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SecondaryMarket).HasColumnName("INV_SECMARKET").HasMaxLength(1);
        builder.Property(e => e.BrokerId).HasColumnName("INV_BROKERID");
        builder.Property(e => e.CumulativeInterestFrom).HasColumnName("INV_CUMINTFROM");
        builder.Property(e => e.CumulativeInterestTo).HasColumnName("INV_CUMINTTO");
        builder.Property(e => e.CumulativeInterestAmount).HasColumnName("INV_CUMINTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.CumulativeInterestDays).HasColumnName("INV_CUMINTDYS");
        builder.Property(e => e.CreditAgency1).HasColumnName("INV_CRAGENCY1");
        builder.Property(e => e.CreditAgency2).HasColumnName("INV_CRAGENCY2");
        builder.Property(e => e.Rating1).HasColumnName("INV_RATING1");
        builder.Property(e => e.Rating2).HasColumnName("INV_RATING2");
        builder.Property(e => e.ClientId).HasColumnName("INV_CLIENTID").HasMaxLength(250);
        builder.Property(e => e.InterestFrequency).HasColumnName("INV_INTFREQUENCY").HasMaxLength(1);
        builder.Property(e => e.PaymentMode).HasColumnName("INV_PAYMODE").HasMaxLength(1);
        builder.Property(e => e.InterestDates).HasColumnName("INV_INTDATES").HasMaxLength(250);
        builder.Property(e => e.BankId).HasColumnName("INV_BANKID");
        builder.Property(e => e.ChequeNumber).HasColumnName("INV_CHQNUM").HasMaxLength(20);
        builder.Property(e => e.ChequeDate).HasColumnName("INV_CHQDATE");
        builder.Property(e => e.BankCharges).HasColumnName("INV_BANKCHARGES").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Status).HasColumnName("INV_STATUS").HasMaxLength(1);
        builder.Property(e => e.CertificateNumber).HasColumnName("INV_CERTNO").HasMaxLength(50);
        builder.Property(e => e.EnteredBy).HasColumnName("INV_ENTEREDBY");
        builder.Property(e => e.EnteredOn).HasColumnName("INV_ENTEREDON");
        builder.Property(e => e.LastModBy).HasColumnName("INV_LASTMODBY");
        builder.Property(e => e.LastModOn).HasColumnName("INV_LASTMODON");
        builder.Property(e => e.LastScheduleDate).HasColumnName("INV_LASTSCHDATE");
        builder.Property(e => e.YtmRate).HasColumnName("INV_YTMRATE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.NetValue).HasColumnName("INV_NETVAL").HasColumnType("decimal(19,0)");

        builder.HasOne(e => e.Category).WithMany(c => c.Investments).HasForeignKey(e => e.CategoryId);
        builder.HasOne(e => e.SubCategory).WithMany(s => s.Investments).HasForeignKey(e => e.SubCategoryId);
        builder.HasOne(e => e.Broker).WithMany(b => b.Investments).HasForeignKey(e => e.BrokerId);

        builder.HasIndex(e => e.Status).HasDatabaseName("IDX_INV_MAIN_STATUS");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
{
    public void Configure(EntityTypeBuilder<SaleDetail> builder)
    {
        builder.ToTable("INV_SALEDET");
        builder.HasKey(e => e.SaleNo);
        builder.Property(e => e.SaleNo).HasColumnName("INV_SALENO").ValueGeneratedNever();
        builder.Property(e => e.InvNo).HasColumnName("INV_NO");
        builder.Property(e => e.SaleType).HasColumnName("INV_SALETYPE").HasMaxLength(1);
        builder.Property(e => e.SaleDate).HasColumnName("INV_SALEDATE");
        builder.Property(e => e.InterestAdjusted).HasColumnName("INV_INTADJUSTED").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SalePremium).HasColumnName("INV_SALPREMIUM").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SaleValue).HasColumnName("INV_SALVALUE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SaleTransactionId).HasColumnName("INV_SALTRANID");
        builder.Property(e => e.Remarks).HasColumnName("INV_SALREMARKS").HasMaxLength(200);
        builder.Property(e => e.EnteredBy).HasColumnName("INV_ENTEREDBY");
        builder.Property(e => e.EnteredOn).HasColumnName("INV_ENTEREDON");
        builder.Property(e => e.LastModBy).HasColumnName("INV_LASTMODBY");
        builder.Property(e => e.LastModOn).HasColumnName("INV_LASTMODON");

        builder.HasOne(e => e.Investment).WithMany(i => i.SaleDetails).HasForeignKey(e => e.InvNo);
        builder.HasIndex(e => e.InvNo).HasDatabaseName("IDX_INV_SALEDET_INVNO");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ScheduleDetailConfiguration : IEntityTypeConfiguration<ScheduleDetail>
{
    public void Configure(EntityTypeBuilder<ScheduleDetail> builder)
    {
        builder.ToTable("INV_SCHDET");
        builder.HasKey(e => e.SchId);
        builder.Property(e => e.SchId).HasColumnName("SCH_ID").ValueGeneratedNever();
        builder.Property(e => e.InvNo).HasColumnName("SCH_INVNO");
        builder.Property(e => e.SlId).HasColumnName("SCH_SLID");
        builder.Property(e => e.ScheduleType).HasColumnName("SCH_TYPE").HasMaxLength(3);
        builder.Property(e => e.InterestFrom).HasColumnName("SCH_INTFROM");
        builder.Property(e => e.InterestTo).HasColumnName("SCH_INTTO");
        builder.Property(e => e.InterestOption).HasColumnName("SCH_INTOPTION").HasColumnType("decimal(19,0)");
        builder.Property(e => e.DueAmount).HasColumnName("SCH_DUEAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.DueDate).HasColumnName("SCH_DUEDATE");
        builder.Property(e => e.ReceivedAmount).HasColumnName("SCH_RECAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ReceivedDate).HasColumnName("SCH_RECDATE");
        builder.Property(e => e.ReceivedTransactionId).HasColumnName("SCH_RECTRANID");
        builder.Property(e => e.LogSysId).HasColumnName("SCH_LOGSYSID");
        builder.Property(e => e.Year).HasColumnName("SCH_YEAR");

        builder.HasOne(e => e.Investment).WithMany(i => i.ScheduleDetails).HasForeignKey(e => e.InvNo);
        builder.HasIndex(e => new { e.InvNo, e.DueDate }).HasDatabaseName("IDX_INV_SCHDET_INVNO");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class CallDetailConfiguration : IEntityTypeConfiguration<CallDetail>
{
    public void Configure(EntityTypeBuilder<CallDetail> builder)
    {
        builder.ToTable("INV_CALLDET");
        builder.HasKey(e => e.CallDetailId);
        builder.Property(e => e.CallDetailId).HasColumnName("INV_CALLDETID");
        builder.Property(e => e.InvNo).HasColumnName("INV_INVNO");
        builder.Property(e => e.CallDate).HasColumnName("INV_CALLDATE");
        builder.Property(e => e.CallAmount).HasColumnName("INV_CALLAMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ConfirmStatus).HasColumnName("INV_CNFSTATUS").HasMaxLength(1);
        builder.Property(e => e.InterestRevFlag).HasColumnName("INV_INTREVFLAG").HasMaxLength(1);
        builder.Property(e => e.RevisedInterestRate).HasColumnName("INV_REVINTRATE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SaleRefId).HasColumnName("INV_SALEREFID");
        builder.Property(e => e.LastModBy).HasColumnName("INV_LASTMODBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.LastModOn).HasColumnName("INV_LASTMODON");
        builder.Property(e => e.SlNo).HasColumnName("INV_SLNO");

        builder.HasOne(e => e.Investment).WithMany(i => i.CallDetails).HasForeignKey(e => e.InvNo);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ApprovalDetailConfiguration : IEntityTypeConfiguration<ApprovalDetail>
{
    public void Configure(EntityTypeBuilder<ApprovalDetail> builder)
    {
        builder.ToTable("INV_APPRDETAILS");
        builder.HasKey(e => e.ApprovalDetailId);
        builder.Property(e => e.ApprovalDetailId).HasColumnName("INV_APRDETID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.InvestmentId).HasColumnName("INV_INVID");
        builder.Property(e => e.RefId).HasColumnName("INV_REFID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ApprovalLevel).HasColumnName("INV_APRLEVEL").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Flag).HasColumnName("INV_FLAG").HasMaxLength(1);
        builder.Property(e => e.ApproverSysId).HasColumnName("INV_APPROVERSYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ApprovedOn).HasColumnName("INV_APPROVEDON");
        builder.Property(e => e.Remarks).HasColumnName("INV_REMARKS").HasMaxLength(200);

        builder.HasOne(e => e.Investment).WithMany(i => i.ApprovalDetails)
            .HasForeignKey(e => e.InvestmentId);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class InvestmentCategoryConfiguration : IEntityTypeConfiguration<InvestmentCategory>
{
    public void Configure(EntityTypeBuilder<InvestmentCategory> builder)
    {
        builder.ToTable("INVCAT_MAST");
        builder.HasKey(e => e.Code);
        builder.Property(e => e.Code).HasColumnName("INVCAT_CODE").ValueGeneratedNever();
        builder.Property(e => e.ShortCode).HasColumnName("INVCAT_SHTCODE").HasMaxLength(10);
        builder.Property(e => e.Name).HasColumnName("INVCAT_NAME").HasMaxLength(50);
        builder.Property(e => e.Denomination).HasColumnName("INVCAT_DENOM");
        builder.Property(e => e.GroupId).HasColumnName("INVCAT_GRPID");

        builder.HasIndex(e => e.Name).HasDatabaseName("IDX_INVCAT_MAST_NAME");
    }
}

public class InvestmentSubCategoryConfiguration : IEntityTypeConfiguration<InvestmentSubCategory>
{
    public void Configure(EntityTypeBuilder<InvestmentSubCategory> builder)
    {
        builder.ToTable("INVSUBCAT_MAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("INVSUBCAT_ID").ValueGeneratedNever();
        builder.Property(e => e.ShortName).HasColumnName("INVSUBCAT_SHTNAME").HasMaxLength(10);
        builder.Property(e => e.Name).HasColumnName("INVSUBCAT_NAME").HasMaxLength(50);
        builder.Property(e => e.CategoryId).HasColumnName("INVSUBCAT_CATID");
        builder.Property(e => e.InterestDenomination).HasColumnName("INVSUBCAT_INTDEN");
        builder.Property(e => e.SubCategory).HasColumnName("INVSUBCAT_SUBCAT");

        builder.HasOne(e => e.Category).WithMany(c => c.SubCategories).HasForeignKey(e => e.CategoryId);
    }
}

public class CreditAgencyConfiguration : IEntityTypeConfiguration<CreditAgency>
{
    public void Configure(EntityTypeBuilder<CreditAgency> builder)
    {
        builder.ToTable("CREDITAGENCY_MAST");
        builder.HasKey(e => e.AgencyId);
        builder.Property(e => e.AgencyId).HasColumnName("AGENCY_ID").ValueGeneratedNever();
        builder.Property(e => e.AgencyName).HasColumnName("AGENCY_NAME").HasMaxLength(50);
    }
}

public class CreditRatingConfiguration : IEntityTypeConfiguration<CreditRating>
{
    public void Configure(EntityTypeBuilder<CreditRating> builder)
    {
        builder.ToTable("CREDITRATING_MAST");
        builder.HasKey(e => e.RatingId);
        builder.Property(e => e.RatingId).HasColumnName("RATING_ID").ValueGeneratedNever();
        builder.Property(e => e.RatingName).HasColumnName("RATING_NAME").HasMaxLength(50);
    }
}

public class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.ToTable("INVBROKER_MASTER");
        builder.HasKey(e => e.BrokerId);
        builder.Property(e => e.BrokerId).HasColumnName("BROKER_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.BrokerName).HasColumnName("BROKER_NAME").HasMaxLength(50);
        builder.Property(e => e.BrokerStatus).HasColumnName("BROKER_STATUS").HasMaxLength(1);
    }
}

public class InterestScheduleBatchConfiguration : IEntityTypeConfiguration<InterestScheduleBatch>
{
    public void Configure(EntityTypeBuilder<InterestScheduleBatch> builder)
    {
        builder.ToTable("INV_INTSCHBATCH");
        builder.HasKey(e => e.BatchNo);
        builder.Property(e => e.BatchNo).HasColumnName("INV_INTSCHBATHNO");
        builder.Property(e => e.InvestmentId).HasColumnName("INV_INTSCHINVID");
        builder.Property(e => e.Year).HasColumnName("INV_INVSCHYEAR");
        builder.Property(e => e.PreviousRunDate).HasColumnName("INV_INVSCHPREVRUNDATE");
        builder.Property(e => e.LastRunDate).HasColumnName("INV_INVSCHLASTRUNDATE");
        builder.Property(e => e.EnteredOn).HasColumnName("INV_INVSCHENTON");
        builder.Property(e => e.EnteredBy).HasColumnName("INV_INVSCHENTBY").HasColumnType("decimal(38,0)");
    }
}

public class BankDetailConfiguration : IEntityTypeConfiguration<BankDetail>
{
    public void Configure(EntityTypeBuilder<BankDetail> builder)
    {
        builder.ToTable("INV_BANKDET");
        builder.HasKey(e => e.TransactionId);
        builder.Property(e => e.TransactionId).HasColumnName("BNK_TRANID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EntryType).HasColumnName("BNK_ENTRYTYPE").HasMaxLength(3);
        builder.Property(e => e.TransactionType).HasColumnName("BNK_TRANTYPE").HasMaxLength(3);
        builder.Property(e => e.InvNo).HasColumnName("BNK_INVNO");
        builder.Property(e => e.TransactionAmount).HasColumnName("BNK_TRNAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.BankId).HasColumnName("BNK_ID");
        builder.Property(e => e.DematId).HasColumnName("BNK_DEMATID");
        builder.Property(e => e.TransactionDate).HasColumnName("BNK_TRANDATE");
        builder.Property(e => e.Remarks).HasColumnName("BNK_REMARKS").HasMaxLength(200);

        builder.HasOne(e => e.Investment).WithMany(i => i.BankDetails).HasForeignKey(e => e.InvNo);
        builder.Ignore(e => e.DomainEvents);
    }
}
