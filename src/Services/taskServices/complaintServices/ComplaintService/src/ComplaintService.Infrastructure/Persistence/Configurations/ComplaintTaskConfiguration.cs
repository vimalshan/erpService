using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintService.Infrastructure.Persistence.Configurations;

public class ComplaintTaskConfiguration : IEntityTypeConfiguration<ComplaintTask>
{
    public void Configure(EntityTypeBuilder<ComplaintTask> builder)
    {
        builder.ToTable("COMPL_TASK");
        builder.HasKey(x => x.TaskNum);

        builder.Property(x => x.TaskNum).HasColumnName("CT_TASK_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.TicketNum).HasColumnName("CT_TICKET_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ScheduleFreq).HasColumnName("CT_SCHEDULE_FREQ").HasMaxLength(2).IsFixedLength().IsRequired();
        builder.Property(x => x.ScheduleValue).HasColumnName("CT_SCHEDULE_VALUE").HasMaxLength(300);
        builder.Property(x => x.ScheduleTime).HasColumnName("CT_SCHEDULE_TIME").HasMaxLength(12);
        builder.Property(x => x.ScheduleDay).HasColumnName("CT_SCHEDULE_DAY").HasMaxLength(65);
        builder.Property(x => x.EffDate).HasColumnName("CT_EFF_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.ClsDate).HasColumnName("CT_CLS_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("CT_UPDATED_BY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.UpdatedOn).HasColumnName("CT_UPDATED_ON").HasColumnType("datetime2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}
