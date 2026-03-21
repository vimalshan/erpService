using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintService.Infrastructure.Persistence.Configurations;

public class ComplaintActionConfiguration : IEntityTypeConfiguration<ComplaintAction>
{
    public void Configure(EntityTypeBuilder<ComplaintAction> builder)
    {
        builder.ToTable("COMPL_ACTION");
        builder.HasKey(x => x.ActionNum);

        builder.Property(x => x.ActionNum).HasColumnName("CA_ACTION_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.TaskNum).HasColumnName("CA_TASK_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.PrimaryResp).HasColumnName("CA_PRM_RESP").HasMaxLength(300);
        builder.Property(x => x.PrimaryActBy).HasColumnName("CA_PRM_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.PrimaryActDate).HasColumnName("CA_PRM_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.PrimarySolution).HasColumnName("CA_PRM_SOLUTION").HasMaxLength(4000);
        builder.Property(x => x.SecEscHrs).HasColumnName("CA_SEC_ESCHRS").HasColumnType("decimal(38,0)");
        builder.Property(x => x.SecResp).HasColumnName("CA_SEC_RESP").HasMaxLength(300);
        builder.Property(x => x.SecActBy).HasColumnName("CA_SEC_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.SecActDate).HasColumnName("CA_SEC_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.SecSolution).HasColumnName("CA_SEC_SOLUTION").HasMaxLength(4000);
        builder.Property(x => x.FwdRemarks).HasColumnName("CA_FWD_REMARKS").HasMaxLength(4000);
        builder.Property(x => x.FwdResp).HasColumnName("CA_FWD_RESP").HasMaxLength(300);
        builder.Property(x => x.FwdActBy).HasColumnName("CA_FWD_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.FwdActDate).HasColumnName("CA_FWD_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.FwdSolution).HasColumnName("CA_FWD_SOLUTION").HasMaxLength(4000);
        builder.Property(x => x.CurrentEscLevel).HasColumnName("CA_CUR_ESCLEVEL").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CorrActRequired).HasColumnName("CA_CORR_ACTREQ").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.CorrRemarks).HasColumnName("CA_CORR_REMARKS").HasMaxLength(4000);
        builder.Property(x => x.CorrResp).HasColumnName("CA_CORR_RESP").HasMaxLength(300);
        builder.Property(x => x.CorrActBy).HasColumnName("CA_CORR_ACTBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.CorrActDate).HasColumnName("CA_CORR_ACTDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.CorrSolution).HasColumnName("CA_CORR_SOLUTION").HasMaxLength(4000);
        builder.Property(x => x.ReopenFlag).HasColumnName("CA_REOPEN_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.ReopenRemarks).HasColumnName("CA_REOPEN_REMARKS").HasMaxLength(4000);
        builder.Property(x => x.TargetDate).HasColumnName("CA_TRG_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.ClosureDate).HasColumnName("CA_CLS_DAT").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("CA_UPATEDBY").HasColumnType("decimal(38,0)");

        builder.Ignore(x => x.DomainEvents);
        builder.HasMany(x => x.Histories).WithOne().HasForeignKey(h => h.ActionNum);
    }
}
