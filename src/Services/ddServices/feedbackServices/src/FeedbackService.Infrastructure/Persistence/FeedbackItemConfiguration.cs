namespace FeedbackService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

/// <summary>
/// Entity configuration for FeedbackItem
/// </summary>
public class FeedbackItemConfiguration : IEntityTypeConfiguration<FeedbackItem>
{
    /// <summary>
    /// Configures the FeedbackItem entity
    /// </summary>
    public void Configure(EntityTypeBuilder<FeedbackItem> builder)
    {
        builder.ToTable("APP_FEEDBACKSUB");

        builder.HasKey(fi => new { fi.FeedbackId, fi.QuestionNo });

        builder.Property(fi => fi.FeedbackId)
            .HasColumnName("FB_FEEDBACKID")
            .IsRequired();

        builder.Property(fi => fi.QuestionNo)
            .HasColumnName("FB_QTNNO")
            .IsRequired();

        builder.Property(fi => fi.AnswerNo)
            .HasColumnName("FB_ANSNO");

        builder.Property(fi => fi.UpdatedOn)
            .HasColumnName("UPDATEDON");
    }
}
