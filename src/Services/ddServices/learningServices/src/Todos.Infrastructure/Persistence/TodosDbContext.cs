using Microsoft.EntityFrameworkCore;
using Todos.Domain.Abstractions;
using Todos.Domain.Entities;

namespace Todos.Infrastructure.Persistence;

/// <summary>
/// Application DbContext for Learning module
/// </summary>
public class TodosDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet for learning records
    /// </summary>
    public DbSet<LearningRecord> LearningRecords { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for learning sub-records
    /// </summary>
    public DbSet<LearningSubRecord> LearningSubRecords { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for learning feedback records
    /// </summary>
    public DbSet<LearningFeedback> LearningFeedbacks { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for development category details
    /// </summary>
    public DbSet<DevelopmentCategoryDetail> DevelopmentCategoryDetails { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the TodosDbContext class
    /// </summary>
    public TodosDbContext(DbContextOptions<TodosDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Learning Record configuration
        modelBuilder.Entity<LearningRecord>(entity =>
        {
            entity.ToTable("LET_MAIN09");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LET_GUID");

            entity.Property(e => e.LetId).HasColumnName("LET_ID");
            entity.Property(e => e.Version).HasColumnName("LET_VERSION");
            entity.Property(e => e.CreatedAt).HasColumnName("LET_CREATED_AT");
            entity.Property(e => e.UpdatedAt).HasColumnName("LET_UPDATED_AT");

            // Value object conversions
            entity.OwnsOne(e => e.RequestNumber, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_DD_REQNO");
            });

            entity.OwnsOne(e => e.EmployeeId, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_EMPLOYEE_ID");
            });

            entity.Property(e => e.DevelopmentSourceId).HasColumnName("LET_DEV_SOURCE");
            entity.Property(e => e.SpecificNeed).HasColumnName("LET_SPECIFIC_NEED").HasMaxLength(2000);
            entity.Property(e => e.Indicator).HasColumnName("LET_INDICATOR").HasMaxLength(2000);
            entity.Property(e => e.DevelopmentArea).HasColumnName("LET_DEV_AREA").HasMaxLength(2000);
            entity.Property(e => e.ExpectedPostTraining).HasColumnName("LET_EXPECTEDPOST_TRAINING").HasMaxLength(2000);

            entity.OwnsOne(e => e.BhrStatus, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_BHRSTATUS");
            });

            entity.Property(e => e.ReviewerComments).HasColumnName("LET_REVIEWER_COMMENTS").HasMaxLength(2000);
            entity.Property(e => e.AppraiseeOpinion).HasColumnName("LET_APP_OPINION").HasMaxLength(2000);
            entity.Property(e => e.AppraiserComments).HasColumnName("LET_APPR_COMMENTS").HasMaxLength(2000);
            entity.Property(e => e.ModifiedBy).HasColumnName("LET_MODIFIEDBY");

            // Relationships
            entity.HasMany(e => e.SubRecords)
                .WithOne()
                .HasForeignKey(sr => sr.LearningRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Learning Sub Record configuration
        modelBuilder.Entity<LearningSubRecord>(entity =>
        {
            entity.ToTable("LET_SUB09");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LET_SUB_GUID");

            entity.Property(e => e.SubId).HasColumnName("LET_MODID");
            entity.Property(e => e.LearningRecordId).HasColumnName("LET_RECORD_ID");

            entity.OwnsOne(e => e.RequestNumber, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_DD_REQNO");
            });

            entity.Property(e => e.DevelopmentModeId).HasColumnName("LET_DEVELOPMEN_MODE");

            entity.OwnsOne(e => e.TrainingId, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_TRAINING_ID");
            });

            entity.Property(e => e.TrainingDetail).HasColumnName("LET_TRAINING_DET").HasMaxLength(1000);
            entity.Property(e => e.Remarks).HasColumnName("LET_REMARKS").HasMaxLength(200);
            entity.Property(e => e.DevelopmentId).HasColumnName("LET_DEVELOPMENTID");
            entity.Property(e => e.FinalReview).HasColumnName("LET_FINALREVIEW").HasMaxLength(50);
        });

        // Learning Feedback configuration
        modelBuilder.Entity<LearningFeedback>(entity =>
        {
            entity.ToTable("LET_FEEDBACK");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LET_FEEDBACK_GUID");

            entity.Property(e => e.FeedbackId).HasColumnName("LET_SRL");
            entity.Property(e => e.Version).HasColumnName("LET_FEEDBACK_VERSION");
            entity.Property(e => e.CreatedAt).HasColumnName("LET_FEEDBACK_CREATED_AT");
            entity.Property(e => e.UpdatedAt).HasColumnName("LET_FEEDBACK_UPDATED_AT");

            entity.OwnsOne(e => e.RequestNumber, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_DDREQNO");
            });

            entity.Property(e => e.SpecificNeed).HasColumnName("LET_SPECIFIC_NEED").HasMaxLength(2000);
            entity.Property(e => e.TrainingProgram).HasColumnName("LET_TRAINING").HasMaxLength(500);

            entity.OwnsOne(e => e.FeedbackStatus, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_FEEDBACK_STATUS");
            });

            entity.Property(e => e.AppraiseeComments).HasColumnName("LET_APPRAISEE_COMMENTS").HasMaxLength(2000);
            entity.Property(e => e.AppraiserComments).HasColumnName("LET_APPRAISER_COMMENTS").HasMaxLength(2000);
            entity.Property(e => e.ReviewerComments).HasColumnName("LET_REVIEWER_COMMENTS").HasMaxLength(2000);
            entity.Property(e => e.ModifiedBy).HasColumnName("LET_MODIFIEDBY");

            entity.OwnsOne(e => e.AppraiserNeedStatus, nav =>
            {
                nav.Property(p => p.Value).HasColumnName("LET_APPR_NEEDSTATUS");
            });

            entity.Property(e => e.AppraiserPostTraining).HasColumnName("LET_APPR_POSTTRAINING").HasMaxLength(2000);
        });

        // Development Category Detail configuration
        modelBuilder.Entity<DevelopmentCategoryDetail>(entity =>
        {
            entity.ToTable("DD_CAT_DEV_DETAIL");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("CAT_DEV_GUID");

            entity.Property(e => e.RequestNumber).HasColumnName("CT_REQ_NUM");
            entity.Property(e => e.QuestionNumber).HasColumnName("CT_QTN_NUM");
            entity.Property(e => e.AnswerSerial).HasColumnName("CT_ANS_SRL");
            entity.Property(e => e.EmployeeId).HasColumnName("CT_APP_ID").HasMaxLength(30);
            entity.Property(e => e.EmployeeNumber).HasColumnName("CT_APP_NUM");
            entity.Property(e => e.EntryDate).HasColumnName("CT_ENT_DAT");
            entity.Property(e => e.DevelopmentArea).HasColumnName("CT_DESC").HasMaxLength(400);
            entity.Property(e => e.Need).HasColumnName("CT_NEED").HasMaxLength(400);
        });
    }
}
