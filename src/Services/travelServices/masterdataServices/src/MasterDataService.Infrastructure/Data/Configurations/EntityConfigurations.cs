using MasterDataService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterDataService.Infrastructure.Data.Configurations;

public class GuestHouseConfiguration : IEntityTypeConfiguration<GuestHouse>
{
    public void Configure(EntityTypeBuilder<GuestHouse> builder)
    {
        builder.ToTable("TRAVEL_GUESTHOUSE");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AdminCode).HasColumnName("AD_ADM_COD").IsRequired();
        builder.Property(e => e.GuestHouseName).HasColumnName("AD_ADM_NAM").HasMaxLength(50);
        builder.Property(e => e.Type).HasColumnName("AD_ADM_TYP").HasMaxLength(1);
        builder.Property(e => e.DailyAmount).HasColumnName("AD_ADM_AMOUNT");
        builder.HasMany(e => e.Rooms).WithOne(r => r.GuestHouse).HasForeignKey(r => r.GuestHouseCode).HasPrincipalKey(e => e.AdminCode);
        builder.HasIndex(e => e.AdminCode).IsUnique();
    }
}

public class GuestHouseRoomConfiguration : IEntityTypeConfiguration<GuestHouseRoom>
{
    public void Configure(EntityTypeBuilder<GuestHouseRoom> builder)
    {
        builder.ToTable("GH_ROOMS");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.GuestHouseCode).HasColumnName("GH_GHS_COD");
        builder.Property(e => e.RoomSerial).HasColumnName("GH_ROM_SRL");
        builder.Property(e => e.NumberOfPersons).HasColumnName("GH_NOF_PER");
        builder.Property(e => e.RoomNumber).HasColumnName("GH_ROM_NUM");
        builder.Property(e => e.Floor).HasColumnName("GH_GHS_FLR");
    }
}

public class GuestRoomAvailabilityConfiguration : IEntityTypeConfiguration<GuestRoomAvailability>
{
    public void Configure(EntityTypeBuilder<GuestRoomAvailability> builder)
    {
        builder.ToTable("GUEST_ROOM_AVAIL_TEMP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.FloorNumber).HasColumnName("GHS_FLR_NUM");
        builder.Property(e => e.RoomNumber).HasColumnName("GHS_ROM_NUM");
        builder.Property(e => e.RoomStatus).HasColumnName("GHS_ROM_STS").HasMaxLength(1);
        builder.Property(e => e.FloorValue).HasColumnName("GHS_FLR_VAL").HasMaxLength(200);
    }
}

public class GlCodeCombinationConfiguration : IEntityTypeConfiguration<GlCodeCombination>
{
    public void Configure(EntityTypeBuilder<GlCodeCombination> builder)
    {
        builder.ToTable("GL_CODE_COMBINATIONS_KFV");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.RowId).HasColumnName("ROW_ID");
        builder.Property(e => e.CodeCombinationId).HasColumnName("CODE_COMBINATION_ID");
        builder.Property(e => e.ChartOfAccountsId).HasColumnName("CHART_OF_ACCOUNTS_ID");
        builder.OwnsOne(e => e.Segments, seg =>
        {
            seg.Property(s => s.ConcatenatedSegments).HasColumnName("CONCATENATED_SEGMENTS").HasMaxLength(207);
            seg.Property(s => s.AccountType).HasColumnName("GL_ACCOUNT_TYPE").HasMaxLength(1);
        });
        builder.Property(e => e.EnabledFlag).HasColumnName("ENABLED_FLAG");
        builder.Property(e => e.SummaryFlag).HasColumnName("SUMMARY_FLAG");
        builder.Property(e => e.Context).HasColumnName("CONTEXT").HasMaxLength(150);
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE");
    }
}

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("COUPON_TEMP");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CouponId).HasColumnName("CPN_ID");
        builder.Property(e => e.Airline).HasColumnName("AIR_LIN").HasMaxLength(50);
        builder.Property(e => e.TotalCoupons).HasColumnName("TOT_CPN");
        builder.Property(e => e.UsedCoupons).HasColumnName("USD_CPN");
        builder.Property(e => e.BalanceCoupons).HasColumnName("BAL_CPN");
        builder.Property(e => e.ValidTill).HasColumnName("VLS_TIL");
    }
}

public class TaxSlabConfiguration : IEntityTypeConfiguration<TaxSlab>
{
    public void Configure(EntityTypeBuilder<TaxSlab> builder)
    {
        builder.ToTable("TAX_SLABS");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.TaxType).HasColumnName("TAX_TYPE").HasMaxLength(5);
        builder.Property(e => e.EffectiveDate).HasColumnName("TAX_EFFDAT");
        builder.Property(e => e.CloseDate).HasColumnName("TAX_CLSDAT");
        builder.Property(e => e.TaxRate).HasColumnName("TAX_RATE").HasColumnType("decimal(19,4)");
        builder.Property(e => e.VendorCode).HasColumnName("VENDORCODE");
    }
}

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("AREA_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AreaId).HasColumnName("AREA_ID");
        builder.Property(e => e.AreaName).HasColumnName("AREA_NAME").HasMaxLength(200);
        builder.HasIndex(e => e.AreaId).IsUnique();
    }
}

public class RouteConfiguration : IEntityTypeConfiguration<Domain.Entities.Route>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Route> builder)
    {
        builder.ToTable("ROUTE_MASTER");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.RouteId).HasColumnName("ROUTE_ID");
        builder.Property(e => e.RouteName).HasColumnName("ROUTE_NAME").HasMaxLength(200);
        builder.HasIndex(e => e.RouteId).IsUnique();
    }
}
