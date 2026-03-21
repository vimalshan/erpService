using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComplaintService.Infrastructure.Persistence.Configurations;

public class ComplaintTicketConfiguration : IEntityTypeConfiguration<ComplaintTicket>
{
    public void Configure(EntityTypeBuilder<ComplaintTicket> builder)
    {
        builder.ToTable("COMPL_DET");
        builder.HasKey(x => x.TicketNum);

        builder.Property(x => x.TicketNum).HasColumnName("CD_TICKET_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.GroupId).HasColumnName("CD_GROUPID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.Type).HasColumnName("CD_TYPE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.Location).HasColumnName("CD_LOCATION").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.Department).HasColumnName("CD_DEPARTMENT").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.Process).HasColumnName("CD_PROCESS").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.Subject).HasColumnName("CD_SUBJECT").HasMaxLength(500);
        builder.Property(x => x.Description).HasColumnName("CD_DESCRIPTION").HasMaxLength(4000);
        builder.Property(x => x.IsNCR).HasColumnName("CD_NCR").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.PicturePath).HasColumnName("CD_PICTUREPATH").HasMaxLength(200);
        builder.Property(x => x.FilePath).HasColumnName("CD_FILEPATH").HasMaxLength(200);
        builder.Property(x => x.TargetDate).HasColumnName("CD_TARGET_DATE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ClosureDate).HasColumnName("CD_CLOSURE_DATE").HasColumnType("datetime2(3)");

        builder.Ignore(x => x.DomainEvents);

        builder.HasOne(x => x.Action).WithOne(a => a.Ticket)
            .HasForeignKey<ComplaintAction>(a => a.TaskNum)
            .HasPrincipalKey<ComplaintTicket>(t => t.TicketNum);

        builder.HasMany(x => x.Escalations).WithOne()
            .HasForeignKey(e => e.TicketNum);

        builder.HasMany(x => x.Tasks).WithOne()
            .HasForeignKey(t => t.TicketNum);
    }
}
