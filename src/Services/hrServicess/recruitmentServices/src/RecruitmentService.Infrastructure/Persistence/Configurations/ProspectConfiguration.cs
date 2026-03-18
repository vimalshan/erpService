using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Infrastructure.Persistence.Configurations;

public class ProspectConfiguration : IEntityTypeConfiguration<Prospect>
{
    public void Configure(EntityTypeBuilder<Prospect> builder)
    {
        builder.ToTable("WEBPROSPECT_MAST");

        builder.HasKey(p => p.WebUserId);
        builder.Property(p => p.WebUserId).HasColumnName("WEBUSER_ID").HasColumnType("DECIMAL(38)");
        builder.Property(p => p.Password).HasColumnName("WEBUSER_PWD").HasMaxLength(10);
        builder.Property(p => p.FirstName).HasColumnName("WEBUSER_FRS_NAME").HasMaxLength(65);
        builder.Property(p => p.MiddleName).HasColumnName("WEBUSER_MID_NAME").HasMaxLength(65);
        builder.Property(p => p.LastName).HasColumnName("WEBUSER_LST_NAME").HasMaxLength(65);
        builder.Property(p => p.EmailId).HasColumnName("WEBUSER_EMAILID").HasMaxLength(200);
        builder.Property(p => p.Status).HasColumnName("WEBUSER_STATUS")
            .HasConversion(s => s.ToCode(), c => ProspectStatusExtensions.FromCode(c))
            .HasColumnType("CHAR(1)");
        builder.Property(p => p.DateOfBirth).HasColumnName("WEBUSER_DATEOFBIRTH").HasColumnType("DATETIME2(3)");
        builder.Property(p => p.CreatedOn).HasColumnName("WEBUSER_CREATEDON").HasColumnType("DATETIME2(3)");
        builder.Property(p => p.ProspectType).HasColumnName("WEBUSER_TYPE").HasColumnType("CHAR(1)");

        builder.Ignore(p => p.DomainEvents);
        builder.Ignore(p => p.FullName);

        builder.HasMany(p => p.Addresses).WithOne().HasForeignKey(a => a.EmpSysId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.Qualifications).WithOne().HasForeignKey(q => q.EmpSysId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.References).WithOne().HasForeignKey(r => r.EmpSysId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(p => p.Trainings).WithOne().HasForeignKey(t => t.EmpSysId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProspectAddressConfiguration : IEntityTypeConfiguration<ProspectAddress>
{
    public void Configure(EntityTypeBuilder<ProspectAddress> builder)
    {
        builder.ToTable("PROSPECT_ADDRESS");
        builder.HasKey(a => new { a.EmpSysId, a.AddressFlag });
        builder.Property(a => a.EmpSysId).HasColumnName("ADDRESS_EMP_SYSID").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.AddressFlag).HasColumnName("ADDRESS_FLAG").HasColumnType("CHAR(1)");
        builder.Property(a => a.Address1).HasColumnName("ADDRESS_1").HasMaxLength(65);
        builder.Property(a => a.Address2).HasColumnName("ADDRESS_2").HasMaxLength(65);
        builder.Property(a => a.Address3).HasColumnName("ADDRESS_3").HasMaxLength(65);
        builder.Property(a => a.Address4).HasColumnName("ADDRESS_4").HasMaxLength(65);
        builder.Property(a => a.City).HasColumnName("ADDRESS_CITY").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.PinCode).HasColumnName("ADDRESS_PINCODE").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.UpdatedBy).HasColumnName("ADDRESS_UPDATED_BY").HasColumnType("DECIMAL(38)");
        builder.Property(a => a.UpdatedOn).HasColumnName("ADDRESS_UPDATED_ON").HasColumnType("DATETIME2(3)");
        builder.Property(a => a.MobileNo).HasColumnName("ADDRESS_MOBNO").HasMaxLength(65);
        builder.Property(a => a.LandlineNo).HasColumnName("ADDRESS_LANDLINE").HasMaxLength(65);
    }
}

public class ProspectQualificationConfiguration : IEntityTypeConfiguration<ProspectQualification>
{
    public void Configure(EntityTypeBuilder<ProspectQualification> builder)
    {
        builder.ToTable("PROSPECT_QUALIFICATION");
        builder.HasKey(q => new { q.EmpSysId, q.QualId });
        builder.Property(q => q.EmpSysId).HasColumnName("QUAL_EMP_SYSID").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.QualId).HasColumnName("QUAL_ID").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.QualCode).HasColumnName("QUAL_CODE").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.QualDescription).HasColumnName("QUAL_DESC").HasMaxLength(65);
        builder.Property(q => q.YearFrom).HasColumnName("QUAL_YEARFRO").HasColumnType("CHAR(6)");
        builder.Property(q => q.YearTo).HasColumnName("QUAL_YEARTO").HasColumnType("CHAR(6)");
        builder.Property(q => q.InstitutionCode).HasColumnName("QUAL_INST_CODE").HasColumnType("DECIMAL(22,0)");
        builder.Property(q => q.InstitutionDescription).HasColumnName("QUAL_INST_DESC").HasMaxLength(65);
        builder.Property(q => q.EducationType).HasColumnName("QUAL_EDU_TYPE").HasColumnType("CHAR(1)");
        builder.Property(q => q.SpecializationCode).HasColumnName("QUAL_SPE_CODE").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.SpecializationDescription).HasColumnName("QUAL_SPE_DESC").HasMaxLength(65);
        builder.Property(q => q.Percentage).HasColumnName("QUAL_PERCENTAGE").HasMaxLength(10);
        builder.Property(q => q.DegreeCode).HasColumnName("QUAL_DEGREE_CODE").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.DegreeDescription).HasColumnName("QUAL_DEGREE_DESC").HasMaxLength(65);
        builder.Property(q => q.UpdatedBy).HasColumnName("QUAL_UPDATEDBY").HasColumnType("DECIMAL(38)");
        builder.Property(q => q.UpdatedOn).HasColumnName("QUAL_UPDATEDON").HasColumnType("DATETIME2(3)");
    }
}

