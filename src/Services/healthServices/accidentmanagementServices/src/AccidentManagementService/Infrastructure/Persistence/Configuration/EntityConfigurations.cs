using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccidentManagementService.Domain.Entities;

namespace AccidentManagementService.Infrastructure.Persistence.Configuration
{
    /// <summary>
    /// EF Core Configuration for AccidentSeverity entity
    /// </summary>
    public class AccidentSeverityConfiguration : IEntityTypeConfiguration<AccidentSeverity>
    {
        public void Configure(EntityTypeBuilder<AccidentSeverity> builder)
        {
            builder.ToTable("ACCIDENT_SEVERITY");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("SEVERITY_ID");
            builder.Property(x => x.Guid).HasColumnName("SEVERITY_GUID").IsRequired();
            builder.Property(x => x.Code).HasColumnName("SEVERITY_CODE").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Name).HasColumnName("SEVERITY_NAME").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(200);

            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            builder.HasIndex(x => x.Guid).IsUnique();
            builder.HasIndex(x => x.Code).IsUnique();
        }
    }

    /// <summary>
    /// EF Core Configuration for AccidentStatus entity
    /// </summary>
    public class AccidentStatusConfiguration : IEntityTypeConfiguration<AccidentStatus>
    {
        public void Configure(EntityTypeBuilder<AccidentStatus> builder)
        {
            builder.ToTable("ACCIDENT_STATUS");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("STATUS_ID");
            builder.Property(x => x.Guid).HasColumnName("STATUS_GUID").IsRequired();
            builder.Property(x => x.Code).HasColumnName("STATUS_CODE").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Name).HasColumnName("STATUS_NAME").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(200);

            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            builder.HasIndex(x => x.Guid).IsUnique();
            builder.HasIndex(x => x.Code).IsUnique();
        }
    }

    /// <summary>
    /// EF Core Configuration for InjuryCategory entity
    /// </summary>
    public class InjuryCategoryConfiguration : IEntityTypeConfiguration<InjuryCategory>
    {
        public void Configure(EntityTypeBuilder<InjuryCategory> builder)
        {
            builder.ToTable("CATEGORY_INJURY");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("CAT_ID");
            builder.Property(x => x.Guid).HasColumnName("CAT_GUID").IsRequired();
            builder.Property(x => x.Name).HasColumnName("CAT_NAME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(200);

            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            builder.HasIndex(x => x.Guid).IsUnique();
        }
    }

    /// <summary>
    /// EF Core Configuration for InjuryNature entity
    /// </summary>
    public class InjuryNatureConfiguration : IEntityTypeConfiguration<InjuryNature>
    {
        public void Configure(EntityTypeBuilder<InjuryNature> builder)
        {
            builder.ToTable("NATURE_INJURY");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("NATURE_ID");
            builder.Property(x => x.Guid).HasColumnName("NATURE_GUID").IsRequired();
            builder.Property(x => x.Name).HasColumnName("NATURE_NAME").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasMaxLength(200);

            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            builder.HasIndex(x => x.Guid).IsUnique();
        }
    }

    /// <summary>
    /// EF Core Configuration for Contractor entity
    /// </summary>
    public class ContractorConfiguration : IEntityTypeConfiguration<Contractor>
    {
        public void Configure(EntityTypeBuilder<Contractor> builder)
        {
            builder.ToTable("ACC_CONTRCT_LST");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ACL_ID");
            builder.Property(x => x.Guid).HasColumnName("ACL_GUID").IsRequired();
            builder.Property(x => x.Name).HasColumnName("ACL_CONT_NAM").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ContractorId).HasColumnName("ACL_CONT_ID").IsRequired();
            builder.Property(x => x.Status).HasColumnName("ACL_STATUS").HasDefaultValue(ContractorStatusEnum.Active);

            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            builder.HasIndex(x => x.Guid).IsUnique();
            builder.HasIndex(x => x.ContractorId);
        }
    }

    /// <summary>
    /// EF Core Configuration for InjuredPerson entity
    /// </summary>
    public class InjuredPersonConfiguration : IEntityTypeConfiguration<InjuredPerson>
    {
        public void Configure(EntityTypeBuilder<InjuredPerson> builder)
        {
            builder.ToTable("ACC_PERS_INJ");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("API_ID");
            builder.Property(x => x.Guid).HasColumnName("API_GUID").IsRequired();
            builder.Property(x => x.SerialNumber).HasColumnName("API_SRL_NUM").IsRequired();
            builder.Property(x => x.PersonName).HasColumnName("API_PERS_NAM").HasMaxLength(100).IsRequired();
            builder.Property(x => x.EmployeeStatus).HasColumnName("API_EMP_STATUS").IsRequired();

            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            builder.HasIndex(x => x.Guid).IsUnique();
            builder.HasIndex(x => x.SerialNumber);
        }
    }

    /// <summary>
    /// EF Core Configuration for AccidentReport aggregate root
    /// </summary>
    public class AccidentReportConfiguration : IEntityTypeConfiguration<AccidentReport>
    {
        public void Configure(EntityTypeBuilder<AccidentReport> builder)
        {
            builder.ToTable("DAILY_ACC_FIR");
            builder.HasKey(x => x.Id);

            // Identity and tracking
            builder.Property(x => x.Id).HasColumnName("DAF_ID");
            builder.Property(x => x.Guid).HasColumnName("DAF_GUID").IsRequired();
            
            // Value Object conversion: AccidentNumber -> long
            builder.Property(x => x.AccidentNumber)
                .HasColumnName("DAF_ACC_NUM")
                .IsRequired()
                .HasConversion(
                    v => v.Value,  // Convert AccidentNumber to long for database
                    v => new AccidentNumber(v));  // Convert long back to AccidentNumber from database
            builder.Property(x => x.CompanyCode).HasColumnName("DAF_COM_COD").HasMaxLength(10).IsRequired();

            // Accident core info
            builder.OwnsOne(x => x.AccidentCircumstances, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.Location).HasColumnName("DAF_ACC_LOC").HasMaxLength(255).IsRequired();
                navigationBuilder.Property(x => x.AccidentDateTime).HasColumnName("DAF_ACC_DAT").IsRequired();
                navigationBuilder.Property(x => x.Cause).HasColumnName("DAF_CAU_INC").HasMaxLength(500).IsRequired();
                navigationBuilder.Property(x => x.PreventiveMeasures).HasColumnName("DAF_PRV_MES").HasMaxLength(500).IsRequired();
            });

            // Injured person info
            builder.OwnsOne(x => x.InjuredPersonInfo, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.PersonName).HasColumnName("DAF_PERS_NAM").HasMaxLength(100).IsRequired();
                navigationBuilder.Property(x => x.SerialNumber).HasColumnName("DAF_SRL_NUM");
                navigationBuilder.Property(x => x.EmployeeStatus).HasColumnName("DAF_EMP_STATUS").IsRequired();
            });

            // Employee info (optional)
            builder.OwnsOne(x => x.EmployeeInfo, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.EmployeeNumber).HasColumnName("DAF_EMP_NUM").HasMaxLength(20);
                navigationBuilder.Property(x => x.EmployeeName).HasColumnName("DAF_EMP_NAM").HasMaxLength(100);
                navigationBuilder.Property(x => x.Department).HasColumnName("DAF_EMP_DEPT").HasMaxLength(100);
            });

            // Contractor info (optional)
            builder.OwnsOne(x => x.ContractorInfo, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.ContractorId).HasColumnName("DAF_CONT_ID");
                navigationBuilder.Property(x => x.ContractorName).HasColumnName("DAF_CONT_NAM").HasMaxLength(100);
            });

            // Injury details
            builder.OwnsOne(x => x.InjuryDetails, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.InjuryCategoryId).HasColumnName("DAF_CAT_INJ").IsRequired();
                navigationBuilder.Property(x => x.InjuryNatureId).HasColumnName("DAF_NAT_INJ").IsRequired();
                navigationBuilder.Property(x => x.BodyPart).HasColumnName("DAF_BODY_PART").HasMaxLength(100).IsRequired();
                navigationBuilder.Property(x => x.Description).HasColumnName("DAF_NATURE_INJ").HasMaxLength(100).IsRequired();
            });

            // Treatment info
            builder.OwnsOne(x => x.TreatmentInfo, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.MedicalCentreName).HasColumnName("DAF_MEDCENTRE_NAM").HasMaxLength(100).IsRequired();
                navigationBuilder.Property(x => x.MedicalCentreReceivedDate).HasColumnName("DAF_MEDCENTRE_DAT").IsRequired();
                navigationBuilder.Property(x => x.TreatmentGiven).HasColumnName("DAF_TRT_GIVEN").HasMaxLength(500).IsRequired();
                navigationBuilder.Property(x => x.Shift).HasColumnName("DAF_SHIFT").HasMaxLength(100);
                navigationBuilder.Property(x => x.ShiftInchargeMan).HasColumnName("DAF_SHFTINCHRG_NAM").HasMaxLength(100);
            });

            // Severity & Status foreign keys
            builder.Property(x => x.SeverityId).HasColumnName("DAF_SEVERITY_ID").IsRequired().HasDefaultValue(1);
            builder.Property(x => x.StatusId).HasColumnName("DAF_STATUS_ID").IsRequired().HasDefaultValue(1);

            // Reporting info
            builder.Property(x => x.EnteredUserId).HasColumnName("DAF_ENT_USR").HasMaxLength(100).IsRequired();
            builder.Property(x => x.EnteredUserNumber).HasColumnName("DAF_ENT_NUM").IsRequired();
            builder.Property(x => x.EnteredDate).HasColumnName("DAF_ENT_DATE").IsRequired();

            // Audit columns
            builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(100);
            builder.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(100);
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);

            // Indexes
            builder.HasIndex(x => x.Guid).IsUnique();
            builder.HasIndex(x => new { x.AccidentNumber }).IsUnique();
            builder.HasIndex(x => x.CompanyCode).HasName("IDX_DAILY_ACC_FIR_DAF_COM_COD");
            builder.HasIndex(x => x.EnteredDate).HasName("IDX_DAILY_ACC_FIR_DAF_ACC_DAT");

            // Foreign key constraints
            builder.HasOne<InjuryCategory>()
                .WithMany()
                .HasForeignKey("InjuryDetails.InjuryCategoryId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<InjuryNature>()
                .WithMany()
                .HasForeignKey("InjuryDetails.InjuryNatureId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AccidentSeverity>()
                .WithMany()
                .HasForeignKey(x => x.SeverityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<AccidentStatus>()
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ignore domain events collection
            builder.Ignore(x => x.DomainEvents);
        }
    }
}
