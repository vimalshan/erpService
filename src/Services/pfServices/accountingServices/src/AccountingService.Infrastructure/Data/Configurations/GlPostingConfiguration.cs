using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class GlPostingConfiguration : IEntityTypeConfiguration<GlPosting>
{
    public void Configure(EntityTypeBuilder<GlPosting> builder)
    {
        builder.ToTable("GL_POSTING");
        builder.HasKey(x => x.PostingId);
        builder.Property(x => x.PostingId).HasColumnName("POSTING_ID").UseIdentityColumn();
        builder.Property(x => x.AccountCode).HasColumnName("ACCOUNT_CODE").HasMaxLength(10).IsRequired();
        builder.Property(x => x.PostingDate).HasColumnName("POSTING_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.DebitAmount).HasColumnName("DEBIT_AMOUNT").HasColumnType("DECIMAL(19,0)").HasDefaultValue(0);
        builder.Property(x => x.CreditAmount).HasColumnName("CREDIT_AMOUNT").HasColumnType("DECIMAL(19,0)").HasDefaultValue(0);
        builder.Property(x => x.ReferenceId).HasColumnName("REFERENCE_ID").IsRequired();
        builder.Property(x => x.PostingRemarks).HasColumnName("POSTING_REMARKS").HasMaxLength(200);

        builder.HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(x => x.AccountCode)
            .HasConstraintName("FK_GL_POSTING_ACCOUNT");

        builder.HasIndex(x => new { x.AccountCode, x.PostingDate }).HasDatabaseName("IDX_GL_POSTING_ACCOUNT");
    }
}
