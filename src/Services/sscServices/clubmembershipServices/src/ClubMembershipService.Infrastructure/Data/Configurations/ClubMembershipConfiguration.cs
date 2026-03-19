using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.ValueObjects;

namespace ClubMembershipService.Infrastructure.Data.Configurations;

public class ClubMembershipConfiguration : IEntityTypeConfiguration<ClubMembership>
{
    public void Configure(EntityTypeBuilder<ClubMembership> builder)
    {
        builder.ToTable("CLUB_MEMBERSHIP");
        builder.HasKey(e => e.MembershipId);
        builder.Property(e => e.MembershipId).HasColumnName("MEMBERSHIP_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ClubId).HasColumnName("CLUB_ID").IsRequired();
        builder.Property(e => e.MemberId).HasColumnName("MEMBER_ID").IsRequired();
        builder.Property(e => e.JoinDate).HasColumnName("JOIN_DATE").IsRequired();
        builder.Property(e => e.MembershipFee).HasColumnName("MEMBERSHIP_FEE").HasPrecision(19, 2);
        builder.Property(e => e.Status)
            .HasColumnName("MEMBERSHIP_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(v => v.Value, v => MembershipStatus.From(v));
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON");
        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Id);
    }
}
