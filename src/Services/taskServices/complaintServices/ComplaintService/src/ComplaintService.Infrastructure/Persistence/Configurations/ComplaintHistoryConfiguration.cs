using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintService.Infrastructure.Persistence.Configurations;

public class ComplaintHistoryConfiguration : IEntityTypeConfiguration<ComplaintHistory>
{
    public void Configure(EntityTypeBuilder<ComplaintHistory> builder)
    {
        builder.ToTable("COMPL_HIST");
        builder.HasKey(x => x.HistoryNum);

        builder.Property(x => x.HistoryNum).HasColumnName("CH_HISTORY_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ActionNum).HasColumnName("CH_ACTION_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.SerialNum).HasColumnName("CH_SERIAL_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.From).HasColumnName("CH_FROM").HasMaxLength(65);
        builder.Property(x => x.To).HasColumnName("CH_TO").HasMaxLength(1000);
        builder.Property(x => x.ActionDate).HasColumnName("CH_ACTION_DATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.ActionType).HasColumnName("CH_ACTION_TYPE").HasMaxLength(1).IsFixedLength().IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("CH_REMARKS").HasMaxLength(4000);
        builder.Property(x => x.UpdatedBy).HasColumnName("CH_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.UpdatedOn).HasColumnName("CH_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.FilePath).HasColumnName("CH_FILEPATH").HasMaxLength(200);

        builder.Ignore(x => x.DomainEvents);
    }
}
