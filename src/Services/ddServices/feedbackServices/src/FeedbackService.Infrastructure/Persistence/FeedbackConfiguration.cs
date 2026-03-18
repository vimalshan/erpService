namespace FeedbackService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates;
using Domain.ValueObjects;

/// <summary>
/// Entity configuration for Feedback aggregate
/// </summary>
public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    /// <summary>
    /// Configures the Feedback entity
    /// </summary>
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("APP_FEEDBACKMAIN");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("FB_FEEDBACKID")
            .IsRequired();

        builder.Property(f => f.RequestNo)
            .HasColumnName("FB_REQUESTNO")
            .IsRequired();

        builder.Property(f => f.ApproverSystemId)
            .HasColumnName("FB_APPRSYSID")
            .IsRequired();

        builder.Property(f => f.Remarks)
            .HasColumnName("FB_REMARKS")
            .HasMaxLength(2000);

        builder.Property(f => f.CreatedOn)
            .HasColumnName("CREATEDON")
            .IsRequired();

        builder.Property(f => f.UpdatedOn)
            .HasColumnName("UPDATEDON");

        builder.Property(f => f.Status)
            .HasColumnName("FB_STATUS")
            .HasConversion(
                v => v != null ? v.Value : null,
                v => v != null ? new FeedbackStatus(v) : null)
            .HasMaxLength(1);

        builder.HasMany(f => f.Items)
            .WithOne()
            .HasForeignKey(fi => fi.FeedbackId);

        builder.Navigation(f => f.Items).AutoInclude();
    }
}
