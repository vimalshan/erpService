using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.ValueObjects;

namespace ClubMembershipService.Infrastructure.Data.Configurations;

public class ClubMasterConfiguration : IEntityTypeConfiguration<ClubMaster>
{
    public void Configure(EntityTypeBuilder<ClubMaster> builder)
    {
        builder.ToTable("CLUB_MASTER");
        builder.HasKey(e => e.ClubId);
        builder.Property(e => e.ClubId).HasColumnName("CLUB_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ClubName).HasColumnName("CLUB_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Status)
            .HasColumnName("CLUB_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(v => v.Value, v => ClubStatus.From(v));
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON");

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Id);

        builder.HasMany(e => e.Memberships)
            .WithOne(m => m.Club)
            .HasForeignKey(m => m.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Activities)
            .WithOne(a => a.Club)
            .HasForeignKey(a => a.ClubId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
