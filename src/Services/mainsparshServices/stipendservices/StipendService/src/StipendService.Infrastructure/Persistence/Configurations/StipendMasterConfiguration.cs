using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StipendService.Domain.Entities;

namespace StipendService.Infrastructure.Persistence.Configurations;

public class StipendMasterConfiguration : IEntityTypeConfiguration<StipendMaster>
{
    public void Configure(EntityTypeBuilder<StipendMaster> builder)
    {
        builder.ToTable("SRF_STIPEND_MASTER");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("STIPEND_ID")
            .UseIdentityColumn(1, 1);

        builder.Property(x => x.ResearchCategoryId).HasColumnName("RESEARCH_CATEGORY_ID").IsRequired();
        builder.Property(x => x.SrfRankId).HasColumnName("SRF_RANK_ID").IsRequired();
        builder.Property(x => x.SrfMonthlyStipend).HasColumnName("SRF_MONTHLY_STIPEND").HasColumnType("decimal(19,2)").IsRequired();
        builder.Property(x => x.AdditionalAllowance).HasColumnName("ADDITIONAL_ALLOWANCE").HasColumnType("decimal(19,2)");
        builder.Property(x => x.EffectiveFrom).HasColumnName("EFFECTIVE_FROM").HasColumnType("date").IsRequired();
        builder.Property(x => x.EffectiveTo).HasColumnName("EFFECTIVE_TO").HasColumnType("date");
        builder.Property(x => x.Status).HasColumnName("STATUS").HasMaxLength(1).HasDefaultValue("A");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("datetime2(3)");

        builder.HasIndex(x => new { x.ResearchCategoryId, x.SrfRankId })
            .IsUnique()
            .HasDatabaseName("UC_STIPEND_CATEGORY_RANK");

        builder.HasIndex(x => x.ResearchCategoryId).HasDatabaseName("IX_SRF_STIPEND_MASTER_CATEGORY");
        builder.HasIndex(x => x.SrfRankId).HasDatabaseName("IX_SRF_STIPEND_MASTER_RANK");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_SRF_STIPEND_MASTER_STATUS");
        builder.HasIndex(x => new { x.EffectiveFrom, x.EffectiveTo }).HasDatabaseName("IX_SRF_STIPEND_MASTER_EFFECTIVE");

        builder.HasMany(x => x.Disbursements)
            .WithOne(x => x.StipendMaster)
            .HasForeignKey(x => x.StipendId)
            .HasConstraintName("FK_SRF_STIPEND_DISBURSE_MASTER");
    }
}
