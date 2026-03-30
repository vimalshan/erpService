using ExitManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExitManagement.Infrastructure.Persistence.Configurations;

public class ExitInterviewFeedbackConfiguration : IEntityTypeConfiguration<ExitInterviewFeedback>
{
    public void Configure(EntityTypeBuilder<ExitInterviewFeedback> builder)
    {
        builder.ToTable("EMPLOYEE_EXIT_INT");

        builder.HasKey(e => new { e.ExitNo, e.SerialNo });
        builder.Property(e => e.ExitNo).HasColumnName("INT_EXITNO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.SerialNo).HasColumnName("INT_SLNO").HasColumnType("decimal(38,0)");
        builder.Property(e => e.QuestionId).HasColumnName("INT_QUES_ID").HasMaxLength(4);
        builder.Property(e => e.Feedback).HasColumnName("INT_FEEDBACK").HasMaxLength(1000);
        builder.Property(e => e.UpdatedBy).HasColumnName("INT_UPDATED_BY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.UpdatedOn).HasColumnName("INT_UPDATED_ON");
        builder.Ignore(e => e.CreatedOn);
    }
}

public class ExitQuestionConfiguration : IEntityTypeConfiguration<ExitQuestion>
{
    public void Configure(EntityTypeBuilder<ExitQuestion> builder)
    {
        builder.ToTable("TT_EXIT_QUESTIONS");
        builder.HasNoKey();
        builder.Property(e => e.QuestionId).HasColumnName("QUESTION_ID").HasMaxLength(4);
        builder.Property(e => e.QuestionDescription).HasColumnName("QUESTION_DESC").HasMaxLength(500);
        builder.Property(e => e.QuestionOrder).HasColumnName("QUESTION_ORDER").HasColumnType("decimal(22,0)");
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.UpdatedBy);
        builder.Ignore(e => e.UpdatedOn);
    }
}

public class ExitInterviewQuestionConfiguration : IEntityTypeConfiguration<ExitInterviewQuestion>
{
    public void Configure(EntityTypeBuilder<ExitInterviewQuestion> builder)
    {
        builder.ToTable("TT_EXIT_INTERVIEW");
        builder.HasNoKey();
        builder.Property(e => e.QuestionId).HasColumnName("QUESTION_ID").HasMaxLength(3);
        builder.Property(e => e.QuestionDescription).HasColumnName("QUESTION_DESC").HasMaxLength(500);
        builder.Property(e => e.OrderId).HasColumnName("ORDER_ID").HasColumnType("decimal(38,0)");
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.UpdatedBy);
        builder.Ignore(e => e.UpdatedOn);
    }
}

public class ExitResponsibilityMapConfiguration : IEntityTypeConfiguration<ExitResponsibilityMap>
{
    public void Configure(EntityTypeBuilder<ExitResponsibilityMap> builder)
    {
        builder.ToTable("TT_EMPLOYEE_EXITRESPEX");
        builder.HasNoKey();
        builder.Property(e => e.TtId).HasColumnName("TT_ID").HasColumnType("decimal(22,0)");
        builder.Property(e => e.EmployeeSysId).HasColumnName("TT_SYSID").HasColumnType("decimal(22,0)");
        builder.Property(e => e.ChecklistMapId).HasColumnName("TT_CHKMAPID").HasColumnType("decimal(22,0)");
        builder.Property(e => e.Primary).HasColumnName("TT_PRI").HasMaxLength(30);
        builder.Property(e => e.Secondary).HasColumnName("TT_SEC").HasMaxLength(30);
        builder.Property(e => e.FunctionalHead).HasColumnName("TT_FHEAD").HasMaxLength(30);
        builder.Ignore(e => e.CreatedOn);
        builder.Ignore(e => e.UpdatedBy);
        builder.Ignore(e => e.UpdatedOn);
    }
}
