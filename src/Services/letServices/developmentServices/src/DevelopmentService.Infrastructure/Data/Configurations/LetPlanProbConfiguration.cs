using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Infrastructure.Data.Configurations;

public class LetPlanProbConfiguration : IEntityTypeConfiguration<LetPlanProb>
{
    public void Configure(EntityTypeBuilder<LetPlanProb> builder)
    {
        builder.ToTable("DD_LETPLAN_PROB");
        builder.HasNoKey();
        builder.Property(x => x.ReqNum).HasColumnName("DD_REQNUM");
        builder.Property(x => x.Sno).HasColumnName("DD_SNO");
        builder.Property(x => x.UserId).HasColumnName("DD_USERID").HasMaxLength(255);
        builder.Property(x => x.PinNum).HasColumnName("DD_PINNUM");
        builder.Property(x => x.DevSource).HasColumnName("DD_DEVSOURCE").HasMaxLength(255);
        builder.Property(x => x.DevNeed).HasColumnName("DD_DEVNEED").HasMaxLength(255);
        builder.Property(x => x.DevIndicator).HasColumnName("DD_DEVINDICATOR").HasMaxLength(255);
        builder.Property(x => x.DevMode).HasColumnName("DD_DEVMODE");
        builder.Property(x => x.RecProg).HasColumnName("DD_RECPROG").HasMaxLength(255);
        builder.Property(x => x.TrainingProgram).HasColumnName("DD_TRAININGPROGRAM").HasMaxLength(255);
        builder.Property(x => x.InternalTraining).HasColumnName("DD_INTERNALTRAINING");
        builder.Property(x => x.RevDate).HasColumnName("DD_REVDATE").HasMaxLength(255);
        builder.Property(x => x.Priority).HasColumnName("DD_PRIORITY");
        builder.Property(x => x.EntDate).HasColumnName("DD_ENTDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.AppStatus).HasColumnName("DD_APPSTATUS").HasMaxLength(1);
        builder.Property(x => x.BhrStatus).HasColumnName("DD_BHRSTATUS").HasMaxLength(1);
        builder.Property(x => x.StrDate).HasColumnName("DD_STRDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.EnDate).HasColumnName("DD_ENDATE").HasColumnType("datetime2(3)");
    }
}
