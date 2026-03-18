using DispatchPlanning.Domain.Aggregates;
using DispatchPlanning.Domain.Entities;
using DispatchPlanning.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DispatchPlanning.Infrastructure.Persistence.Configurations;

public class DispatchPlanHeaderConfiguration : IEntityTypeConfiguration<DispatchPlanAggregate>
{
    public void Configure(EntityTypeBuilder<DispatchPlanAggregate> builder)
    {
        builder.ToTable("DISPATCH_PLAN_HEADER");
        builder.HasKey(x => x.DispatchPlanHeaderId);
        builder.Property(x => x.DispatchPlanHeaderId).HasColumnName("DISPATCH_PLAN_HEADER_ID");
        builder.Property(x => x.PlanMonth).HasColumnName("DISPATCH_PLAN_MONTH");
        builder.Property(x => x.PlanMPlus1).HasColumnName("DISPATCH_PLAN_MPLUS1").HasMaxLength(255);
        builder.Property(x => x.PlanMPlus2).HasColumnName("DISPATCH_PLAN_MPLUS2").HasMaxLength(255);
        builder.Property(x => x.PlanMPlus3).HasColumnName("DISPATCH_PLAN_MPLUS3").HasMaxLength(255);
        builder.Property(x => x.PlanMPlus4).HasColumnName("DISPATCH_PLAN_MPLUS4").HasMaxLength(255);
        builder.Property(x => x.EntryDate).HasColumnName("DISPATCH_PLAN_ENTRYDATE");
        builder.Property(x => x.CompanyUnitId).HasColumnName("COMPANY_UNIT_ID");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");

        builder.Property(x => x.PlanType)
            .HasColumnName("DISPATCH_PLAN_TYPE")
            .HasConversion(v => v.Value, v => PlanType.From(v))
            .HasMaxLength(1);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(i => i.DispatchPlanHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SubGroupTargets)
            .WithOne()
            .HasForeignKey(s => s.DispatchPlanHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}

public class DispatchPlanMainGroupConfiguration : IEntityTypeConfiguration<DispatchPlanMainGroup>
{
    public void Configure(EntityTypeBuilder<DispatchPlanMainGroup> builder)
    {
        builder.ToTable("DISPATCH_PLAN_MAINGROUP");
        builder.HasKey(x => x.MainGroupId);
        builder.Property(x => x.MainGroupId).HasColumnName("MAIN_GROUP_ID");
        builder.Property(x => x.MainGroupName).HasColumnName("MAIN_GROUP_NAME").HasMaxLength(20).IsRequired();
        builder.Property(x => x.GroupType).HasColumnName("GROUP_TYPE");
        builder.Property(x => x.ProductSummary).HasColumnName("PRODUCT_SUMMARY");
        builder.Property(x => x.TotalDisplayName).HasColumnName("TOTAL_DISPLAY_NAME").HasMaxLength(20).IsRequired();
        builder.Property(x => x.MgDisplayOrder).HasColumnName("MG_DISPLAY_ORDER");
        builder.Property(x => x.CompanyUnitId).HasColumnName("COMPANY_UNIT_ID");
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");
        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.SubGroups);
    }
}

public class DispatchPlanSubGroupConfiguration : IEntityTypeConfiguration<DispatchPlanSubGroup>
{
    public void Configure(EntityTypeBuilder<DispatchPlanSubGroup> builder)
    {
        builder.ToTable("DISPATCH_PLAN_SUBGROUP");
        builder.HasKey(x => x.SubGroupId);
        builder.Property(x => x.SubGroupId).HasColumnName("SUB_GROUP_ID");
        builder.Property(x => x.MainGroupId).HasColumnName("MAIN_GROUP_ID");
        builder.Property(x => x.SubGroupName).HasColumnName("SUB_GROUP_NAME").HasMaxLength(20).IsRequired();
        builder.Property(x => x.ProductId).HasColumnName("PRODUCT_ID");
        builder.Property(x => x.SgDisplayOrder).HasColumnName("SG_DISPLAY_ORDER");
        builder.Property(x => x.CaptureTotalDirectly).HasColumnName("CAPTURE_TOTAL_DIRECTLY");
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class DispatchPlanBreakupItemConfiguration : IEntityTypeConfiguration<DispatchPlanBreakupItem>
{
    public void Configure(EntityTypeBuilder<DispatchPlanBreakupItem> builder)
    {
        builder.ToTable("DISPATCH_PLAN_BREAKUP_ITEM");
        builder.HasKey(x => x.BreakupItemId);
        builder.Property(x => x.BreakupItemId).HasColumnName("BREAKUP_ITEM_ID");
        builder.Property(x => x.SubGroupId).HasColumnName("SUB_GROUP_ID");
        builder.Property(x => x.ProductId).HasColumnName("PRODUCT_ID");
        builder.Property(x => x.BreakupItemDesc).HasColumnName("BREAKUP_ITEM_DESC").HasMaxLength(4000).IsRequired();
        builder.Property(x => x.UnitId).HasColumnName("UNIT_ID");
        builder.Property(x => x.MainProductUnitsConFactor).HasColumnName("MAIN_PRODUCT_UNITS_CONFACTOR");
        builder.Property(x => x.BiDisplayOrder).HasColumnName("BI_DISPLAY_ORDER");
        builder.Property(x => x.EffectiveDate).HasColumnName("EFFECTIVE_DATE");
        builder.Property(x => x.ClosureDate).HasColumnName("CLOSURE_DATE").HasMaxLength(255);
        builder.Property(x => x.SciUserIdCreated).HasColumnName("SCI_USER_ID_CREATED");
        builder.Property(x => x.CreationDate).HasColumnName("CREATION_DATE");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE").HasMaxLength(255);
        builder.Property(x => x.PackageId).HasColumnName("PACKAGE_ID").HasPrecision(38, 0);
        builder.Ignore(x => x.DomainEvents);
    }
}

public class DispatchPlanItemwiseConfiguration : IEntityTypeConfiguration<DispatchPlanItemwise>
{
    public void Configure(EntityTypeBuilder<DispatchPlanItemwise> builder)
    {
        builder.ToTable("DISPATCH_PLAN_ITEMWISE");
        builder.HasKey(x => new { x.DispatchPlanHeaderId, x.BreakupItemId });
        builder.Property(x => x.DispatchPlanHeaderId).HasColumnName("DISPATCH_PLAN_HEADER_ID");
        builder.Property(x => x.BreakupItemId).HasColumnName("BREAKUP_ITEM_ID");
        builder.Property(x => x.TargetWeek1).HasColumnName("TARGET_WEEK1");
        builder.Property(x => x.TargetWeek2).HasColumnName("TARGET_WEEK2");
        builder.Property(x => x.TargetWeek3).HasColumnName("TARGET_WEEK3");
        builder.Property(x => x.TargetWeek4).HasColumnName("TARGET_WEEK4");
        builder.Property(x => x.TargetWeek5).HasColumnName("TARGET_WEEK5");
        builder.Property(x => x.TargetMPlus1).HasColumnName("TARGET_MPLUS1");
        builder.Property(x => x.TargetMPlus2).HasColumnName("TARGET_MPLUS2");
        builder.Property(x => x.TargetMPlus3).HasColumnName("TARGET_MPLUS3");
        builder.Property(x => x.TargetMPlus4).HasColumnName("TARGET_MPLUS4");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class DispatchPlanSubGroupwiseConfiguration : IEntityTypeConfiguration<DispatchPlanSubGroupwise>
{
    public void Configure(EntityTypeBuilder<DispatchPlanSubGroupwise> builder)
    {
        builder.ToTable("DISPATCH_PLAN_SUBGROUPWISE");
        builder.HasKey(x => new { x.DispatchPlanHeaderId, x.SubGroupId });
        builder.Property(x => x.DispatchPlanHeaderId).HasColumnName("DISPATCH_PLAN_HEADER_ID");
        builder.Property(x => x.SubGroupId).HasColumnName("SUB_GROUP_ID");
        builder.Property(x => x.TargetWeek1).HasColumnName("TARGET_WEEK1");
        builder.Property(x => x.TargetWeek2).HasColumnName("TARGET_WEEK2");
        builder.Property(x => x.TargetWeek3).HasColumnName("TARGET_WEEK3");
        builder.Property(x => x.TargetWeek4).HasColumnName("TARGET_WEEK4");
        builder.Property(x => x.TargetWeek5).HasColumnName("TARGET_WEEK5");
        builder.Property(x => x.TargetMPlus1).HasColumnName("TARGET_MPLUS1");
        builder.Property(x => x.TargetMPlus2).HasColumnName("TARGET_MPLUS2");
        builder.Property(x => x.TargetMPlus3).HasColumnName("TARGET_MPLUS3");
        builder.Property(x => x.TargetMPlus4).HasColumnName("TARGET_MPLUS4");
        builder.Property(x => x.SciUserIdModified).HasColumnName("SCI_USER_ID_MODIFIED");
        builder.Property(x => x.ModifiedDate).HasColumnName("MODIFIED_DATE");
        builder.Ignore(x => x.DomainEvents);
    }
}
