using CompensationBenefits.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompensationBenefits.Infrastructure.Persistence.Configurations;

public class SalaryMainConfiguration : IEntityTypeConfiguration<SalaryMain>
{
    public void Configure(EntityTypeBuilder<SalaryMain> b)
    {
        b.ToTable("SALARY_MAIN");
        b.HasKey(x => x.SalaryId);
        b.Property(x => x.SalaryId).HasColumnName("SALARY_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryType).HasColumnName("SALARY_TYPE").HasMaxLength(1).IsRequired();
        b.Property(x => x.SalaryCTC).HasColumnName("SALARY_CTC").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryStructureId).HasColumnName("SALARY_STRUCTUREID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryFooterId).HasColumnName("SALARY_FOOTERID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryCopyEmpSysId).HasColumnName("SALARY_COPYEMPSYSID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryCreatedBy).HasColumnName("SALARY_CREATEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryCreatedOn).HasColumnName("SALARY_CREATEDON");
        b.Property(x => x.SalaryCancelledBy).HasColumnName("SALARY_CANCELLEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalaryCancelledOn).HasColumnName("SALARY_CANCELLEDON");

        b.HasMany(x => x.Details)
            .WithOne(d => d.SalaryMain)
            .HasForeignKey(d => d.SalDetSalaryId);
    }
}

public class SalaryDetailConfiguration : IEntityTypeConfiguration<SalaryDetail>
{
    public void Configure(EntityTypeBuilder<SalaryDetail> b)
    {
        b.ToTable("SALARY_DET");
        b.HasKey(x => x.SalDetId);
        b.Property(x => x.SalDetId).HasColumnName("SALDET_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalDetSalaryId).HasColumnName("SALDET_SALARYID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalDetSrl).HasColumnName("SALDET_SRL").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalDetAnnGroup).HasColumnName("SALDET_ANNGROUP").HasMaxLength(50);
        b.Property(x => x.SalDetEdId).HasColumnName("SALDET_EDID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalDetCategory).HasColumnName("SALDET_CATEGORY").HasMaxLength(1).IsRequired();
        b.Property(x => x.SalDetEdName).HasColumnName("SALDET_EDNAME").HasMaxLength(50).IsRequired();
        b.Property(x => x.SalDetEdAmt).HasColumnName("SALDET_EDAMT").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalDetFrequency).HasColumnName("SALDET_FREQUENCY").HasMaxLength(1).IsRequired();
        b.Property(x => x.SalDetSuperChar).HasColumnName("SALDET_SUPERCHAR").HasMaxLength(25);
        b.Property(x => x.SalDetSuperDesc).HasColumnName("SALDET_SUPERDESC").HasMaxLength(1000);
        b.Property(x => x.SalDetYearType).HasColumnName("SALDET_YEARTYPE").HasMaxLength(1);
        b.Property(x => x.SalDetGlobalUnitId).HasColumnName("SALDET_GLOBALUNITID").HasColumnType("decimal(38,0)");
        b.Property(x => x.SalDetFormula).HasColumnName("SALDET_FORMULA").HasMaxLength(1).IsRequired();
        b.Property(x => x.SalDetShowMonthly).HasColumnName("SALDET_SHOWMONTHLY").HasMaxLength(1).IsRequired();
        b.Property(x => x.SalDetAnnexOnly).HasColumnName("SALDET_ANNEXONLY").HasMaxLength(1).IsRequired();
    }
}

