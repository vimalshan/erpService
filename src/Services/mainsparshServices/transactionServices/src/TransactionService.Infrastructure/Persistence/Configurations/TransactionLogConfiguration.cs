using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public class TransactionLogConfiguration : IEntityTypeConfiguration<TransactionLog>
{
    public void Configure(EntityTypeBuilder<TransactionLog> builder)
    {
        builder.ToTable("TRANSACTION_LOG");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("LOG_ID")
            .UseIdentityColumn(1, 1);

        builder.Property(x => x.TransactionType).HasColumnName("TRANSACTION_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(x => x.TransactionId).HasColumnName("TRANSACTION_ID").IsRequired();
        builder.Property(x => x.Action).HasColumnName("ACTION").HasMaxLength(50).IsRequired();
        builder.Property(x => x.ActionBy).HasColumnName("ACTION_BY").IsRequired();
        builder.Property(x => x.ActionData).HasColumnName("ACTION_DATA").HasColumnType("nvarchar(max)");
        builder.Property(x => x.PreviousStatus).HasColumnName("PREVIOUS_STATUS").HasMaxLength(50);
        builder.Property(x => x.NewStatus).HasColumnName("NEW_STATUS").HasMaxLength(50);
        builder.Property(x => x.IpAddress).HasColumnName("IP_ADDRESS").HasMaxLength(50);
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();

        builder.HasIndex(x => x.TransactionType).HasDatabaseName("IX_TRANSACTION_LOG_TYPE");
        builder.HasIndex(x => new { x.TransactionType, x.TransactionId }).HasDatabaseName("IX_TRANSACTION_LOG_ENTITY");
        builder.HasIndex(x => x.Action).HasDatabaseName("IX_TRANSACTION_LOG_ACTION");
        builder.HasIndex(x => x.ActionBy).HasDatabaseName("IX_TRANSACTION_LOG_ACTION_BY");
        builder.HasIndex(x => x.CreatedOn).HasDatabaseName("IX_TRANSACTION_LOG_CREATED_ON");
    }
}
