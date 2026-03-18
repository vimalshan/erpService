using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MedicalVisit.Domain.Entities;
using MedicalVisit.Domain.Enums;

namespace MedicalVisit.Infrastructure.Persistence.Configurations;

public class VisitMainConfiguration : IEntityTypeConfiguration<VisitMainAggregate>
{
    public void Configure(EntityTypeBuilder<VisitMainAggregate> builder)
    {
        builder.ToTable("VISIT_MAIN");

        builder.HasKey(v => new { v.CompanyCode, v.VisitNumber });

        builder.Property(v => v.CompanyCode)
            .HasColumnName("VM_COM_COD")
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(v => v.VisitNumber)
            .HasColumnName("VM_VIS_NUM")
            .IsRequired();

        builder.Property(v => v.MedicalUserId)
            .HasColumnName("VM_USR_ID")
            .HasMaxLength(25);

        builder.Property(v => v.MedicalPinNumber)
            .HasColumnName("VM_PIN_NUM")
            .HasColumnType("decimal(20,0)");

        builder.Property(v => v.WorkerName)
            .HasColumnName("VM_WRK_NAM")
            .HasMaxLength(50);

        builder.Property(v => v.ContractorId)
            .HasColumnName("VM_CONTRCT_ID")
            .HasMaxLength(20);

        builder.Property(v => v.ContractorName)
            .HasColumnName("VM_CONTRCT_NAM")
            .HasMaxLength(20);

        builder.Property(v => v.VisitDate)
            .HasColumnName("VM_VIS_DAT")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(v => v.OtherHospital)
            .HasColumnName("VM_OTH_HOSP")
            .HasMaxLength(200);

        builder.Property(v => v.Shift)
            .HasColumnName("VM_VIS_SHIFT")
            .HasMaxLength(1)
            .HasConversion(
                s => s.HasValue ? (char)s.Value : (char?)null,
                c => c.HasValue ? (VisitShift)c.Value : null);

        builder.Property(v => v.Type)
            .HasColumnName("VM_VIS_TYP")
            .HasMaxLength(1)
            .HasConversion(
                t => t.HasValue ? (char)t.Value : (char?)null,
                c => c.HasValue ? (VisitType)c.Value : null);

        builder.Property(v => v.AttendantCode)
            .HasColumnName("VM_ATT_COD")
            .HasMaxLength(10);

        builder.Property(v => v.DoctorCode)
            .HasColumnName("VM_DOC_COD")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(v => v.MedicineGiven)
            .HasColumnName("VM_MED_GIV")
            .HasMaxLength(3);

        builder.Property(v => v.NextReviewDate)
            .HasColumnName("VM_NXT_REV")
            .HasColumnType("datetime2(3)");

        builder.Property(v => v.IsCancelled)
            .HasColumnName("VM_CAN_FLG")
            .HasMaxLength(1)
            .HasConversion(
                b => b ? "Y" : "N",
                s => s == "Y");

        // Owned type for DiagnosisInfo
        builder.OwnsOne(v => v.Diagnosis, diagnosis =>
        {
            diagnosis.Property(d => d.PatientDiagnosis)
                .HasColumnName("VM_PAT_DIA")
                .HasMaxLength(200)
                .IsRequired();

            diagnosis.Property(d => d.TreatmentRemarks)
                .HasColumnName("VM_TRT_REM")
                .HasMaxLength(200)
                .IsRequired();

            diagnosis.Property(d => d.TestAdvice)
                .HasColumnName("VM_TST_ADV")
                .HasMaxLength(200);

            diagnosis.Property(d => d.DoctorRemarks)
                .HasColumnName("VM_DOC_REMARKS")
                .HasMaxLength(1000);

            diagnosis.Property(d => d.DiagnosisCategory)
                .HasColumnName("VM_DIA_CAT")
                .HasMaxLength(3);

            diagnosis.Property(d => d.DiagnosisSubCategory)
                .HasColumnName("VM_DIA_SUBCAT");
        });

        // Owned type for CreatedInfo
        builder.OwnsOne(v => v.CreatedInfo, audit =>
        {
            audit.Property(a => a.UserId)
                .HasColumnName("VM_ENT_USR")
                .HasMaxLength(25);

            audit.Property(a => a.UserPin)
                .HasColumnName("VM_ENT_NUM")
                .HasColumnType("decimal(20,0)");

            audit.Property(a => a.Timestamp)
                .HasColumnName("VM_ENT_DAT")
                .HasColumnType("datetime2(3)");
        });

        // Owned type for ModifiedInfo
        builder.OwnsOne(v => v.ModifiedInfo, audit =>
        {
            audit.Property(a => a.UserId)
                .HasColumnName("DV_MOD_USR")
                .HasMaxLength(25);

            audit.Property(a => a.UserPin)
                .HasColumnName("VM_MOD_NUM")
                .HasColumnType("decimal(20,0)");

            audit.Property(a => a.Timestamp)
                .HasColumnName("VM_MOD_DAT")
                .HasColumnType("datetime2(3)");
        });

        // Navigation to SubRecords
        builder.HasMany(v => v.SubRecords)
            .WithOne()
            .HasForeignKey(sr => new { sr.CompanyCode, sr.VisitNumber })
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(v => v.CompanyCode).HasDatabaseName("IDX_VISIT_MAIN_VM_COM_COD");
        builder.HasIndex(v => v.VisitDate).HasDatabaseName("IDX_VISIT_MAIN_VM_VIS_DAT");

        // Ignore DomainEvents
        builder.Ignore(v => v.DomainEvents);
    }
}
