namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;

public sealed class RequestSubConfiguration : IEntityTypeConfiguration<RequestSub>
{
    public void Configure(EntityTypeBuilder<RequestSub> builder)
    {
        builder.ToTable("SP_REQUEST_SUB");
        builder.HasKey(r => r.RequestSubId);
        builder.Property(r => r.RequestSubId).HasColumnName("RS_REQUESTSUB_ID").ValueGeneratedNever();
        builder.Property(r => r.RequestId).HasColumnName("RS_REQUESTID");
        builder.Property(r => r.StationaryId).HasColumnName("RS_STATIONARYID");
        builder.Property(r => r.DeptId).HasColumnName("RS_DEPTID");
        builder.Property(r => r.ExpectedDate).HasColumnName("RS_EXPECTED_DATE");
        builder.Property(r => r.UserSysId).HasColumnName("RS_USER_SYSID");
        builder.Property(r => r.RequestedQty).HasColumnName("RS_REQUESTEDQTY");
        builder.Property(r => r.IndentedQty).HasColumnName("RS_INDENTEDQTY");
        builder.Property(r => r.ApprovedQty).HasColumnName("RS_APPROVEDQTY");
        builder.Property(r => r.ApproverSysId).HasColumnName("RS_APPROVER_SYSID");
        builder.Property(r => r.ApproverRemarks).HasColumnName("RS_APPROVER_RAMARKS").HasMaxLength(255);
        builder.Property(r => r.ReceivedDate).HasColumnName("RS_RECEIVED_DATE");
        builder.Property(r => r.Status)
            .HasColumnName("RS_STATUS")
            .HasMaxLength(1)
            .HasConversion(
                s => s.Value,
                v => new RequestStatus(v));
        builder.Property(r => r.UpdatedBy).HasColumnName("RS_UPDATED_BY");
        builder.Property(r => r.UpdatedOn).HasColumnName("RS_UPDATED_ON");
        builder.Property(r => r.ApprovedOn).HasColumnName("RS_APPROVED_ON");

        builder.Ignore(r => r.DomainEvents);
    }
}
