using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.ValueObjects;

namespace ClubMembershipService.Infrastructure.Data.Configurations;

public class ClubActivityConfiguration : IEntityTypeConfiguration<ClubActivity>
{
    public void Configure(EntityTypeBuilder<ClubActivity> builder)
    {
        builder.ToTable("CLUB_ACTIVITY");
        builder.HasKey(e => e.ActivityId);
        builder.Property(e => e.ActivityId).HasColumnName("ACTIVITY_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ClubId).HasColumnName("CLUB_ID").IsRequired();
        builder.Property(e => e.ActivityName).HasColumnName("ACTIVITY_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.ActivityDate).HasColumnName("ACTIVITY_DATE").IsRequired();
        builder.Property(e => e.ActivityBudget).HasColumnName("ACTIVITY_BUDGET").HasPrecision(19, 2);
        builder.Property(e => e.OrganizerId).HasColumnName("ORGANIZER_ID").IsRequired();
        builder.Property(e => e.Status)
            .HasColumnName("ACTIVITY_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(v => v.Value, v => ActivityStatus.From(v));
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON");
        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Id);
    }
}
