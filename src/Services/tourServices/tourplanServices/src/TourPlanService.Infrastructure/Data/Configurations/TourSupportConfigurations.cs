using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourPlanService.Domain.Entities;

namespace TourPlanService.Infrastructure.Data.Configurations;

public sealed class TourAdvanceConfiguration : IEntityTypeConfiguration<TourAdvance>
{
    public void Configure(EntityTypeBuilder<TourAdvance> builder)
    {
        builder.ToTable("TOURPLAN_ADVANCE");
        builder.HasKey(x => x.AdvId);
        builder.Property(x => x.AdvId).HasColumnName("ADV_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvTpId).HasColumnName("ADV_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvAmount).HasColumnName("ADV_AMOUNT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvJvId).HasColumnName("ADV_JVID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvRemarks).HasColumnName("ADV_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvAppStatus).HasColumnName("ADV_APPSTATUS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvAppBy).HasColumnName("ADV_APPBY").HasMaxLength(255);
        builder.Property(x => x.AdvAppOn).HasColumnName("ADV_APPON");
        builder.Property(x => x.AdvCurrency).HasColumnName("ADV_CURRENCY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvRate).HasColumnName("ADV_RATE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvTotal).HasColumnName("ADV_TOTAL").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvModifiedOn).HasColumnName("ADV_MODIFIEDON").IsRequired();
        builder.Property(x => x.AdvAppRemarks).HasColumnName("ADV_APPREMARKS").HasMaxLength(200);
        builder.Property(x => x.AdvFinRemarks).HasColumnName("ADV_FINREMARKS").HasMaxLength(200);
        builder.Property(x => x.AdvType).HasColumnName("ADV_TYPE").HasMaxLength(1);
        builder.Property(x => x.AdvPayMode).HasColumnName("ADV_PAYMODE").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class TourAgendaConfiguration : IEntityTypeConfiguration<TourAgenda>
{
    public void Configure(EntityTypeBuilder<TourAgenda> builder)
    {
        builder.ToTable("TOURPLAN_AGENDA");
        builder.HasKey(x => x.AgendaId);
        builder.Property(x => x.AgendaId).HasColumnName("AGENDA_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AgendaTpId).HasColumnName("AGENDA_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AgendaCity).HasColumnName("AGENDA_CITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AgendaMeet).HasColumnName("AGENDA_MEET").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AgendaOutcome).HasColumnName("AGENDA_OUTCOME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AgendaType).HasColumnName("AGENDA_TYPE");
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class TourCostCentreConfiguration : IEntityTypeConfiguration<TourCostCentre>
{
    public void Configure(EntityTypeBuilder<TourCostCentre> builder)
    {
        builder.ToTable("TOURPLAN_COSTCENTRE");
        builder.HasKey(x => x.TpCostId);
        builder.Property(x => x.TpCostId).HasColumnName("TPCOST_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostTpId).HasColumnName("TPCOST_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostBuCode).HasColumnName("TPCOST_BUCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostCcCode).HasColumnName("TPCOST_CCCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostSubAccCode).HasColumnName("TPCOST_SUBACCCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostProductCode).HasColumnName("TPCOST_PRODUCTCODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostLocSegment).HasColumnName("TPCOST_LOCSEGMENT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostAlllPer).HasColumnName("TPCOST_ALLLPER").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpCostDefault).HasColumnName("TPCOST_DEFAULT").HasMaxLength(1);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class TourDaBreakConfiguration : IEntityTypeConfiguration<TourDaBreak>
{
    public void Configure(EntityTypeBuilder<TourDaBreak> builder)
    {
        builder.ToTable("TOURPLAN_DABREAK");
        builder.HasKey(x => x.TpDaId);
        builder.Property(x => x.TpDaId).HasColumnName("TPDA_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpDaTpId).HasColumnName("TPDA_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpDaCountryId).HasColumnName("TPDA_COUNTRYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpDaCurrency).HasColumnName("TPDA_CURRENCY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpDaDays).HasColumnName("TPDA_DAYS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpDaRate).HasColumnName("TPDA_RATE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpDaGhDays).HasColumnName("TPDA_GHDAYS").HasMaxLength(255);
        builder.Property(x => x.TpDaGhRate).HasColumnName("TPDA_GHRATE").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class TourExpenseConfiguration : IEntityTypeConfiguration<TourExpense>
{
    public void Configure(EntityTypeBuilder<TourExpense> builder)
    {
        builder.ToTable("TOURPLAN_EXPENSE");
        builder.HasKey(x => x.TpExpId);
        builder.Property(x => x.TpExpId).HasColumnName("TPEXP_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpTpId).HasColumnName("TPEXP_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpExpId).HasColumnName("TPEXP_EXPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpCur).HasColumnName("TPEXP_CUR").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpExpAmt).HasColumnName("TPEXP_EXPAMT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpRemarks).HasColumnName("TPEXP_REMARKS").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}