public class SalaryStructureMainConfiguration : IEntityTypeConfiguration<SalaryStructureMain>
{
    public void Configure(EntityTypeBuilder<SalaryStructureMain> b)
    {
        b.ToTable("SALSTRUCTURE_MAIN");
        b.HasKey(x => x.StructureId);
        b.Property(x => x.StructureId).HasColumnName("STRUCTURE_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureUnitId).HasColumnName("STRUCTURE_UNITID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureName).HasColumnName("STRUCTURE_NAME").HasMaxLength(200).IsRequired();
        b.Property(x => x.StructureGradeCategory).HasColumnName("STRUCTURE_GRADECATEGORY").HasMaxLength(3).IsRequired();
        b.Property(x => x.StructureApplyToAll).HasColumnName("STRUCTURE_APPLYTOALL").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureGradeId).HasColumnName("STRUCTURE_GRADEID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureType).HasColumnName("STRUCTURE_TYPE").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructureCtcMin).HasColumnName("STRUCTURE_CTCMIN").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureCtcMax).HasColumnName("STRUCTURE_CTCMAX").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureFooterId).HasColumnName("STRUCTURE_FOOTERID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureClsDate).HasColumnName("STRUCTURE_CLSDATE");
        b.Property(x => x.StructureCreatedBy).HasColumnName("STRUCTURE_CREATEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureCreatedOn).HasColumnName("STRUCTURE_CREATEDON");
        b.Property(x => x.StructureLastModifiedBy).HasColumnName("STRUCTURE_LASTMODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureLastModifiedOn).HasColumnName("STRUCTURE_LASTMODIFIEDON");
        b.Property(x => x.StructureApplyToAllUnit).HasColumnName("STRUCTURET_APPLYTOALLUNIT").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructureOfferFooterId).HasColumnName("STRUCTURE_OFFERFOOTERID").HasColumnType("decimal(38,0)");

        b.HasMany(x => x.Details)
            .WithOne(d => d.StructureMain)
            .HasForeignKey(d => d.StructDetStructureId);
    }
}

public class SalaryStructureDetailConfiguration : IEntityTypeConfiguration<SalaryStructureDetail>
{
    public void Configure(EntityTypeBuilder<SalaryStructureDetail> b)
    {
        b.ToTable("SALSTRUCTURE_DET");
        b.HasKey(x => x.StructDetId);
        b.Property(x => x.StructDetId).HasColumnName("STRUCTDET_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetStructureId).HasColumnName("STRUCTDET_STRUCTUREID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetEdId).HasColumnName("STRUCTDET_EDID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetAmtType).HasColumnName("STRUCTDET_AMTTYPE").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructDetCalType).HasColumnName("STRUCTDET_CALTYPE").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetCategory).HasColumnName("STRUCTDET_CATEGORY").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructDetFrequency).HasColumnName("STRUCTDET_FREQUENCY").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructDetEdAmt).HasColumnName("STRUCTDET_EDAMT").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetMinValue).HasColumnName("STRUCTDET_MINVALUE").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetMaxValue).HasColumnName("STRUCTDET_MAXVALUE").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetGlobalUnitId).HasColumnName("STRUCTDET_GLOBALUNITID").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetSuperChar).HasColumnName("STRUCTDET_SUPERCHAR").HasMaxLength(25);
        b.Property(x => x.StructDetSuperDesc).HasColumnName("STRUCTDET_SUPERDESC").HasMaxLength(1000);
        b.Property(x => x.StructDetModify).HasColumnName("STRUCTDET_MODIFY").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructDetFormula).HasColumnName("STRUCTDET_FORMULA").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructDetCreatedBy).HasColumnName("STRUCTDET_CREATEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetCreatedOn).HasColumnName("STRUCTDET_CREATEDON");
        b.Property(x => x.StructDetLastModifiedBy).HasColumnName("STRUCTDET_LASTMODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.StructDetLastModifiedOn).HasColumnName("STRUCTDET_LASTMODIFIEDON");
        b.Property(x => x.StructureShowMonthly).HasColumnName("STRUCTURE_SHOWMONTHLY").HasMaxLength(1).IsRequired();
        b.Property(x => x.StructureAnnexOnly).HasColumnName("STRUCTURE_ANNEXONLY").HasMaxLength(1).IsRequired();
    }
}

public class MediclaimMasterConfiguration : IEntityTypeConfiguration<MediclaimMaster>
{
    public void Configure(EntityTypeBuilder<MediclaimMaster> b)
    {
        b.ToTable("MEDICLAIM_MASTER");
        b.HasKey(x => x.MediclaimId);
        b.Property(x => x.MediclaimId).HasColumnName("MEDICLAIM_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MediclaimRefName).HasColumnName("MEDICLAIM_REFNAME").HasMaxLength(200);
        b.Property(x => x.MediclaimProviderId).HasColumnName("MEDICLAIM_PROVIDERID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MediclaimTppId).HasColumnName("MEDICLAIM_TPPID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MediclaimStartDate).HasColumnName("MEDICLAIM_STARTDATE");
        b.Property(x => x.MediclaimCloseDate).HasColumnName("MEDICLAIM_CLOSEDATE");
        b.Property(x => x.MediclaimMaxEntryDate).HasColumnName("MEDICLAIM_MAXENTRYDATE");
        b.Property(x => x.MediclaimInsRefNo).HasColumnName("MEDICLAIM_INSREFNO").HasMaxLength(100);
        b.Property(x => x.MediclaimType).HasColumnName("MEDICLAIM_TYPE").HasMaxLength(1);
        b.Property(x => x.MediclaimPaidBy).HasColumnName("MEDICLAIM_PAIDBY").HasMaxLength(1);
        b.Property(x => x.MediclaimServiceTaxPer).HasColumnName("MEICLAIM_SERVICETAXPER");
        b.Property(x => x.MediclaimCompPayLimit).HasColumnName("MEDICLAIM_COMPPAYLIMIT").HasColumnType("decimal(38,0)");
        b.Property(x => x.MediclaimLoadingPer).HasColumnName("MEDICLAIM_LOADINGPER");
        b.Property(x => x.MediclaimNonClaimPer).HasColumnName("MEDICLAIM_NONCLAIMPER");

        b.HasMany(x => x.YearlyPremiums)
            .WithOne(y => y.MediclaimMaster)
            .HasForeignKey(y => y.MedYpMediclaimId);
    }
}

public class MediclaimDetailConfiguration : IEntityTypeConfiguration<MediclaimDetail>
{
    public void Configure(EntityTypeBuilder<MediclaimDetail> b)
    {
        b.ToTable("MEDICLAIM_DET");
        b.HasKey(x => x.MedNominationRunId);
        b.Property(x => x.MedNominationRunId).HasColumnName("MED_NOMINATIONRUNID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedNominationId).HasColumnName("MED_NOMINATIONID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedRelationship).HasColumnName("MED_RELATIONSHIP").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedNomineeName).HasColumnName("MED_NOMINEENAME").HasMaxLength(200).IsRequired();
        b.Property(x => x.MedNomineeDob).HasColumnName("MED_NOMINEEDOB");
        b.Property(x => x.MedNomineeAge).HasColumnName("MED_NOMINEEAGE").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedNomineeGender).HasColumnName("MED_NOMINEEGENDER").HasMaxLength(1).IsRequired();
        b.Property(x => x.MedPremium).HasColumnName("MED_PREMIUM").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedTaxStatus).HasColumnName("MED_TAXSTATUS").HasMaxLength(1).IsRequired();
        b.Property(x => x.MedNetPremium).HasColumnName("MED_NETPREMIUM");
        b.Property(x => x.MedPremiumServiceTax).HasColumnName("MED_PREMIUMSERVICETAX");
        b.Property(x => x.MedGrossPremium).HasColumnName("MED_GROSSPREMIUM");
    }
}