public class ProspectReferenceConfiguration : IEntityTypeConfiguration<ProspectReference>
{
    public void Configure(EntityTypeBuilder<ProspectReference> builder)
    {
        builder.ToTable("PROSPECT_REFERENCE");
        builder.HasKey(r => new { r.EmpSysId, r.RefId });
        builder.Property(r => r.EmpSysId).HasColumnName("REF_EMP_SYS_ID").HasColumnType("DECIMAL(38)");
        builder.Property(r => r.RefId).HasColumnName("REF_ID").HasColumnType("DECIMAL(38)");
        builder.Property(r => r.Name).HasColumnName("REF_NAME").HasMaxLength(65);
        builder.Property(r => r.Designation).HasColumnName("REF_DESGN").HasMaxLength(65);
        builder.Property(r => r.Address1).HasColumnName("REF_ADDRESS1").HasMaxLength(200);
        builder.Property(r => r.Address2).HasColumnName("REF_ADDRESS2").HasMaxLength(200);
        builder.Property(r => r.Phone).HasColumnName("REF_PHONE").HasMaxLength(50);
        builder.Property(r => r.Email).HasColumnName("REF_EMAIL").HasMaxLength(200);
    }
}

public class ProspectTrainingConfiguration : IEntityTypeConfiguration<ProspectTraining>
{
    public void Configure(EntityTypeBuilder<ProspectTraining> builder)
    {
        builder.ToTable("PROSPECT_TRAINING");
        builder.HasKey(t => new { t.EmpSysId, t.TrainingId });
        builder.Property(t => t.EmpSysId).HasColumnName("TRAINING_EMP_SYSID").HasColumnType("DECIMAL(38)");
        builder.Property(t => t.TrainingId).HasColumnName("TRAINING_ID").HasColumnType("DECIMAL(38)");
        builder.Property(t => t.Title).HasColumnName("TRAINING_TITLE").HasMaxLength(2000);
        builder.Property(t => t.Duration).HasColumnName("TRAINING_DURATION").HasMaxLength(2000);
        builder.Property(t => t.Institute).HasColumnName("TRAINING_INSTITUTE").HasMaxLength(2000);
        builder.Property(t => t.Location).HasColumnName("TRAINING_LOCATION").HasMaxLength(2000);
    }
}
