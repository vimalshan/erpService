using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisitorServices.Domain.Entities;
using VisitorServices.Domain.Enums;

namespace VisitorServices.Infrastructure.Data.Configurations;

public class VisitorApprovalRequestConfiguration : IEntityTypeConfiguration<VisitorApprovalRequest>
{
    public void Configure(EntityTypeBuilder<VisitorApprovalRequest> builder)
    {
        builder.ToTable("VISITOR_APPREQUEST");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("VREQ_ID").ValueGeneratedNever();

        builder.Property(r => r.VisitorId).HasColumnName("VREQ_VISITORID").IsRequired();
        builder.Property(r => r.RequiredApproverId).HasColumnName("VREQ_REQUIREDAPPROVERID").IsRequired();

        builder.Property(r => r.ApprovalStatus)
            .HasColumnName("VREQ_APPROVALSTATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => ((char)(int)v).ToString(),
                s => (ApprovalStatus)s[0]);

        builder.Property(r => r.ApprovalDate)
            .HasColumnName("VREQ_APPROVALDATE")
            .HasColumnType("datetime2(3)");

        builder.Property(r => r.ApprovalRemarks)
            .HasColumnName("VREQ_APPROVALREMARKS")
            .HasMaxLength(500);

        builder.Property(r => r.RequestedOn)
            .HasColumnName("VREQ_REQUESTEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(r => r.RequestedBy).HasColumnName("VREQ_REQUESTEDBY").IsRequired();
        builder.Property(r => r.LastModifiedBy).HasColumnName("VREQ_LASTMODIFIEDBY").IsRequired();

        builder.Property(r => r.LastModifiedOn)
            .HasColumnName("VREQ_LASTMODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(r => r.DomainEvents);

        builder.HasIndex(r => r.VisitorId).HasDatabaseName("IX_VISITOR_APPREQUEST_VISITORID");
        builder.HasIndex(r => r.ApprovalStatus).HasDatabaseName("IX_VISITOR_APPREQUEST_STATUS");
    }
}