public class MediclaimYearlyPremiumConfiguration : IEntityTypeConfiguration<MediclaimYearlyPremium>
{
    public void Configure(EntityTypeBuilder<MediclaimYearlyPremium> b)
    {
        b.ToTable("MEDICLAIM_YEARLYPREM");
        b.HasKey(x => x.MedYpYearlyPremId);
        b.Property(x => x.MedYpYearlyPremId).HasColumnName("MEDYP_YEARLYPREMID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedYpMediclaimId).HasColumnName("MEDYP_MEDICLAIMID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedYpSumAssured).HasColumnName("MEDYP_SUMASSURED").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedYpPremiumAmnt).HasColumnName("MEDYP_PREMIUMAMNT").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedYpModifiedBy).HasColumnName("MEDYP_MODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedYpModifiedOn).HasColumnName("MEDYP_MODIFIEDON");
        b.Property(x => x.MedYpType).HasColumnName("MEDYP_TYPE").HasMaxLength(1).IsRequired();
    }
}

public class MobileConnectionConfiguration : IEntityTypeConfiguration<MobileConnection>
{
    public void Configure(EntityTypeBuilder<MobileConnection> b)
    {
        b.ToTable("MOBILE_CONNECTION");
        b.HasKey(x => x.ConnId);
        b.Property(x => x.ConnId).HasColumnName("CONN_ID");
        b.Property(x => x.ConnEmpSysId).HasColumnName("CONN_EMPSYSID");
        b.Property(x => x.ConnEffDate).HasColumnName("CONN_EFFDATE");
        b.Property(x => x.ConnClsDate).HasColumnName("CONN_CLSDATE");
        b.Property(x => x.ConnType).HasColumnName("CONN_TYPE").HasMaxLength(1).IsRequired();
        b.Property(x => x.ConnPhoneNo).HasColumnName("CONN_PHONENO");
        b.Property(x => x.ConnRemarks).HasColumnName("CONN_REMARKS").HasMaxLength(500);
        b.Property(x => x.ConnOpenRequestNo).HasColumnName("CONN_OPENREQUESTNO");
        b.Property(x => x.ConnCloseRequestNo).HasColumnName("CONN_CLOSEREQUESTNO");
        b.Property(x => x.ConnCalendarId).HasColumnName("CONN_CALENDARID");
        b.Property(x => x.ConnCreatedBy).HasColumnName("CONN_CREATEDBY");
        b.Property(x => x.ConnCreatedOn).HasColumnName("CONN_CREATEDON");
        b.Property(x => x.ConnModifiedBy).HasColumnName("CONN_MODIFIEDBY");
        b.Property(x => x.ConnModifiedOn).HasColumnName("CONN_MODIFIEDON");
    }
}

