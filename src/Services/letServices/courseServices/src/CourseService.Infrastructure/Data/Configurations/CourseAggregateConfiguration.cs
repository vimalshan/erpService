using CourseService.Domain.Aggregates;
using CourseService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseService.Infrastructure.Data.Configurations;

public class CourseAggregateConfiguration : IEntityTypeConfiguration<CourseAggregate>
{
    public void Configure(EntityTypeBuilder<CourseAggregate> builder)
    {
        builder.ToTable("COURSE_MAST");
        builder.HasKey(c => c.CourseId);
        builder.Property(c => c.CourseId).HasColumnName("CR_CRS_ID").ValueGeneratedNever();
        builder.Property(c => c.CourseType).HasColumnName("CR_CRS_TYP").HasMaxLength(1).IsRequired();
        builder.Property(c => c.CourseDescription).HasColumnName("CR_CRS_DES").HasMaxLength(255).IsRequired();
        builder.Property(c => c.ObjectiveDescription).HasColumnName("CR_OBJ_DES").HasMaxLength(255).IsRequired();
        builder.Property(c => c.EffectiveDate).HasColumnName("CR_EFF_DAT").IsRequired();
        builder.Property(c => c.ClosingDate).HasColumnName("CR_CLS_DAT").IsRequired();
        builder.Property(c => c.LastDate).HasColumnName("CR_LST_DAT").IsRequired();
        builder.Property(c => c.TrainingType).HasColumnName("CR_TRN_TYP").HasMaxLength(1).IsRequired();
        builder.Property(c => c.CancellationDate).HasColumnName("CR_CAN_DAT");
        builder.Property(c => c.CancellationRemark).HasColumnName("CR_CAN_REM").HasMaxLength(255);
        builder.Property(c => c.PendingDate).HasColumnName("CR_PEN_DAT");
        builder.Property(c => c.FileName).HasColumnName("CR_FIL_NAM").HasMaxLength(255);
        builder.Property(c => c.ThumbnailPicture).HasColumnName("CR_THMB_PIC").HasMaxLength(255);
        builder.Property(c => c.TrainerRating).HasColumnName("CR_TRN_RAT").HasPrecision(18, 4);
        builder.Property(c => c.ContentRating).HasColumnName("CR_CNT_RAT").HasPrecision(18, 4);
        builder.Property(c => c.AdminRating).HasColumnName("CR_ADM_RAT").HasPrecision(18, 4);
        builder.Property(c => c.EvaluationId).HasColumnName("CR_EVAL_ID");

        // Address value object
        builder.OwnsOne(c => c.Address, addr =>
        {
            addr.Property(a => a.LocationCode).HasColumnName("CR_LOC_COD").HasMaxLength(1).IsRequired();
            addr.Property(a => a.AddressLine1).HasColumnName("CR_ADD_LN1").HasMaxLength(255).IsRequired();
            addr.Property(a => a.AddressLine2).HasColumnName("CR_ADD_LN2").HasMaxLength(255).IsRequired();
            addr.Property(a => a.AddressLine3).HasColumnName("CR_ADD_LN3").HasMaxLength(255).IsRequired();
            addr.Property(a => a.PinCode).HasColumnName("CR_PIN_COD").IsRequired();
            addr.Property(a => a.PhoneNumber).HasColumnName("CR_PHN_NUM").HasMaxLength(255).IsRequired();
        });

        // Duration value object
        builder.OwnsOne(c => c.Duration, dur =>
        {
            dur.Property(d => d.StartDate).HasColumnName("CR_STR_DAT").IsRequired();
            dur.Property(d => d.EndDate).HasColumnName("CR_END_DAT").IsRequired();
            dur.Property(d => d.NumberOfDays).HasColumnName("CR_NO_DYS").IsRequired();
            dur.Property(d => d.DurationDisplay).HasColumnName("CR_CRS_DUR").HasMaxLength(255);
        });

        // TrainerInfo value object
        builder.OwnsOne(c => c.TrainerInfo, ti =>
        {
            ti.Property(t => t.TrainerName1).HasColumnName("CR_TRN_NAM1").HasMaxLength(255);
            ti.Property(t => t.TrainerName2).HasColumnName("CR_TRN_NAM2").HasMaxLength(255);
            ti.Property(t => t.TrainerName3).HasColumnName("CR_TRN_NAM3").HasMaxLength(255);
            ti.Property(t => t.TrainerDesignation1).HasColumnName("CR_TRN_DES1").HasMaxLength(255);
            ti.Property(t => t.TrainerDesignation2).HasColumnName("CR_TRN_DES2").HasMaxLength(255);
            ti.Property(t => t.TrainerDesignation3).HasColumnName("CR_TRN_DES3").HasMaxLength(255);
            ti.Property(t => t.TrainerContact1).HasColumnName("CR_TRN_CNT1").HasMaxLength(255);
            ti.Property(t => t.TrainerContact2).HasColumnName("CR_TRN_CNT2").HasMaxLength(255);
            ti.Property(t => t.TrainerContact3).HasColumnName("CR_TRN_CNT3").HasMaxLength(255);
            ti.Property(t => t.TrainerCode).HasColumnName("CR_TRN_COD");
        });

        // Ignore domain events collection - not persisted
        builder.Ignore(c => c.DomainEvents);

        // Navigation properties (child collections stored via separate tables)
        builder.HasMany(c => c.Schedules).WithOne().HasForeignKey(s => s.CourseId);
        builder.HasMany(c => c.Participants).WithOne().HasForeignKey(p => p.CourseId);
        builder.HasMany(c => c.Bands).WithOne().HasForeignKey(b => b.CourseBandCourseId);
        builder.HasMany(c => c.Costs).WithOne().HasForeignKey(cc => cc.CourseId);
        builder.HasMany(c => c.Models).WithOne().HasForeignKey(m => m.CourseId);

        builder.HasIndex(c => c.CourseType).HasDatabaseName("IDX_COURSE_MAST_CRS_TYP");
    }
}
