using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class LeaveApprovalConfiguration : IEntityTypeConfiguration<LeaveApproval>
{
    public void Configure(EntityTypeBuilder<LeaveApproval> builder)
    {
        builder.ToTable("LEAVE_DETAILSAPR");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("LDA_SYSID").ValueGeneratedNever();

        builder.Property(a => a.LeaveDetailId)
            .HasColumnName("LDA_LVDSYSID")
            .IsRequired();

        builder.Property(a => a.ApprovedBy)
            .HasColumnName("LDA_APPROVEDBY")
            .IsRequired();

        builder.Property(a => a.ApprovedOn)
            .HasColumnName("LDA_APPROVEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(a => a.DomainEvents);

        builder.HasIndex(a => a.LeaveDetailId).HasDatabaseName("IX_LEAVE_DETAILSAPR_LVDSYSID");
    }
}
