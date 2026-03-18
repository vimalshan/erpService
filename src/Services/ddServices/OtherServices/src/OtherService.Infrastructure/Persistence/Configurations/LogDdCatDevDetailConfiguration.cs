using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OtherService.Domain.Entities;

namespace OtherService.Infrastructure.Persistence.Configurations;

public sealed class LogDdCatDevDetailConfiguration
    : IEntityTypeConfiguration<LogDdCatDevDetail>
{
    public void Configure(EntityTypeBuilder<LogDdCatDevDetail> builder)
    {
        builder.ToTable("LOG_DD_CAT_DEV_DETAIL");

        // Composite primary key
        builder.HasKey(e => new { e.AppId, e.AppNum });

        builder.Property(e => e.AppId)
            .HasColumnName("CT_APP_ID")
            .HasColumnType("VARCHAR(30)")
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(e => e.AppNum)
            .HasColumnName("CT_APP_NUM")
            .HasColumnType("DECIMAL(38,0)")
            .IsRequired();

        builder.Property(e => e.ReqNum)
            .HasColumnName("CT_REQ_NUM")
            .HasColumnType("DECIMAL(38,0)")
            .IsRequired(false);

        builder.Property(e => e.QtnNum)
            .HasColumnName("CT_QTN_NUM")
            .HasColumnType("DECIMAL(38,0)")
            .IsRequired(false);

        builder.Property(e => e.AnsSrl)
            .HasColumnName("CT_ANS_SRL")
            .HasColumnType("DECIMAL(38,0)")
            .IsRequired(false);

        builder.Property(e => e.EntDat)
            .HasColumnName("CT_ENT_DAT")
            .HasColumnType("DATETIME2(3)")
            .IsRequired(false);

        builder.Property(e => e.Desc)
            .HasColumnName("CT_DESC")
            .HasColumnType("VARCHAR(400)")
            .IsRequired(false)
            .HasMaxLength(400);

        builder.Property(e => e.Need)
            .HasColumnName("CT_NEED")
            .HasColumnType("VARCHAR(400)")
            .IsRequired(false)
            .HasMaxLength(400);

        // Ignore domain events – not persisted
        builder.Ignore("DomainEvents");
    }
}
