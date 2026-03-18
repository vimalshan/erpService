using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class FlWorkingShiftConfiguration : IEntityTypeConfiguration<FlWorkingShift>
{
    public void Configure(EntityTypeBuilder<FlWorkingShift> builder)
    {
        builder.ToTable("FL_WORKING_SHIFT");
        builder.HasNoKey();
        builder.Property(x => x.FlWorkingId).HasColumnName("FL_WORKING_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.FillingLineId).HasColumnName("FILLINGLINE_ID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ShiftCode).HasColumnName("SHIFT_CODE").HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.StartDate).HasColumnName("START_DATE").IsRequired();
        builder.Property(x => x.CloseDate).HasColumnName("CLOSE_DATE");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);
    }
}
