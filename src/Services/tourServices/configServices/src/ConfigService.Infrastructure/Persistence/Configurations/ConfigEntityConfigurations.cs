using ConfigService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigService.Infrastructure.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> b)
    {
        b.ToTable("CURRENCY_MASTER");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CURRENCY_ID");
        b.Property(e => e.CurrencyCode).HasColumnName("CURRENCY_CODE").HasMaxLength(3).IsRequired();
        b.Property(e => e.CurrencyName).HasColumnName("CURRENCY_NAME").HasMaxLength(65);
        b.Property(e => e.CurrencySymbol).HasColumnName("CURRENCY_SYMBOL").HasMaxLength(10);
    }
}

public class ExpenseCurrencyConfiguration : IEntityTypeConfiguration<ExpenseCurrency>
{
    public void Configure(EntityTypeBuilder<ExpenseCurrency> b)
    {
        b.ToTable("EXPCUR_MAST");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CUR_CODE").HasMaxLength(3);
        b.Property(e => e.CurrencyName).HasColumnName("CUR_NAME").HasMaxLength(65).IsRequired();
        b.Property(e => e.CurrencyShortName).HasColumnName("CUR_SHTNAME").HasMaxLength(5).IsRequired();
        b.Property(e => e.CurrencySymbol).HasColumnName("CUR_SYMBOL").HasMaxLength(5).IsRequired();
    }
}

public class ExpenseGroupConfiguration : IEntityTypeConfiguration<ExpenseGroup>
{
    public void Configure(EntityTypeBuilder<ExpenseGroup> b)
    {
        b.ToTable("EXPENSEGROUP_MAST");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("EXPGROUP_ID").HasMaxLength(255);
        b.Property(e => e.GroupName).HasColumnName("EXPGROUP_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.TravelType).HasColumnName("EXPGROUP_TRAVELTYPE").HasMaxLength(255).IsRequired();
        b.Property(e => e.BreakFlag).HasColumnName("EXPGROUP_BREAKFLAG").HasMaxLength(255).IsRequired();
        b.HasMany(e => e.Mappings).WithOne().HasForeignKey(m => m.GroupId);
    }
}

public class ExpenseGroupMapConfiguration : IEntityTypeConfiguration<ExpenseGroupMap>
{
    public void Configure(EntityTypeBuilder<ExpenseGroupMap> b)
    {
        b.ToTable("EXPENSEGROUP_MAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("EXPGROUPMAP_ID").HasMaxLength(255);
        b.Property(e => e.GroupId).HasColumnName("EXPGROUPMAP_GROUPID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ExpenseId).HasColumnName("EXPGROUPMAP_EXPENSEID").HasMaxLength(255).IsRequired();
    }
}

public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
{
    public void Configure(EntityTypeBuilder<ExpenseType> b)
    {
        b.ToTable("EXPENSETYPE_MAST");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("EXPENSE_ID");
        b.Property(e => e.ExpenseName).HasColumnName("EXPENSE_NAME").HasMaxLength(75).IsRequired();
        b.Property(e => e.ExpenseCategoryId).HasColumnName("EXPENSE_CATID").IsRequired();
        b.Property(e => e.TravelType).HasColumnName("EXPENSE_TRAVELTYPE").HasMaxLength(3).IsRequired();
        b.Property(e => e.SortNo).HasColumnName("EXPENSE_SORTNO").IsRequired();
    }
}

public class GlobalPayParamConfiguration : IEntityTypeConfiguration<GlobalPayParam>
{
    public void Configure(EntityTypeBuilder<GlobalPayParam> b)
    {
        b.ToTable("GLOBALPAY_PARAMS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("PARAM_ID").HasMaxLength(255);
        b.Property(e => e.ParamCode).HasColumnName("PARAM_CODE").HasMaxLength(255).IsRequired();
        b.Property(e => e.ParamDescription).HasColumnName("PARAM_DESC").HasMaxLength(255).IsRequired();
        b.Property(e => e.ParamValue).HasColumnName("PARAM_VALUE").HasMaxLength(255).IsRequired();
    }
}

public class CalendarGstBuMapConfiguration : IEntityTypeConfiguration<CalendarGstBuMap>
{
    public void Configure(EntityTypeBuilder<CalendarGstBuMap> b)
    {
        b.ToTable("CALENDAR_GSTBUMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CALENDAR_ID");
        b.Property(e => e.CalendarName).HasColumnName("CALENDAR_NAME").HasMaxLength(100).IsRequired();
        b.Property(e => e.R12Bu).HasColumnName("CALENDAR_R12BU").HasMaxLength(25);
    }
}
