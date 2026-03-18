using ErrorLoggingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorLoggingService.Infrastructure.Persistence.Configurations;

public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.ToTable("ERRSP");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ErrorMessage)
            .HasColumnName("ERR_MESS")
            .HasMaxLength(4000)
            .IsRequired(false);

        builder.Property(x => x.StoredProcedureName)
            .HasColumnName("ERR_SP")
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.ErrorReference)
            .HasColumnName("ERR_REF")
            .IsRequired(false);

        builder.Property(x => x.ErrorDate)
            .HasColumnName("ERR_DATE")
            .HasColumnType("datetime2(3)")
            .IsRequired(false);

        builder.Ignore(x => x.DomainEvents);
    }
}
