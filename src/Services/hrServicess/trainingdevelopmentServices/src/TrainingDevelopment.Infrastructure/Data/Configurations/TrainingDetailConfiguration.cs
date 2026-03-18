using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Infrastructure.Data.Configurations;

public class TrainingDetailConfiguration : IEntityTypeConfiguration<TrainingDetail>
{
    public void Configure(EntityTypeBuilder<TrainingDetail> builder)
    {
        builder.ToTable("TRAINING_DET");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TR_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.FinancialYear).HasColumnName("TR_FINYEAR").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.EmployeeSysId).HasColumnName("TR_EMPSYSID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.TrainingNeed).HasColumnName("TR_NEED").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.GapArea).HasColumnName("TR_GAPS").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Mode).HasColumnName("TR_MODE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ProgramId).HasColumnName("TR_PROGRAMID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ProgramDescription).HasColumnName("TR_PROGRAMDESC").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.PlannedFrom).HasColumnName("TR_PLANFROM").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.PlannedTo).HasColumnName("TR_PLANTO").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.Status).HasColumnName("TR_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ActualFrom).HasColumnName("TR_ACTFROM").HasColumnType("datetime2(3)");
        builder.Property(x => x.ActualTo).HasColumnName("TR_ACTTO").HasColumnType("datetime2(3)");
        builder.Property(x => x.InstituteId).HasColumnName("TR_INSTITUTEID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.InstituteDescription).HasColumnName("TR_INSTITUTEDESC").HasMaxLength(1000);
        builder.Property(x => x.TrainerId).HasColumnName("TR_TRAINERID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.TrainerDescription).HasColumnName("TR_TRAINERDESC").HasMaxLength(65);
        builder.Property(x => x.PlaceId).HasColumnName("TR_PLACEID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.Place).HasColumnName("TR_PLACE").HasMaxLength(65);
        builder.Property(x => x.Cost).HasColumnName("TR_COST").HasColumnType("decimal(38,0)");
        builder.Property(x => x.DroppedRemarks).HasColumnName("TR_DROPREMARKS").HasMaxLength(1000);
        builder.Property(x => x.LastModifiedBy).HasColumnName("TR_LASTMODIFIEDBY").HasColumnType("decimal(22,0)");
        builder.Property(x => x.LastModifiedOn).HasColumnName("TR_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(x => x.EmployeeSysId);
        builder.HasIndex(x => x.FinancialYear);
        builder.HasIndex(x => x.Status);
    }
}
