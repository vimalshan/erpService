using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence.Configurations;

public sealed class ActualOrderMapConfiguration : IEntityTypeConfiguration<ActualOrderMapEntity>
{
    public void Configure(EntityTypeBuilder<ActualOrderMapEntity> builder)
    {
        builder.ToTable("ACTUAL_ORDER_MAP");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("ACTUAL_ORDER_MAP_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TiedOrderDetailId).HasColumnName("TIED_ORDER_DETAIL_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.ActualLineId).HasColumnName("ACTUAL_LINE_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.MappingQuantity).HasColumnName("MAPPING_QUANTITY");
        builder.Property(e => e.ModifiedByUserId).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(e => e.ModifiedDate).HasColumnName("MODIFIED_DATE").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 1, TiedOrderDetailId = (decimal?)1001m, ActualLineId = (decimal?)5001m,
                MappingQuantity = (int?)500, ModifiedByUserId = (int?)1,
                ModifiedDate = new DateTime?(new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc)) },
            new { Id = 2, TiedOrderDetailId = (decimal?)1002m, ActualLineId = (decimal?)5002m,
                MappingQuantity = (int?)300, ModifiedByUserId = (int?)1,
                ModifiedDate = new DateTime?(new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc)) },
            new { Id = 3, TiedOrderDetailId = (decimal?)1003m, ActualLineId = (decimal?)5003m,
                MappingQuantity = (int?)750, ModifiedByUserId = (int?)2,
                ModifiedDate = new DateTime?(new DateTime(2026, 3, 19, 0, 0, 0, DateTimeKind.Utc)) }
        );
    }
}
