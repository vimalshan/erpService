using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Infrastructure.Configurations;

public class ScholarshipDetailConfiguration : IEntityTypeConfiguration<ScholarshipDetail>
{
    public void Configure(EntityTypeBuilder<ScholarshipDetail> builder)
    {
        builder.ToTable("SCHOLARSHIP_DETAIL");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("SCHDET_ID").ValueGeneratedNever();
        builder.Property(x => x.MainId).HasColumnName("SCHDET_MAINID").HasColumnType("int").IsRequired();
        builder.Property(x => x.Year).HasColumnName("SCHDET_YEAR").IsRequired();
        builder.Property(x => x.MarksFile).HasColumnName("SCHDET_MARKSFILE").HasMaxLength(100).IsRequired();
        builder.Property(x => x.MarksStatus).HasColumnName("SCHDET_MARKSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.PayStatus).HasColumnName("SCHDET_PAYSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("SCHDET_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("SCHDET_CREATEDBY").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("SCHDET_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("SCHDET_UPDATEDBY");
        builder.Property(x => x.ApprovedOn).HasColumnName("SCHDET_APPROVEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.ApprovedBy).HasColumnName("SCHDET_APPROVEDBY");
        builder.Property(x => x.PayApprovedOn).HasColumnName("SCHDET_PAYAPPROVEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.PayApprovedBy).HasColumnName("SCHDET_PAYAPPROVEDBY");
        builder.Property(x => x.PayDate).HasColumnName("SCHDET_PAYDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.PayAmount).HasColumnName("SCHDET_PAYAMOUNT");
        builder.Property(x => x.PayUpdatedOn).HasColumnName("SCHDET_PAYUPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.PayUpdatedBy).HasColumnName("SCHDET_PAYUPDATEDBY");

        builder.Ignore(x => x.DomainEvents);
    }
}
