using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MedicalVisit.Domain.Entities;

namespace MedicalVisit.Infrastructure.Persistence.Configurations;

public class VisitSubRecordConfiguration : IEntityTypeConfiguration<VisitSubRecord>
{
    public void Configure(EntityTypeBuilder<VisitSubRecord> builder)
    {
        builder.ToTable("VISIT_SUB");

        builder.HasKey(v => new { v.CompanyCode, v.VisitNumber, v.SerialNumber });

        builder.Property(v => v.CompanyCode)
            .HasColumnName("VS_COM_COD")
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(v => v.VisitNumber)
            .HasColumnName("VS_VIS_NUM")
            .IsRequired();

        builder.Property(v => v.TestType)
            .HasColumnName("VS_TST_TYP")
            .HasMaxLength(20);

        builder.Property(v => v.TestValue)
            .HasColumnName("VS_TST_VAL")
            .HasMaxLength(25);

        builder.Property(v => v.SerialNumber)
            .HasColumnName("VS_SRL_NUM");

        // Index
        builder.HasIndex(v => v.VisitNumber).HasDatabaseName("IDX_VISIT_SUB_VS_VIS_NUM");

        // Ignore DomainEvents
        builder.Ignore(v => v.DomainEvents);
    }
}
