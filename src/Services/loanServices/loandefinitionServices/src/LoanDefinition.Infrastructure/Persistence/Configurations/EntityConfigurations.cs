using LoanDefinition.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanDefinition.Infrastructure.Persistence.Configurations;

public class LoanTypeMasterConfiguration : IEntityTypeConfiguration<LoanTypeMaster>
{
    public void Configure(EntityTypeBuilder<LoanTypeMaster> builder)
    {
        builder.ToTable("LOAN_TYPEMASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOAN_TYPE").ValueGeneratedNever();
        builder.Property(e => e.LoanName).HasColumnName("LOAN_NAME").HasMaxLength(200).IsRequired();
        builder.Property(e => e.LoanCategory).HasColumnName("LOAN_CATEGORY").HasMaxLength(10).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("LOAN_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("LOAN_CREATEDON");
        builder.Property(e => e.LastModifiedBy).HasColumnName("LOAN_MODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("LOAN_MODIFIEDON");

        builder.HasMany(e => e.Loans).WithOne(e => e.LoanType).HasForeignKey(e => e.LoanTypeId);
    }
}

public class LoanMasterConfiguration : IEntityTypeConfiguration<LoanMaster>
{
    public void Configure(EntityTypeBuilder<LoanMaster> builder)
    {
        builder.ToTable("LOAN_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOAN_ID").ValueGeneratedNever();
        builder.Property(e => e.LoanName).HasColumnName("LOAN_NAME").HasMaxLength(65).IsRequired();
        builder.Property(e => e.LoanPurpose).HasColumnName("LOAN_PURPOSE").HasMaxLength(200).IsRequired();
        builder.Property(e => e.ApplyToUnit).HasColumnName("LOAN_APPLYToUNIT");
        builder.Property(e => e.OrgId).HasColumnName("LOAN_ORGID");
        builder.Property(e => e.UnitId).HasColumnName("LOAN_UNITID");
        builder.Property(e => e.LoanTypeId).HasColumnName("LOAN_TYPEID");
        builder.Property(e => e.ApplyToConfirmedEmp).HasColumnName("LOAN_APPLYToCONFIRMEMP").HasMaxLength(1);
        builder.Property(e => e.GradeCategory).HasColumnName("LOAN_GRADECATAGORY").HasMaxLength(3);
        builder.Property(e => e.ApplyToAllGrade).HasColumnName("LOAN_APPLYToALLGRADE");
        builder.Property(e => e.GradeId).HasColumnName("LOAN_GRADEID");

        builder.OwnsOne(e => e.LoanLimit, ll =>
        {
            ll.Property(p => p.MinimumLimit).HasColumnName("LOAN_MINIMUMLIMIT");
            ll.Property(p => p.MaximumLimit).HasColumnName("LOAN_MAXIMUMLIMIT");
        });

        builder.Property(e => e.AutoPayOnCompletion).HasColumnName("LOAN_AUTOPAYONCOMPLETION").HasMaxLength(1);
        builder.Property(e => e.AllowForceClose).HasColumnName("LOAN_ALLOWFORCECLOSE").HasMaxLength(1);
        builder.Property(e => e.AllowMultipleNos).HasColumnName("LOAN_ALLOWMULTIPLENOS").HasMaxLength(1);
        builder.Property(e => e.OnConfirmation).HasColumnName("LOAN_ONCONFIRMATION").HasMaxLength(1);
        builder.Property(e => e.CheckEntitlement).HasColumnName("LOAN_CHECKENTITLEMENT").HasMaxLength(1);
        builder.Property(e => e.Recoverable).HasColumnName("LOAN_RECOVERABLE").HasMaxLength(1);
        builder.Property(e => e.ApplicationNos).HasColumnName("LOAN_APPLICATIONNOS");
        builder.Property(e => e.CheckNetPayPercentage).HasColumnName("LOAN_CHECKNETPAYPERCENTAGE").HasMaxLength(1);
        builder.Property(e => e.BkdInterestRateRevision).HasColumnName("LOAN_BKDINTERESTRATEREVISION").HasMaxLength(1);
        builder.Property(e => e.SubClassAvailable).HasColumnName("LOAN_SUBCLASSAVAILABLE").HasMaxLength(1);
        builder.Property(e => e.ItClass).HasColumnName("LOAN_ITCLASS").HasMaxLength(3);
        builder.Property(e => e.DocumentRequired).HasColumnName("LOAN_DOCUMENTREQUIRED").HasMaxLength(1);
        builder.Property(e => e.DocumentUploadRequired).HasColumnName("LOAN_DOCUMENTUPLOADREQUIRED").HasMaxLength(1);
        builder.Property(e => e.SelfApplicationAllowed).HasColumnName("LOAN_SLFAPPALLOWED").HasMaxLength(1);
        builder.Property(e => e.EmpSpecificRatesAllowed).HasColumnName("LOAN_EMPSPECIFICRATESALLOWED").HasMaxLength(1);
        builder.Property(e => e.HrApproval).HasColumnName("LOAN_HRAPPROVAL").HasMaxLength(1);
        builder.Property(e => e.EffectiveDate).HasColumnName("LOAN_EFFDATE");
        builder.Property(e => e.ClosureDate).HasColumnName("LOAN_CLSDATE");
        builder.Property(e => e.CompoundingFactor).HasColumnName("LOAN_COMFACTOR").HasMaxLength(1);
        builder.Property(e => e.InterestFrequency).HasColumnName("LOAN_INTFREQUENCY").HasMaxLength(1);
        builder.Property(e => e.RecoveryType).HasColumnName("LOAN_RECTYPE").HasMaxLength(3);
        builder.Property(e => e.CreatedBy).HasColumnName("LOAN_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("LOAN_CREATEDON");
        builder.Property(e => e.LastModifiedBy).HasColumnName("LOAN_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("LOAN_LASTMODIFIEDON");
        builder.Property(e => e.BulkUploadAllowed).HasColumnName("LOAN_BULKUPLOADALLOWED").HasMaxLength(1);
        builder.Property(e => e.PrincipalRecoveryEdId).HasColumnName("LOAN_PRNRECEDID");
        builder.Property(e => e.InterestRecoveryEdId).HasColumnName("LOAN_INTRECEDID");
        builder.Property(e => e.PrincipalPaymentEdId).HasColumnName("LOAN_PRNPAYEDID");
        builder.Property(e => e.PolicyFileName).HasColumnName("LOAN_POLICYFILENAME").HasMaxLength(250);
        builder.Property(e => e.GuarantorRequired).HasColumnName("LOAN_GUARANTORREQUIRED").HasMaxLength(1);
        builder.Property(e => e.CheckBasicEntitlement).HasColumnName("LOAN_CHKBASICENTITLEMENT").HasMaxLength(1);
        builder.Property(e => e.AllowAdditionalLoan).HasColumnName("LOAN_ALLOWADDLLOAN").HasMaxLength(1);
        builder.Property(e => e.AdditionalLoanNo).HasColumnName("LOAN_ADDITONALLOANNO");
        builder.Property(e => e.CurrentRecovery).HasColumnName("LOAN_CURRECOVERY").HasMaxLength(1);
        builder.Property(e => e.ReportingUnitApplicable).HasColumnName("LOAN_REPUNITAPPLICABLE").HasMaxLength(1);
        builder.Property(e => e.ReportingUnitId).HasColumnName("LOAN_REPUNITID");
        builder.Property(e => e.FlexiFirstInstDate).HasColumnName("LOAN_FLEXIFIRSTINSDATE").HasMaxLength(1);

        builder.HasIndex(e => e.LoanTypeId).HasDatabaseName("IDX_LOAN_MASTER_LOAN_TYPEID");
    }
}

public class LoanSubClassConfiguration : IEntityTypeConfiguration<LoanSubClass>
{
    public void Configure(EntityTypeBuilder<LoanSubClass> builder)
    {
        builder.ToTable("LOAN_SUBCLASS");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("SUBCLASS_ID");
        builder.Property(e => e.LoanId).HasColumnName("SUBCLASS_LOANID");
        builder.Property(e => e.Description).HasColumnName("SUBCLASS_DESC").HasMaxLength(200).IsRequired();
        builder.Property(e => e.ItClassification).HasColumnName("SUBCLASS_IT").HasMaxLength(3);
        builder.Property(e => e.ModifiedBy).HasColumnName("SUBCLASS_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("SUBCLASS_MODIFIEDON");
        builder.Property(e => e.PrincipalRecoveryEdId).HasColumnName("SUBCLASS_PRNRECEDID");
        builder.Property(e => e.InterestRecoveryEdId).HasColumnName("SUBCLASS_INTRECEDID");

        builder.HasOne(e => e.Loan).WithMany(e => e.SubClasses).HasForeignKey(e => e.LoanId);
    }
}

public class LoanInterestRateConfiguration : IEntityTypeConfiguration<LoanInterestRateMaster>
{
    public void Configure(EntityTypeBuilder<LoanInterestRateMaster> builder)
    {
        builder.ToTable("LOAN_INTRATEMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOANINT_RATEID");
        builder.Property(e => e.LoanId).HasColumnName("LOANINT_LOANID");
        builder.Property(e => e.EffectiveDate).HasColumnName("LOANINT_EFFDATE");
        builder.Property(e => e.ClosureDate).HasColumnName("LOANINT_CLSDATE");
        builder.Property(e => e.Rate).HasColumnName("LOANINT_RATE");
        builder.Property(e => e.LastModifiedBy).HasColumnName("LOANINT_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("LOANINT_LASTMODIFIEDON");
        builder.Property(e => e.EmiAmount).HasColumnName("LOANINT_EMIAMT");
        builder.Property(e => e.InstallmentNos).HasColumnName("LOANINT_INSNOS");
        builder.Property(e => e.RangeSpecific).HasColumnName("LOANINT_RANGESPECIFIC").HasMaxLength(1);

        builder.HasOne(e => e.Loan).WithMany(e => e.InterestRates).HasForeignKey(e => e.LoanId);
        builder.HasIndex(e => e.LoanId).HasDatabaseName("IDX_LOAN_INTRATEMAST_LOANINT_LOANID");
    }
}

public class LoanLimitRangeConfiguration : IEntityTypeConfiguration<LoanLimitRangeMaster>
{
    public void Configure(EntityTypeBuilder<LoanLimitRangeMaster> builder)
    {
        builder.ToTable("LOANLIMITRANGE_MAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOANLIMITRANGE_RATEID");
        builder.Property(e => e.LoanId).HasColumnName("LOANLIMITRANGE_LOANID");
        builder.Property(e => e.MinYear).HasColumnName("LOANLIMITRANGE_MINYEAR");
        builder.Property(e => e.MaxYear).HasColumnName("LOANLIMITRANGE_MAXYEAR");
        builder.Property(e => e.LoanAmount).HasColumnName("LOANLIMITRANGE_LOANAMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("LOANLIMITRANGE_EFFDATE");
        builder.Property(e => e.ClosureDate).HasColumnName("LOANLIMITRANGE_CLSDATE");
        builder.Property(e => e.CreatedBy).HasColumnName("LOANLIMITRANGE_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("LOANLIMITRANGE_CREATEDON");
        builder.Property(e => e.ModifiedBy).HasColumnName("LOANLIMITRANGE_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("LOANLIMITRANGE_MODIFIEDON");
        builder.Property(e => e.InterestRate).HasColumnName("LOANLIMITRANGE_INTRATE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.AdditionalMinValue).HasColumnName("LOANLIMITRANGE_ADDLMINVALUE").HasColumnType("decimal(19,0)");

        builder.HasOne(e => e.Loan).WithMany(e => e.LimitRanges).HasForeignKey(e => e.LoanId);
    }
}

public class LoanPerquisiteConfiguration : IEntityTypeConfiguration<LoanPerquisite>
{
    public void Configure(EntityTypeBuilder<LoanPerquisite> builder)
    {
        builder.ToTable("LOAN_PRQ");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOAN_PRQID");
        builder.Property(e => e.ClassId).HasColumnName("LOAN_CLASSID").HasMaxLength(3).IsRequired();
        builder.Property(e => e.EffectiveDate).HasColumnName("LOAN_EFFDATE");
        builder.Property(e => e.ClosureDate).HasColumnName("LOAN_CLSDATE");
        builder.Property(e => e.ItInterestRate).HasColumnName("LOAN_ITINTRATE");
        builder.Property(e => e.ModifiedBy).HasColumnName("LOAN_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("LOAN_MODIFIEDON");
        builder.Property(e => e.MinAmount).HasColumnName("LOAN_MINAMT").HasColumnType("decimal(19,0)");

        builder.HasIndex(e => e.ClassId).HasDatabaseName("IDX_LOAN_PRQ_LOAN_CLASSID");
    }
}

public class LoanFestivalConfiguration : IEntityTypeConfiguration<LoanFestival>
{
    public void Configure(EntityTypeBuilder<LoanFestival> builder)
    {
        builder.ToTable("LOAN_FESTIVALS");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOANFEST_ID").ValueGeneratedNever();
        builder.Property(e => e.Description).HasColumnName("LOANFEST_DESC").HasMaxLength(200).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("LOANFEST_STRDATE");
        builder.Property(e => e.EndDate).HasColumnName("LOANFEST_ENDDATE");
        builder.Property(e => e.CreatedBy).HasColumnName("LOANFEST_CREATEDBY");
        builder.Property(e => e.CreatedOn).HasColumnName("LOANFEST_CREATEDON");
        builder.Property(e => e.LastModifiedBy).HasColumnName("LOANFEST_MODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("LOANFEST_MODIFIEDON");
    }
}

public class LoanFestivalMapConfiguration : IEntityTypeConfiguration<LoanFestivalMap>
{
    public void Configure(EntityTypeBuilder<LoanFestivalMap> builder)
    {
        builder.ToTable("LOAN_FESTIVALMAP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOANFESTMAP_ID");
        builder.Property(e => e.LoanId).HasColumnName("LOANFESTMAP_LOANID");
        builder.Property(e => e.FestivalId).HasColumnName("LOANFESTMAP_FESTIVALID");
        builder.Property(e => e.ModifiedBy).HasColumnName("LOANFESTMAP_MODIFIEDBY");
        builder.Property(e => e.ModifiedOn).HasColumnName("LOANFESTMAP_MODIFIEDON");

        builder.HasOne(e => e.Loan).WithMany(e => e.FestivalMaps).HasForeignKey(e => e.LoanId);
        builder.HasOne(e => e.Festival).WithMany(e => e.FestivalMaps).HasForeignKey(e => e.FestivalId);
    }
}

public class LoanAccountMasterConfiguration : IEntityTypeConfiguration<LoanAccountMaster>
{
    public void Configure(EntityTypeBuilder<LoanAccountMaster> builder)
    {
        builder.ToTable("LOAN_ACCMAST");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LOAN_ACID");
        builder.Property(e => e.LoanType).HasColumnName("LOAN_TYPE");
        builder.Property(e => e.GradeType).HasColumnName("LOAN_GRADETYPE").HasMaxLength(3).IsRequired();
        builder.Property(e => e.AccountCode).HasColumnName("LOAN_ACCODE").HasMaxLength(5).IsRequired();
        builder.Property(e => e.UpdatedBy).HasColumnName("LOAN_UPDATEDBY");
        builder.Property(e => e.UpdatedOn).HasColumnName("LOAN_UPDATEDON");
    }
}
