using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DocumentService.Domain.Entities;

namespace DocumentService.Infrastructure.Data.Configurations;

public class LoanDocumentConfiguration : IEntityTypeConfiguration<LoanDocument>
{
    public void Configure(EntityTypeBuilder<LoanDocument> builder)
    {
        builder.ToTable("LOAN_DOCUMENTS");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("LOANDOC_ID")
            .ValueGeneratedNever();

        builder.Property(x => x.LoanId)
            .HasColumnName("LOANDOC_LOANID")
            .IsRequired();

        builder.Property(x => x.TypeId)
            .HasColumnName("LOANDOC_TYPEID")
            .IsRequired();

        builder.Property(x => x.LastModifiedBy)
            .HasColumnName("LOANDOC_LASTMODIFIEDBY")
            .IsRequired();

        builder.Property(x => x.LastModifiedOn)
            .HasColumnName("LOANDOC_LASTMODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        // Ignore the domain events collection — it is not persisted
        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(x => x.LoanId).HasDatabaseName("IX_LOAN_DOCUMENTS_LOANID");
        builder.HasIndex(x => x.TypeId).HasDatabaseName("IX_LOAN_DOCUMENTS_TYPEID");
    }
}
