using MeetingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingModule.Infrastructure.Persistence.Configurations;

public class MeetingTypeConfiguration : IEntityTypeConfiguration<MeetingType>
{
    public void Configure(EntityTypeBuilder<MeetingType> builder)
    {
        builder.ToTable("MEETTYPE_MAST");
        builder.HasKey(e => e.MeetTypeId);
        builder.Property(e => e.MeetTypeId).HasColumnName("MEETTYPE_ID");
        builder.Property(e => e.MeetTypeCode).HasColumnName("MEETTYPE_CODE").HasMaxLength(50).IsRequired();
        builder.Property(e => e.MeetTypeName).HasColumnName("MEETTYPE_NAME").HasMaxLength(255).IsRequired();
        builder.Property(e => e.MeetTypeDesc).HasColumnName("MEETTYPE_DESC");
        builder.Property(e => e.MeetTypeStatus).HasColumnName("MEETTYPE_STATUS").HasMaxLength(1).HasDefaultValue("A");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON").HasPrecision(3);

        builder.HasIndex(e => e.MeetTypeCode).IsUnique();
        builder.HasIndex(e => e.MeetTypeStatus);
    }
}

public class MeetingScheduleConfiguration : IEntityTypeConfiguration<MeetingSchedule>
{
    public void Configure(EntityTypeBuilder<MeetingSchedule> builder)
    {
        builder.ToTable("SRF_MEETINGSCH");
        builder.HasKey(e => e.MeetingId);
        builder.Property(e => e.MeetingId).HasColumnName("MEETING_ID");
        builder.Property(e => e.MeetTypeId).HasColumnName("MEETTYPE_ID");
        builder.Property(e => e.MeetingTitle).HasColumnName("MEETING_TITLE").HasMaxLength(255).IsRequired();
        builder.Property(e => e.MeetingDate).HasColumnName("MEETING_DATE").HasPrecision(3);
        builder.Property(e => e.MeetingLocation).HasColumnName("MEETING_LOCATION").HasMaxLength(255);
        builder.Property(e => e.MeetingDuration).HasColumnName("MEETING_DURATION");
        builder.Property(e => e.OrganizerId).HasColumnName("ORGANIZER_ID");
        builder.Property(e => e.MeetingStatus).HasColumnName("MEETING_STATUS").HasMaxLength(20).HasDefaultValue("SCHEDULED");
        builder.Property(e => e.Notes).HasColumnName("NOTES");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON").HasPrecision(3);

        builder.HasOne(e => e.MeetingType)
            .WithMany(t => t.MeetingSchedules)
            .HasForeignKey(e => e.MeetTypeId)
            .HasConstraintName("FK_SRF_MEETINGSCH_MEETTYPE");

        builder.HasIndex(e => e.MeetTypeId);
        builder.HasIndex(e => e.MeetingDate);
        builder.HasIndex(e => e.MeetingStatus);
    }
}

public class PollDetailConfiguration : IEntityTypeConfiguration<PollDetail>
{
    public void Configure(EntityTypeBuilder<PollDetail> builder)
    {
        builder.ToTable("SRF_POLL_DETAIL");
        builder.HasKey(e => e.PollId);
        builder.Property(e => e.PollId).HasColumnName("POLL_ID");
        builder.Property(e => e.MeetingId).HasColumnName("MEETING_ID");
        builder.Property(e => e.PollQuestion).HasColumnName("POLL_QUESTION").HasMaxLength(500).IsRequired();
        builder.Property(e => e.PollType).HasColumnName("POLL_TYPE").HasMaxLength(20);
        builder.Property(e => e.PollStatus).HasColumnName("POLL_STATUS").HasMaxLength(20).HasDefaultValue("ACTIVE");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON").HasPrecision(3);

        builder.HasOne(e => e.Meeting)
            .WithMany(m => m.Polls)
            .HasForeignKey(e => e.MeetingId)
            .HasConstraintName("FK_SRF_POLL_DETAIL_MEETINGSCH");

        builder.HasIndex(e => e.MeetingId);
        builder.HasIndex(e => e.PollStatus);
    }
}
