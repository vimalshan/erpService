using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class TravelSubConfiguration : IEntityTypeConfiguration<TravelSub>
{
    public void Configure(EntityTypeBuilder<TravelSub> builder)
    {
        builder.ToTable("TRAVEL_SUB");

        builder.HasKey(e => new { e.RequestNumber, e.SerialNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TR_REQ_NUM").HasColumnType("bigint");
        builder.Property(e => e.SerialNumber).HasColumnName("TR_SRL_NUM").HasColumnType("bigint");
        builder.Property(e => e.BookingNumber).HasColumnName("TR_BOK_NUM");
        builder.Property(e => e.ModifiedDate).HasColumnName("TR_MOD_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.CancelDate).HasColumnName("TR_CAN_DAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.CancelRemarks).HasColumnName("TR_CAN_REM").HasMaxLength(200);
        builder.Property(e => e.AdditionalField1).HasColumnName("TR_ADD_FL1");
        builder.Property(e => e.AdditionalField2).HasColumnName("TR_ADD_FL2").HasMaxLength(65);
        builder.Property(e => e.AdditionalField3).HasColumnName("TR_ADD_FL3").HasMaxLength(65);
        builder.Property(e => e.OnDuty).HasColumnName("TR_OND_FLG")
            .HasConversion(v => v ? "Y" : "N", v => v == "Y")
            .HasColumnType("char(1)");

        builder.Ignore(e => e.DomainEvents);
    }
}
