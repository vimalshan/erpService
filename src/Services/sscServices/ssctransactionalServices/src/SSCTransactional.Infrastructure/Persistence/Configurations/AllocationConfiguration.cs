using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SSCTransactional.Domain.Aggregates;

namespace SSCTransactional.Infrastructure.Persistence.Configurations;

public class AllocationConfiguration : IEntityTypeConfiguration<AllocationAggregate>
{
    public void Configure(EntityTypeBuilder<AllocationAggregate> builder)
    {
        builder.ToTable("DOC_APALLDET");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("APALL_ID").ValueGeneratedNever();
        builder.Property(x => x.DocId).HasColumnName("APALL_DOCID").IsRequired();
        builder.Property(x => x.Action).HasColumnName("APALL_ACTION").HasMaxLength(1).IsRequired();
        builder.Property(x => x.GroupId).HasColumnName("APALL_GROUPID").IsRequired();
        builder.Property(x => x.PullStatus).HasColumnName("APALL_PULLSTATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.PullUserId).HasColumnName("APALL_PULLUSERID").IsRequired();
        builder.Property(x => x.Priority).HasColumnName("APALL_PRIORITY").IsRequired();
        builder.Property(x => x.AllocatedBy).HasColumnName("APALL_ALLBY").IsRequired();
        builder.Property(x => x.AllocatedOn).HasColumnName("APALL_ALLON").IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("APALL_REMARKS").HasMaxLength(200);
        builder.Property(x => x.ActionFlag).HasColumnName("APALL_ACTIONFLAG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ActionDate).HasColumnName("APALL_ACTIONDATE");
        builder.Property(x => x.CorrespondenceId).HasColumnName("APALL_CORRID");
        builder.Property(x => x.DefectType).HasColumnName("APALL_DEFTYPE");
        builder.Property(x => x.CloseRemarks).HasColumnName("APALL_CLOSEREMARKS").HasMaxLength(200);
        builder.Property(x => x.ModifiedBy).HasColumnName("APALL_MODIFIEDBY").IsRequired();
        builder.Property(x => x.ModifiedOn).HasColumnName("APALL_MODIFIEDON").IsRequired();
        builder.Property(x => x.PulledOn).HasColumnName("APALL_PULLEDON").IsRequired();

        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.DefectiveAttachments)
            .WithOne()
            .HasForeignKey(d => d.AllocationId);
    }
}
