using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence.Configurations;

public class SignatoryConfiguration : IEntityTypeConfiguration<Signatory>
{
    public void Configure(EntityTypeBuilder<Signatory> builder)
    {
        builder.ToTable("DD_SIGNATORY");
        builder.HasKey(s => s.SignatoryNumber);
        builder.Property(s => s.SignatoryNumber).HasColumnName("DD_SIG_NUM").HasColumnType("decimal(38,0)");
        builder.Property(s => s.Name).HasColumnName("DD_SIG_NAM").HasMaxLength(200);
        builder.Property(s => s.Designation).HasColumnName("DD_SIG_DSG").HasMaxLength(200);
        builder.Property(s => s.LiveFlag).HasColumnName("DD_LIVE_FLG").HasMaxLength(1);
        builder.Property(s => s.EmployeeSysId).HasColumnName("DD_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(s => s.ImageFileName).HasColumnName("DD_SIG_IMG").HasMaxLength(50);
        builder.Property(s => s.DigitalSignPfxFileName).HasColumnName("DD_DIGITALSIGN_PFXFILENAME").HasMaxLength(200);
        builder.Property(s => s.DigitalSignPfxPassword).HasColumnName("DD_DIGITALSIGN_PFXPASSWORD").HasMaxLength(100);
        builder.Property(s => s.AlternateImageFileName).HasColumnName("DD_SIG_IMGALT").HasMaxLength(50);
        builder.Ignore(s => s.DomainEvents);
    }
}
