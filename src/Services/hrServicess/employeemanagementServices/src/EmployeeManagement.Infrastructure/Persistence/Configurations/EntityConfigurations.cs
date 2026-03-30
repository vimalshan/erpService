using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

public class EmployeeLanguageConfiguration : IEntityTypeConfiguration<EmployeeLanguage>
{
    public void Configure(EntityTypeBuilder<EmployeeLanguage> builder)
    {
        builder.ToTable("EMPLOYEE_LANGUAGE");
        builder.HasKey(e => new { e.EmployeeId, e.LanguageId });
        builder.Property(e => e.EmployeeId).HasColumnName("LANG_EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.LanguageId).HasColumnName("LANG_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.LanguageType).HasColumnName("LANG_TYPE").HasMaxLength(3);
        builder.Property(e => e.UpdatedBy).HasColumnName("LANG_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("LANG_UPDATEDON");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class EmployeeDiaryConfiguration : IEntityTypeConfiguration<EmployeeDiary>
{
    public void Configure(EntityTypeBuilder<EmployeeDiary> builder)
    {
        builder.ToTable("EMPLOYEE_DIARY");
        builder.HasKey(e => new { e.EmployeeId, e.DiaryId });
        builder.Property(e => e.EmployeeId).HasColumnName("DIARY_EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DiaryId).HasColumnName("DIARY_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DiaryType).HasColumnName("DIARY_TYPE").HasMaxLength(1);
        builder.Property(e => e.SubType).HasColumnName("DIARY_SUBTYPE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DiaryDate).HasColumnName("DIARY_DATE").HasMaxLength(6);
        builder.Property(e => e.Reason).HasColumnName("DIARY_REASON").HasMaxLength(200);
        builder.Property(e => e.UpdatedBy).HasColumnName("DIARY_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("DIARY_UPDATEDON");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("EMPLOYEE_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("EMP_SYSID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.EmployeeNo).HasColumnName("EMP_NO").HasMaxLength(20);
        builder.Property(e => e.BusinessUnit).HasColumnName("EMP_BUSINESS").HasMaxLength(9);
        builder.Property(e => e.Unit).HasColumnName("EMP_UNIT").HasMaxLength(3);
        builder.Property(e => e.GradeId).HasColumnName("EMP_GRADE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Designation).HasColumnName("EMP_DESIGNATION").HasMaxLength(50);
        builder.Property(e => e.DivisionId).HasColumnName("EMP_DIVISION").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DepartmentId).HasColumnName("EMP_DEPARTMENT").HasColumnType("decimal(38,0)");
        builder.Property(e => e.PositionId).HasColumnName("EMP_POSITION").HasColumnType("decimal(38,0)");
        builder.Property(e => e.IsActive).HasColumnName("EMP_ISACTIVE");
        builder.Property(e => e.CreatedOn).HasColumnName("EMP_CREATEDON");
        builder.Property(e => e.CreatedBy).HasColumnName("EMP_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("EMP_UPDATEDON");
        builder.Property(e => e.UpdatedBy).HasColumnName("EMP_UPDATEDBY").HasColumnType("decimal(38,0)");
        // Navigation properties — loaded separately by repository
        builder.Ignore(e => e.CurrentAddress);
        builder.Ignore(e => e.PermanentAddress);
        builder.Ignore(e => e.Probation);
        builder.Ignore(e => e.Retiral);
        builder.Ignore(e => e.Qualifications);
        builder.Ignore(e => e.Careers);
        builder.Ignore(e => e.Languages);
        builder.Ignore(e => e.Diaries);
        builder.Ignore(e => e.Promotions);
        builder.Ignore(e => e.Transfers);
        builder.Ignore(e => e.DomainEvents);
    }
}

public class EmployeeAddressConfiguration : IEntityTypeConfiguration<EmployeeAddress>
{
    public void Configure(EntityTypeBuilder<EmployeeAddress> builder)
    {
        builder.ToTable("EMPLOYEE_ADDRESS");
        builder.HasKey(e => e.EmployeeId);
        builder.Property(e => e.EmployeeId).HasColumnName("ADDRESS_EMP_SYSID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.AddressFlag).HasColumnName("ADDRESS_FLAG").HasMaxLength(1);
        builder.Property(e => e.Line1).HasColumnName("ADDRESS_1").HasMaxLength(65);
        builder.Property(e => e.Line2).HasColumnName("ADDRESS_2").HasMaxLength(65);
        builder.Property(e => e.Line3).HasColumnName("ADDRESS_3").HasMaxLength(65);
        builder.Property(e => e.Line4).HasColumnName("ADDRESS_4").HasMaxLength(65);
        builder.Property(e => e.CityId).HasColumnName("ADDRESS_CITY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CityOthers).HasColumnName("ADDRESS_CITYOTHERS").HasMaxLength(65);
        builder.Property(e => e.PinCode).HasColumnName("ADDRESS_PINCODE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.StateId).HasColumnName("ADDRESS_STATE").HasColumnType("decimal(22,0)");
        builder.Property(e => e.UpdatedBy).HasColumnName("ADDRESS_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("ADDRESS_UPDATEDON");
    }
}

public class EmployeeCareerConfiguration : IEntityTypeConfiguration<EmployeeCareer>
{
    public void Configure(EntityTypeBuilder<EmployeeCareer> builder)
    {
        builder.ToTable("EMPLOYEE_CAREER");
        builder.HasKey(e => e.CareerId);
        builder.Property(e => e.CareerId).HasColumnName("CAREER_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.EmployeeId).HasColumnName("CAREER_EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Business).HasColumnName("CAREER_BUSINESS").HasMaxLength(9);
        builder.Property(e => e.Unit).HasColumnName("CAREER_UNIT").HasMaxLength(3);
        builder.Property(e => e.From).HasColumnName("CAREER_FROM");
        builder.Property(e => e.To).HasColumnName("CAREER_TO");
        builder.Property(e => e.EmployeeNo).HasColumnName("CAREER_EMPNO").HasMaxLength(20);
        builder.Property(e => e.GradeId).HasColumnName("CAREER_GRADE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.GradeOther).HasColumnName("CAREER_GRADEOTH").HasMaxLength(50);
        builder.Property(e => e.Designation).HasColumnName("CAREER_DESIGNATION").HasMaxLength(50);
        builder.Property(e => e.DivisionId).HasColumnName("CAREER_DIVISION").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DivisionOther).HasColumnName("CAREER_DIVISIONOTH").HasMaxLength(50);
        builder.Property(e => e.ProcessId).HasColumnName("CAREER_PROCESS").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ProcessOther).HasColumnName("CAREER_PROCESSOTH").HasMaxLength(50);
        builder.Property(e => e.DepartmentId).HasColumnName("CAREER_DEPARTMENT").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DepartmentOther).HasColumnName("CAREER_DEPARTMENTOTH").HasMaxLength(50);
        builder.Property(e => e.Reason).HasColumnName("CAREER_REASON").HasMaxLength(150);
        builder.Property(e => e.ModifiedBy).HasColumnName("CAREER_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ModifiedOn).HasColumnName("CAREER_MODIFIEDON");
    }
}

public class EmployeeQualificationConfiguration : IEntityTypeConfiguration<EmployeeQualification>
{
    public void Configure(EntityTypeBuilder<EmployeeQualification> builder)
    {
        builder.ToTable("EMPLOYEE_QUALIFICATION");
        builder.HasKey(e => e.QualificationId);
        builder.Property(e => e.QualificationId).HasColumnName("QUAL_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.EmployeeId).HasColumnName("QUAL_EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.QualCode).HasColumnName("QUAL_CODE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.QualDescription).HasColumnName("QUAL_DESC").HasMaxLength(65);
        builder.Property(e => e.YearFrom).HasColumnName("QUAL_YEARFRO").HasMaxLength(6);
        builder.Property(e => e.YearTo).HasColumnName("QUAL_YEARTO").HasMaxLength(6);
        builder.Property(e => e.InstitutionCode).HasColumnName("QUAL_INST_CODE").HasColumnType("decimal(22,0)");
        builder.Property(e => e.InstitutionDesc).HasColumnName("QUAL_INST_DESC").HasMaxLength(65);
        builder.Property(e => e.EducationType).HasColumnName("QUAL_EDU_TYPE").HasMaxLength(1);
        builder.Property(e => e.SpecializationCode).HasColumnName("QUAL_SPE_CODE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.SpecializationDesc).HasColumnName("QUAL_SPE_DESC").HasMaxLength(65);
        builder.Property(e => e.Percentage).HasColumnName("QUAL_PERCENTAGE").HasMaxLength(10);
        builder.Property(e => e.DegreeCode).HasColumnName("QUAL_DEGREE_CODE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.DegreeDesc).HasColumnName("QUAL_DEGREE_DESC").HasMaxLength(65);
        builder.Property(e => e.UpdatedBy).HasColumnName("QUAL_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("QUAL_UPDATEDON");
    }
}

public class EmployeePromotionConfiguration : IEntityTypeConfiguration<EmployeePromotion>
{
    public void Configure(EntityTypeBuilder<EmployeePromotion> builder)
    {
        builder.ToTable("EMPLOYEE_PROMOTION");
        builder.HasKey(e => e.PromotionNo);
        builder.Property(e => e.PromotionNo).HasColumnName("PROM_NO").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.Source).HasColumnName("PROM_SOURCE").HasMaxLength(3);
        builder.Property(e => e.RequestNo).HasColumnName("PROM_REQUESTNO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.RecommendationDate).HasColumnName("PROM_RECDATE");
        builder.Property(e => e.EmployeeId).HasColumnName("PROM_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.OldGradeId).HasColumnName("PROM_OLDGRADE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.NewGradeId).HasColumnName("PROM_NEWGRADE").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Status).HasColumnName("PROM_STATUS").HasMaxLength(1);
        builder.Property(e => e.OldPositionId).HasColumnName("PROM_OLDPOSITION").HasColumnType("decimal(38,0)");
        builder.Property(e => e.NewPositionId).HasColumnName("PROM_NEWPOSITION").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ReasonId).HasColumnName("PROM_REASON").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Remarks).HasColumnName("PROM_REMARKS").HasMaxLength(400);
        builder.Property(e => e.ConfirmationDate).HasColumnName("PROM_CNFDATE");
        builder.Property(e => e.RevisionStatus).HasColumnName("PROM_REVISIONSTATUS").HasMaxLength(1);
        builder.Property(e => e.IncrementNo).HasColumnName("PROM_INCREMENTNO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Designation).HasColumnName("PROM_DESIGNATION").HasMaxLength(65);
        builder.Property(e => e.PromotionType).HasColumnName("PROM_TYPE").HasMaxLength(1);
        builder.Property(e => e.CreatedOn).HasColumnName("PROM_CREATEDON");
        builder.Property(e => e.CreatedBy).HasColumnName("PROM_CREATEDBY").HasColumnType("decimal(22,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("PROM_UPDATEDON");
        builder.Property(e => e.UpdatedBy).HasColumnName("PROM_UPDATEDBY").HasColumnType("decimal(22,0)");
    }
}

public class EmployeeTransferConfiguration : IEntityTypeConfiguration<EmployeeTransfer>
{
    public void Configure(EntityTypeBuilder<EmployeeTransfer> builder)
    {
        builder.ToTable("TRANSFER_MAIN");
        builder.HasKey(e => e.TransferId);
        builder.Property(e => e.TransferId).HasColumnName("TRANSFER_ID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.EmployeeId).HasColumnName("TRANSFER_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.OldUnit).HasColumnName("TRANSFER_OLDUNIT").HasMaxLength(3);
        builder.Property(e => e.NewUnit).HasColumnName("TRANSFER_NEWUNIT").HasMaxLength(3);
        builder.Property(e => e.OldUnitId).HasColumnName("TRANSFER_OLDUNITID").HasColumnType("decimal(22,0)");
        builder.Property(e => e.NewUnitId).HasColumnName("TRANSFER_NEWUNITID").HasColumnType("decimal(22,0)");
        builder.Property(e => e.ReasonId).HasColumnName("TRANSFER_REASON").HasColumnType("decimal(38,0)");
        builder.Property(e => e.TransferDate).HasColumnName("TRANSFER_DATE");
        builder.Property(e => e.Remarks).HasColumnName("TRANSFER_REMARKS").HasMaxLength(200);
        builder.Property(e => e.PayrollTransfer).HasColumnName("TRANSFER_PAYFLAG");
        builder.Property(e => e.TransferType).HasColumnName("TRANSFER_TYPE").HasMaxLength(2);
        builder.Property(e => e.Status).HasColumnName("TRANSFER_STATUS").HasMaxLength(2);
        builder.Property(e => e.ExpatStatus).HasColumnName("TRANSFER_EXPATSTATUS").HasMaxLength(1);
        builder.Property(e => e.CreatedBy).HasColumnName("TRANSFER_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.CreatedOn).HasColumnName("TRANSFER_CREATEDON");
        builder.Property(e => e.UpdatedBy).HasColumnName("TRANSFER_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("TRANSFER_UPDATEDON");
        builder.Ignore(e => e.TimeOfficeTransfer);
    }
}

public class EmployeeProbationConfiguration : IEntityTypeConfiguration<EmployeeProbation>
{
    public void Configure(EntityTypeBuilder<EmployeeProbation> builder)
    {
        builder.ToTable("EMP_PROBATIONKK");
        builder.HasKey(e => e.ProbationId);
        builder.Property(e => e.ProbationId).HasColumnName("PROBATION_ID").ValueGeneratedNever();
        builder.Property(e => e.EmployeeId).HasColumnName("PROBATION_EMPSYSID");
        builder.Property(e => e.UnitId).HasColumnName("PROBATION_UNITID");
        builder.Property(e => e.GradeId).HasColumnName("PROBATION_GRADE");
        builder.Property(e => e.DueDate).HasColumnName("PROBATION_DUEDATE");
        builder.Property(e => e.ProbationStatus).HasColumnName("PROBATION_PROBATIONSTATUS").HasMaxLength(1);
        builder.Property(e => e.IsExtended).HasColumnName("PROBATION_EXTENDED");
        builder.Property(e => e.ProbationDate).HasColumnName("PROBATION_DATE");
        builder.Property(e => e.SalaryChange).HasColumnName("PROBATION_SALARYCHANGE").HasMaxLength(1);
        builder.Property(e => e.GradeChange).HasColumnName("PROBATION_GRADECHANGE").HasMaxLength(1);
        builder.Property(e => e.Rating).HasColumnName("PROBATION_RATING").HasMaxLength(20);
        builder.Property(e => e.CreatedOn).HasColumnName("PROBATION_CREATEDON");
        builder.Property(e => e.CreatedBy).HasColumnName("PROBATION_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.LastModifiedBy).HasColumnName("PROBATION_LASTMODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.LastModifiedOn).HasColumnName("PROBATION_LASTMODIFIEDON");
    }
}

public class EmployeeRetiralConfiguration : IEntityTypeConfiguration<EmployeeRetiral>
{
    public void Configure(EntityTypeBuilder<EmployeeRetiral> builder)
    {
        builder.ToTable("EMP_RETIRALS");
        builder.HasKey(e => e.EmployeeId);
        builder.Property(e => e.EmployeeId).HasColumnName("RETIRAL_EMP_SYSID").HasColumnType("decimal(38,0)").ValueGeneratedNever();
        builder.Property(e => e.TransactionId).HasColumnName("RETIRAL_TRANID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.PfApplicable).HasColumnName("RETIRAL_PFAPPLICABLE").HasMaxLength(1);
        builder.Property(e => e.PfTrust).HasColumnName("RETIRAL_PFTRUST").HasMaxLength(3);
        builder.Property(e => e.PfNo).HasColumnName("RETIRAL_PFNO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.GratuityApplicable).HasColumnName("RETIRAL_GRATUITYAPP").HasMaxLength(1);
        builder.Property(e => e.EsiApplicable).HasColumnName("RETIRAL_ESIAPP").HasMaxLength(1);
        builder.Property(e => e.EsiNo).HasColumnName("RETIRAL_ESINO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.EffectiveDate).HasColumnName("RETIRAL_EFF_DATE");
        builder.Property(e => e.ClosureDate).HasColumnName("RETIRAL_CLS_DATE");
        builder.Property(e => e.UpdatedBy).HasColumnName("RETIRAL_UPDATED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("RETIRAL_UPDATED_ON");
        builder.Ignore(e => e.AdditionalPf);
        builder.Ignore(e => e.AdditionalPfPercent);
        builder.Ignore(e => e.SuperannuationApplicable);
        builder.Ignore(e => e.SuperannuationOption);
        builder.Ignore(e => e.SuperannuationNo);
    }
}
