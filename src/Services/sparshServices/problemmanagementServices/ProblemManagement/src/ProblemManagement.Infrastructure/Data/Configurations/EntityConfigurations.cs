using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProblemManagement.Domain.Entities;

namespace ProblemManagement.Infrastructure.Data.Configurations;

public class ProblemFunctionConfiguration : IEntityTypeConfiguration<ProblemFunction>
{
    public void Configure(EntityTypeBuilder<ProblemFunction> builder)
    {
        builder.ToTable("PROBLEM_FUNCTION");
        builder.HasKey(e => e.FuncId);
        builder.Property(e => e.FuncId).HasColumnName("FUNCID");
        builder.Property(e => e.FuncName).HasColumnName("FUNCNAME").HasMaxLength(200).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProblemImpactConfiguration : IEntityTypeConfiguration<ProblemImpact>
{
    public void Configure(EntityTypeBuilder<ProblemImpact> builder)
    {
        builder.ToTable("PROBLEM_IMPACT");
        builder.HasKey(e => e.ImpactId);
        builder.Property(e => e.ImpactId).HasColumnName("IMPACT_ID");
        builder.Property(e => e.ImpactDesc).HasColumnName("IMPACT_DESC").HasMaxLength(200).IsRequired();
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProblemMainConfiguration : IEntityTypeConfiguration<ProblemMain>
{
    public void Configure(EntityTypeBuilder<ProblemMain> builder)
    {
        builder.ToTable("PROBLEM_MAIN");
        builder.HasKey(e => e.PrId);

        builder.Property(e => e.PrId).HasColumnName("PR_ID").UseIdentityColumn();
        builder.Property(e => e.PrOwner).HasColumnName("PR_OWNER");
        builder.Property(e => e.PrEnteredBy).HasColumnName("PR_ENTEREDBY");
        builder.Property(e => e.PrDescription).HasColumnName("PR_DESCRIPTION").HasMaxLength(255).IsRequired();
        builder.Property(e => e.PrRespExpBy).HasColumnName("PR_RESPEXPBY");
        builder.Property(e => e.PrCategory).HasColumnName("PR_CATEGORY").HasColumnType("char(1)");
        builder.Property(e => e.PrSpecialization).HasColumnName("PR_SPECIALIZATION");
        builder.Property(e => e.PrImpact).HasColumnName("PR_IMPACT").HasMaxLength(255);
        builder.Property(e => e.PrExpResult).HasColumnName("PR_EXPRESULT").HasMaxLength(255);
        builder.Property(e => e.PrEnteredOn).HasColumnName("PR_ENTEREDON");
        builder.Property(e => e.PrStatus).HasColumnName("PR_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.PrAppId).HasColumnName("PR_APPID");
        builder.Property(e => e.PrStatement).HasColumnName("PR_STATEMENT").HasMaxLength(255);
        builder.Property(e => e.PrType).HasColumnName("PR_TYPE").HasColumnType("char(1)");
        builder.Property(e => e.PrAttach).HasColumnName("PR_ATTACH").HasMaxLength(255);
        builder.Property(e => e.PrPrbFlag).HasColumnName("PR_PRBFLAG").HasColumnType("char(1)");
        builder.Property(e => e.PrPrbDescription).HasColumnName("PR_PRBDESCRIPTION").HasMaxLength(255);
        builder.Property(e => e.PrPostFlag).HasColumnName("PR_POSTFLAG").HasColumnType("char(1)");
        builder.Property(e => e.PrQuestion).HasColumnName("PR_QUESTION").HasMaxLength(255);
        builder.Property(e => e.PrUnitId).HasColumnName("PR_UNITID");
        builder.Property(e => e.PrSiteId).HasColumnName("PR_SITEID");
        builder.Property(e => e.PrSourceId).HasColumnName("PR_SOURCEID");
        builder.Property(e => e.PrModBy).HasColumnName("PR_MODBY");
        builder.Property(e => e.PrModOn).HasColumnName("PR_MODON");

        builder.HasIndex(e => e.PrStatus).HasDatabaseName("IX_PROBLEM_MAIN_STATUS");
        builder.HasIndex(e => e.PrOwner).HasDatabaseName("IX_PROBLEM_MAIN_OWNER");
        builder.HasIndex(e => e.PrCategory).HasDatabaseName("IX_PROBLEM_MAIN_CATEGORY");

        builder.HasMany(e => e.Attachments).WithOne(a => a.Problem).HasForeignKey(a => a.PratPrId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Solutions).WithOne(s => s.Problem).HasForeignKey(s => s.SolPrId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Approvals).WithOne(a => a.Problem).HasForeignKey(a => a.PrAppPrId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Audiences).WithOne(a => a.Problem).HasForeignKey(a => a.PrAudPrId).OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProblemAttachmentConfiguration : IEntityTypeConfiguration<ProblemAttachment>
{
    public void Configure(EntityTypeBuilder<ProblemAttachment> builder)
    {
        builder.ToTable("PROBLEM_ATTACHMENT");
        builder.HasKey(e => e.PratId);
        builder.Property(e => e.PratId).HasColumnName("PRAT_ID").UseIdentityColumn();
        builder.Property(e => e.PratPrId).HasColumnName("PRAT_PRID");
        builder.Property(e => e.PratFileName).HasColumnName("PRAT_FILENAME").HasMaxLength(2000);
        builder.Property(e => e.PratEnteredOn).HasColumnName("PRAT_ENTEREDON");
        builder.HasIndex(e => e.PratPrId).HasDatabaseName("IX_PROBLEM_ATTACHMENT_PRID");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProblemSolutionConfiguration : IEntityTypeConfiguration<ProblemSolution>
{
    public void Configure(EntityTypeBuilder<ProblemSolution> builder)
    {
        builder.ToTable("PROBLEM_SOLUTION");
        builder.HasKey(e => e.SolId);
        builder.Property(e => e.SolId).HasColumnName("SOL_ID").UseIdentityColumn();
        builder.Property(e => e.SolPrId).HasColumnName("SOL_PRID");
        builder.Property(e => e.SolDescription).HasColumnName("SOL_DESCRIPTION").HasMaxLength(255);
        builder.Property(e => e.SolImplementation).HasColumnName("SOL_IMPLEMENTATION").HasColumnType("char(1)");
        builder.Property(e => e.SolEnteredBy).HasColumnName("SOL_ENTEREDBY");
        builder.Property(e => e.SolEnteredOn).HasColumnName("SOL_ENTEREDON");
        builder.Property(e => e.SolAttach).HasColumnName("SOL_ATTACH").HasMaxLength(255);

        builder.HasIndex(e => e.SolPrId).HasDatabaseName("IX_PROBLEM_SOLUTION_PRID");
        builder.HasIndex(e => e.SolEnteredBy).HasDatabaseName("IX_PROBLEM_SOLUTION_ENTEREDBY");

        builder.HasMany(e => e.SolutionApprovals).WithOne(a => a.Solution).HasForeignKey(a => a.SolAppSolId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.SolutionComments).WithOne(c => c.Solution).HasForeignKey(c => c.SolCommentSolId).OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProblemApprovalConfiguration : IEntityTypeConfiguration<ProblemApproval>
{
    public void Configure(EntityTypeBuilder<ProblemApproval> builder)
    {
        builder.ToTable("PROBLEM_APP");
        builder.HasKey(e => e.PrAppId);
        builder.Property(e => e.PrAppId).HasColumnName("PRAPP_ID").UseIdentityColumn();
        builder.Property(e => e.PrAppPrId).HasColumnName("PRAPP_PRID");
        builder.Property(e => e.PrAppBy).HasColumnName("PRAPP_BY");
        builder.Property(e => e.PrAppOn).HasColumnName("PRAPP_ON");
        builder.Property(e => e.PrAppStatus).HasColumnName("PRAPP_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.PrAppReason).HasColumnName("PRAPP_REASON").HasMaxLength(255);
        builder.Property(e => e.PrAppAudFlag).HasColumnName("PRAPP_AUDFLAG").HasColumnType("char(1)").IsRequired();
        builder.HasIndex(e => e.PrAppPrId).HasDatabaseName("IX_PROBLEM_APP_PRID");
        builder.HasIndex(e => e.PrAppStatus).HasDatabaseName("IX_PROBLEM_APP_STATUS");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class ProblemAppAudienceConfiguration : IEntityTypeConfiguration<ProblemAppAudience>
{
    public void Configure(EntityTypeBuilder<ProblemAppAudience> builder)
    {
        builder.ToTable("PROBLEM_APPAUDIENCE");
        builder.HasKey(e => e.PrAudId);
        builder.Property(e => e.PrAudId).HasColumnName("PRAUD_ID").UseIdentityColumn();
        builder.Property(e => e.PrAudPrId).HasColumnName("PRAUD_PRID");
        builder.Property(e => e.PrAudUnitId).HasColumnName("PRAUD_UNITID");
        builder.HasIndex(e => e.PrAudPrId).HasDatabaseName("IX_PROBLEM_APPAUDIENCE_PRID");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SolutionApprovalConfiguration : IEntityTypeConfiguration<SolutionApproval>
{
    public void Configure(EntityTypeBuilder<SolutionApproval> builder)
    {
        builder.ToTable("SOLUTION_APP");
        builder.HasKey(e => e.SolAppId);
        builder.Property(e => e.SolAppId).HasColumnName("SOLAPP_ID").UseIdentityColumn();
        builder.Property(e => e.SolAppSolId).HasColumnName("SOLAPP_SOLID");
        builder.Property(e => e.SolAppBy).HasColumnName("SOLAPP_BY");
        builder.Property(e => e.SolAppOn).HasColumnName("SOLAPP_ON");
        builder.Property(e => e.SolAppStatus).HasColumnName("SOLAPP_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.SolAppReason).HasColumnName("SOLAPP_REASON").HasMaxLength(255);
        builder.Property(e => e.SolAppAudFlag).HasColumnName("SOLAPP_AUDFLAG").HasColumnType("char(1)");
        builder.HasIndex(e => e.SolAppSolId).HasDatabaseName("IX_SOLUTION_APP_SOLID");
        builder.HasIndex(e => e.SolAppStatus).HasDatabaseName("IX_SOLUTION_APP_STATUS");
        builder.Ignore(e => e.DomainEvents);
    }
}

public class SolutionCommentConfiguration : IEntityTypeConfiguration<SolutionComment>
{
    public void Configure(EntityTypeBuilder<SolutionComment> builder)
    {
        builder.ToTable("SOLUTION_COMMENT");
        builder.HasKey(e => e.SolCommentId);
        builder.Property(e => e.SolCommentId).HasColumnName("SOLCOMMENT_ID").UseIdentityColumn();
        builder.Property(e => e.SolCommentSolId).HasColumnName("SOLCOMMENT_SOLID");
        builder.Property(e => e.SolCommentText).HasColumnName("SOLCOMMENT_TEXT").HasMaxLength(500).IsRequired();
        builder.Property(e => e.SolCommentBy).HasColumnName("SOLCOMMENT_BY");
        builder.Property(e => e.SolCommentOn).HasColumnName("SOLCOMMENT_ON");
        builder.HasIndex(e => e.SolCommentSolId).HasDatabaseName("IX_SOLUTION_COMMENT_SOLID");
        builder.Ignore(e => e.DomainEvents);
    }
}
