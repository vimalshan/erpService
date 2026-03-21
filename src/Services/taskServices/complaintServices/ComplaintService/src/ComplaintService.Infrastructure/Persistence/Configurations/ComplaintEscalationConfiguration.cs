using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintService.Infrastructure.Persistence.Configurations;

public class ComplaintEscalationConfiguration : IEntityTypeConfiguration<ComplaintEscalation>
{
    public void Configure(EntityTypeBuilder<ComplaintEscalation> builder)
    {
        builder.ToTable("COMPL_ESC");
        builder.HasKey(x => new { x.TicketNum, x.LevelNum }); // Natural composite PK

        builder.Property(x => x.TicketNum).HasColumnName("CE_TICKET_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LevelNum).HasColumnName("CE_LEVEL_NUM").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.EscNoHrs).HasColumnName("CE_ESC_NOHRS").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.UserPin).HasColumnName("CE_USER_PIN").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.EffDate).HasColumnName("CE_EFF_DATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.ClsDate).HasColumnName("CE_CLS_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.Exclude).HasColumnName("CE_EXCLUDE").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.UpdatedBy).HasColumnName("CE_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.UpdatedOn).HasColumnName("CE_UPDATEDON").HasColumnType("datetime2(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}