public class RetiralRangeMasterConfiguration : IEntityTypeConfiguration<RetiralRangeMaster>
{
    public void Configure(EntityTypeBuilder<RetiralRangeMaster> b)
    {
        b.ToTable("RETRIALS_RANGEMAST");
        b.HasKey(x => x.RrMastId);
        b.Property(x => x.RrMastId).HasColumnName("RRMAST_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.RrMastUnitId).HasColumnName("RRMAST_UNITID").HasColumnType("decimal(38,0)");
        b.Property(x => x.RrMastFromYear).HasColumnName("RRMAST_FROMYEAR").HasColumnType("decimal(38,0)");
        b.Property(x => x.RrMastToYear).HasColumnName("RRMAST_TOYEAR").HasColumnType("decimal(38,0)");
        b.Property(x => x.RrMastPercentage).HasColumnName("RRMAST_PERCENTAGE").HasColumnType("decimal(38,0)");
        b.Property(x => x.RrMastModifiedBy).HasColumnName("RRMAST_MODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.RrMastModifiedOn).HasColumnName("RRMAST_MODIFIEDON");
    }
}

public class MediclaimExceptionConfiguration : IEntityTypeConfiguration<MediclaimException>
{
    public void Configure(EntityTypeBuilder<MediclaimException> b)
    {
        b.ToTable("MEDICLAIM_EXCEPTION");
        b.HasNoKey();
        b.Property(x => x.MediclaimEmpSysId).HasColumnName("MEDICLAIM_EMPSYSID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MediclaimId).HasColumnName("MEDICLAIM_ID").HasColumnType("decimal(38,0)");
    }
}

public class MediclaimPremiumPercentageConfiguration : IEntityTypeConfiguration<MediclaimPremiumPercentage>
{
    public void Configure(EntityTypeBuilder<MediclaimPremiumPercentage> b)
    {
        b.ToTable("MEDICLAIM_PREMPERCENTAGE");
        b.HasKey(x => x.MedPpId);
        b.Property(x => x.MedPpId).HasColumnName("MED_PPID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedRelationshipId).HasColumnName("MED_RELATIONSHIPID").HasColumnType("decimal(38,0)");
        b.Property(x => x.MedPercentage).HasColumnName("MED_PERCENTAGE").HasColumnType("decimal(38,0)");
    }
}

public class MobileLimitMasterConfiguration : IEntityTypeConfiguration<MobileLimitMaster>
{
    public void Configure(EntityTypeBuilder<MobileLimitMaster> b)
    {
        b.ToTable("MOBILE_LIMITMAST");
        b.HasKey(x => x.LimitId);
        b.Property(x => x.LimitId).HasColumnName("LIMIT_ID");
        b.Property(x => x.LimitOrg).HasColumnName("LIMIT_ORG");
        b.Property(x => x.LimitUnitId).HasColumnName("LIMIT_UNITID");
        b.Property(x => x.LimitGradeCatId).HasColumnName("LIMIT_GRADECATID").HasMaxLength(3).IsRequired();
        b.Property(x => x.LimitGradeId).HasColumnName("LIMIT_GRADEID");
        b.Property(x => x.LimitElgAmt).HasColumnName("LIMIT_ELGAMT");
        b.Property(x => x.LimitEffDate).HasColumnName("LIMIT_EFFDATE");
        b.Property(x => x.LimitClsDate).HasColumnName("LIMIT_CLSDATE");
        b.Property(x => x.LimitCreatedBy).HasColumnName("LIMIT_CREATEDBY");
        b.Property(x => x.LimitCreatedOn).HasColumnName("LIMIT_CREATEDON");
        b.Property(x => x.LimitModifiedBy).HasColumnName("LIMIT_MODIFIEDBY");
        b.Property(x => x.LimitModifiedOn).HasColumnName("LIMIT_MODIFIEDON");
    }
}

public class MobileAdditionalLimitConfiguration : IEntityTypeConfiguration<MobileAdditionalLimit>
{
    public void Configure(EntityTypeBuilder<MobileAdditionalLimit> b)
    {
        b.ToTable("MOBILE_ADDLIMIT");
        b.HasKey(x => x.AddId);
        b.Property(x => x.AddId).HasColumnName("ADD_ID");
        b.Property(x => x.AddEmpSysId).HasColumnName("ADD_EMPSYSID");
        b.Property(x => x.AddEffDate).HasColumnName("ADD_EFFDATE");
        b.Property(x => x.AddClsDate).HasColumnName("ADD_CLSDATE");
        b.Property(x => x.AddRemarks).HasColumnName("ADD_REMARKS").HasMaxLength(500);
        b.Property(x => x.AddAmt).HasColumnName("ADD_AMT");
        b.Property(x => x.AddCalendarId).HasColumnName("ADD_CALENDARID");
        b.Property(x => x.AddCreatedBy).HasColumnName("ADD_CREATEDBY");
        b.Property(x => x.AddCreatedOn).HasColumnName("ADD_CREATEDON");
        b.Property(x => x.AddModifiedBy).HasColumnName("ADD_MODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.AddModifiedOn).HasColumnName("ADD_MODIFIEDON");
    }
}

public class EmployeeRetiralEmpSpecificConfiguration : IEntityTypeConfiguration<EmployeeRetiralEmpSpecific>
{
    public void Configure(EntityTypeBuilder<EmployeeRetiralEmpSpecific> b)
    {
        b.ToTable("EMP_RETIRALS_EMPSPECIFIC");
        b.HasKey(x => x.EmpRetId);
        b.Property(x => x.EmpRetId).HasColumnName("EMPRET_ID");
        b.Property(x => x.EmpRetEmpSysId).HasColumnName("EMPRET_EMPSYSID");
        b.Property(x => x.EmpRetPayType).HasColumnName("EMPRET_PAYTYPE").HasMaxLength(3).IsRequired();
        b.Property(x => x.EmpRetEdId).HasColumnName("EMPRET_EDID");
        b.Property(x => x.EmpRetEffDate).HasColumnName("EMPRET_EFFDATE");
        b.Property(x => x.EmpRetClsDate).HasColumnName("EMPRET_CLSDATE");
        b.Property(x => x.EmpRetPercentage).HasColumnName("EMPRET_PERCENTAGE");
        b.Property(x => x.EmpRetCreatedBy).HasColumnName("EMPRET_CREATEDBY");
        b.Property(x => x.EmpRetCreatedOn).HasColumnName("EMPRET_CREATEDON");
        b.Property(x => x.EmpRetModifiedBy).HasColumnName("EMPRET_MODIFIEDBY");
        b.Property(x => x.EmpRetModifiedOn).HasColumnName("EMPRET_MODIFIEDON");
    }
}

public class EmployeeRetiralDetailConfiguration : IEntityTypeConfiguration<EmployeeRetiralDetail>
{
    public void Configure(EntityTypeBuilder<EmployeeRetiralDetail> b)
    {
        b.ToTable("EMP_RETIRALSDET");
        b.HasKey(x => x.ErDetId);
        b.Property(x => x.ErDetId).HasColumnName("ERDET_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.ErDetEmpSysId).HasColumnName("ERDET_EMPSYSID").HasColumnType("decimal(38,0)");
        b.Property(x => x.ErDetPfClsDate).HasColumnName("ERDET_PFCLSDATE");
        b.Property(x => x.ErDetRemarks).HasColumnName("ERDET_REMARKS").HasMaxLength(200).IsRequired();
        b.Property(x => x.ErDetModifiedBy).HasColumnName("ERDET_MODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.ErDetModifiedOn).HasColumnName("ERDET_MODIFIEDON");
    }
}

public class BasicSlabIncrementConfiguration : IEntityTypeConfiguration<BasicSlabIncrement>
{
    public void Configure(EntityTypeBuilder<BasicSlabIncrement> b)
    {
        b.ToTable("BASIC_SLABINC");
        b.HasKey(x => x.SlabIncId);
        b.Property(x => x.SlabIncId).HasColumnName("SLAB_INCID");
        b.Property(x => x.SlabGradeId).HasColumnName("SLAB_GRADEID");
        b.Property(x => x.SlabUnitId).HasColumnName("SLAB_UNITID");
        b.Property(x => x.SlabIncStrtDate).HasColumnName("SLAB_INCSTRTDATE");
        b.Property(x => x.SlabIncClsDate).HasColumnName("SLAB_INCCLSDATE");
        b.Property(x => x.SlabIncModifiedBy).HasColumnName("SLAB_INCMODIFIEDBY");
        b.Property(x => x.SlabIncModifiedOn).HasColumnName("SLAB_INCMODIFIEDON");
    }
}

public class CompensationParameterConfiguration : IEntityTypeConfiguration<CompensationParameter>
{
    public void Configure(EntityTypeBuilder<CompensationParameter> b)
    {
        b.ToTable("COMP_PARAMS");
        b.HasKey(x => x.CpId);
        b.Property(x => x.CpId).HasColumnName("CP_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CpCountryCode).HasColumnName("CP_COUNTRYCODE").HasMaxLength(3).IsRequired();
        b.Property(x => x.CpEdGroup).HasColumnName("CP_EDGROUP").HasMaxLength(3).IsRequired();
        b.Property(x => x.CpType).HasColumnName("CP_TYPE").HasMaxLength(3).IsRequired();
        b.Property(x => x.CpEdId).HasColumnName("CP_EDID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CpModifiedBy).HasColumnName("CP_MODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.CpModifiedOn).HasColumnName("CP_MODIFIEDON");
    }
}

public class DiligenceRateMasterConfiguration : IEntityTypeConfiguration<DiligenceRateMaster>
{
    public void Configure(EntityTypeBuilder<DiligenceRateMaster> b)
    {
        b.ToTable("DILIGENCE_RATEMAST");
        b.HasKey(x => x.DiligenceId);
        b.Property(x => x.DiligenceId).HasColumnName("DILIGENCE_ID");
        b.Property(x => x.DiligencePayUnitId).HasColumnName("DILIGENCE_PAYUNITID");
        b.Property(x => x.DiligenceGradeCategory).HasColumnName("DILIGENCE_GRADECATEGORY").HasMaxLength(3).IsRequired();
        b.Property(x => x.DiligenceEdId).HasColumnName("DILIGENCE_EDID");
        b.Property(x => x.DiligenceYearId).HasColumnName("DILIGENCE_YEARID");
        b.Property(x => x.DiligenceAmount).HasColumnName("DILIGENCE_AMOUNT").HasColumnType("decimal(19,0)");
        b.Property(x => x.DiligenceEffDate).HasColumnName("DILIGENCE_EFFDATE");
        b.Property(x => x.DiligenceClsDate).HasColumnName("DILIGENCE_CLSDATE");
        b.Property(x => x.DiligenceLastModifiedBy).HasColumnName("DILIGENCE_LASTMODIFIEDBY");
        b.Property(x => x.DiligenceLastModifiedOn).HasColumnName("DILIGENCE_LASTMODIFIEDON");
        b.Property(x => x.DiligenceBenLogId).HasColumnName("DILIGENCE_BENLOGID");
    }
}

public class PmsCashPayConfiguration : IEntityTypeConfiguration<PmsCashPay>
{
    public void Configure(EntityTypeBuilder<PmsCashPay> b)
    {
        b.ToTable("PMS_CASHPAY");
        b.HasKey(x => x.CashPayId);
        b.Property(x => x.CashPayId).HasColumnName("CASHPAY_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CashPayUnitId).HasColumnName("CASHPAY_UNITID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CashPayGradeCat).HasColumnName("CASHPAY_GRADECAT").HasMaxLength(5).IsRequired();
        b.Property(x => x.CashPayPayType).HasColumnName("CASHPAY_PAYTYPE").HasMaxLength(1).IsRequired();
        b.Property(x => x.CashPayEffDate).HasColumnName("CASHPAY_EFFDATE");
        b.Property(x => x.CashPayClsDate).HasColumnName("CASHPAY_CLSDATE");
        b.Property(x => x.CashPayModifiedBy).HasColumnName("CASHPAY_MODIFIEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.CashPayModifiedOn).HasColumnName("CASHPAY_MODIFIEDON");

        b.HasMany(x => x.Details)
            .WithOne(d => d.CashPay)
            .HasForeignKey(d => d.CashPayId);
    }
}

public class PmsCashPayDetailConfiguration : IEntityTypeConfiguration<PmsCashPayDetail>
{
    public void Configure(EntityTypeBuilder<PmsCashPayDetail> b)
    {
        b.ToTable("PMS_CASHPAYDET");
        b.HasKey(x => x.CashPayDetId);
        b.Property(x => x.CashPayDetId).HasColumnName("CASHPAY_DETID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CashPayId).HasColumnName("CASHPAY_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CashPayPer).HasColumnName("CASHPAY_PER").HasColumnType("decimal(38,0)");
        b.Property(x => x.CashPayPayDate).HasColumnName("CASHPAY_PAYDATE").HasMaxLength(20).IsRequired();
    }
}

public class EmployeeCtcRemarksConfiguration : IEntityTypeConfiguration<EmployeeCtcRemarks>
{
    public void Configure(EntityTypeBuilder<EmployeeCtcRemarks> b)
    {
        b.ToTable("EMPLOYEE_CTCREMARKS");
        b.HasKey(x => x.CtcRemEmpSysId);
        b.Property(x => x.CtcRemEmpSysId).HasColumnName("CTCREM_EMP_SYSID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcRemId).HasColumnName("CTCREM_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcRemLine1).HasColumnName("CTCREM_LINE1").HasMaxLength(200);
        b.Property(x => x.CtcRemLine2).HasColumnName("CTCREM_LINE2").HasMaxLength(200);
        b.Property(x => x.CtcRemLine3).HasColumnName("CTCREM_LINE3").HasMaxLength(200);
        b.Property(x => x.CtcRemUpdatedBy).HasColumnName("CTCREM_UPDATEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcRemUpdatedOn).HasColumnName("CTCREM_UPDATEDON");
    }
}

public class TevCtcConfiguration : IEntityTypeConfiguration<TevCtc>
{
    public void Configure(EntityTypeBuilder<TevCtc> b)
    {
        b.ToTable("TEVCTC");
        b.HasNoKey();
        b.Property(x => x.CtcEmpSysId).HasColumnName("CTC_EMP_SYSID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcId).HasColumnName("CTC_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcEffDat).HasColumnName("CTC_EFF_DAT");
        b.Property(x => x.CtcClsDat).HasColumnName("CTC_CLS_DAT");
        b.Property(x => x.CtcEdId).HasColumnName("CTC_ED_ID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcEdFreq).HasColumnName("CTC_ED_FREQ").HasMaxLength(1).IsRequired();
        b.Property(x => x.CtcEdAmtPa).HasColumnName("CTC_ED_AMTPA").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcTranNo).HasColumnName("CTC_TRANNO").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcSource).HasColumnName("CTC_SOURCE").HasMaxLength(3).IsRequired();
        b.Property(x => x.CtcStructureId).HasColumnName("CTC_STRUCTUREID").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcUpdatedBy).HasColumnName("CTC_UPDATEDBY").HasColumnType("decimal(38,0)");
        b.Property(x => x.CtcUpdatedOn).HasColumnName("CTC_UPDATEDON");
        b.Property(x => x.CtcFormula).HasColumnName("CTC_FORMULA").HasMaxLength(1).IsRequired();
        b.Property(x => x.CtcLogNo).HasColumnName("CTC_LOGNO").HasColumnType("decimal(22,0)");
    }
}
