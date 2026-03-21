using ConfigService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigService.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> b)
    {
        b.ToTable("VENDOR_MASTER");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("VENDOR_ID").HasMaxLength(255);
        b.Property(e => e.VendorName).HasColumnName("VENDOR_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.ActiveStatus).HasColumnName("VENDOR_ACTIVE").HasMaxLength(255).IsRequired();
        b.Property(e => e.VendorCode).HasColumnName("VENDOR_CODE").HasMaxLength(255).IsRequired();
        b.Property(e => e.ContactPerson).HasColumnName("VENDOR_CONTACTPERSON").HasMaxLength(255).IsRequired();
        b.Property(e => e.Address1).HasColumnName("VENDOR_ADDRESS1").HasMaxLength(255).IsRequired();
        b.Property(e => e.Address2).HasColumnName("VENDOR_ADDRESS2").HasMaxLength(255).IsRequired();
        b.Property(e => e.Address3).HasColumnName("VENDOR_ADDRESS3").HasMaxLength(255).IsRequired();
        b.Property(e => e.Address4).HasColumnName("VENDOR_ADDRESS4").HasMaxLength(255).IsRequired();
        b.Property(e => e.PinCode).HasColumnName("VENDOR_PINCODE").HasMaxLength(255).IsRequired();
        b.Property(e => e.EmailId).HasColumnName("VENDOR_EMAILID").HasMaxLength(255).IsRequired();
        b.Property(e => e.CcEmailId).HasColumnName("VENDOR_CCEMAILID").HasMaxLength(255).IsRequired();
        b.Property(e => e.SrfTriggerId).HasColumnName("VENDOR_SRFTRIGGERID").HasMaxLength(255).IsRequired();
        b.Property(e => e.MobileNo).HasColumnName("VENDOR_MOBILENO").HasMaxLength(255).IsRequired();
        b.Property(e => e.PhoneNos).HasColumnName("VENDOR_PHONENOS").HasMaxLength(255).IsRequired();
        b.Property(e => e.VendorType).HasColumnName("VENDOR_TYPE").HasMaxLength(255).IsRequired();
        b.Property(e => e.SubType).HasColumnName("VENDOR_SUBTYPE").HasMaxLength(255).IsRequired();
        b.Property(e => e.DirectMail).HasColumnName("VENDOR_DIRECTMAIL").HasMaxLength(255);
        b.Property(e => e.UserId).HasColumnName("VENDOR_USERID").HasMaxLength(255);
        b.Property(e => e.GstNo).HasColumnName("VENDOR_GSTNO").HasMaxLength(255);
        b.HasMany(e => e.TaxRates).WithOne().HasForeignKey(t => t.VendorId);
        b.HasMany(e => e.UnitMaps).WithOne().HasForeignKey(t => t.VendorId);
        b.HasMany(e => e.Charges).WithOne().HasForeignKey(t => t.VendorId);
        b.Ignore(e => e.DomainEvents);
    }
}

public class VendorTaxRateConfiguration : IEntityTypeConfiguration<VendorTaxRate>
{
    public void Configure(EntityTypeBuilder<VendorTaxRate> b)
    {
        b.ToTable("VENDOR_TAXRATE");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("VENDOR_TAXID").HasMaxLength(255);
        b.Property(e => e.VendorId).HasColumnName("VENDOR_ID").HasMaxLength(255);
        b.Property(e => e.TaxNature).HasColumnName("VENDOR_TAXNATURE").HasMaxLength(255).IsRequired();
        b.Property(e => e.TaxRate).HasColumnName("VENDOR_TAXRATE").HasMaxLength(255).IsRequired();
        b.Property(e => e.EffectiveDate).HasColumnName("VENDOR_TAXEFFDATE");
        b.Property(e => e.ClosureDate).HasColumnName("VENDOR_TAXCLSDATE");
        b.Property(e => e.EnteredBy).HasColumnName("VENDOR_ENTBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.EnteredOn).HasColumnName("VENDOR_ENTON");
    }
}

public class VendorUnitMapConfiguration : IEntityTypeConfiguration<VendorUnitMap>
{
    public void Configure(EntityTypeBuilder<VendorUnitMap> b)
    {
        b.ToTable("VENDOR_UNITMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("VENDOR_UNITMAPID").HasMaxLength(255);
        b.Property(e => e.VendorId).HasColumnName("VENDOR_ID").HasMaxLength(255).IsRequired();
        b.Property(e => e.PayUnitId).HasColumnName("VENDOR_PAYUNITID").HasMaxLength(255).IsRequired();
        b.Property(e => e.OracleSiteId).HasColumnName("VENDOR_ORASITEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.TermId).HasColumnName("VENDOR_TERMID").HasMaxLength(255).IsRequired();
    }
}

public class VendorChargesConfiguration : IEntityTypeConfiguration<VendorCharges>
{
    public void Configure(EntityTypeBuilder<VendorCharges> b)
    {
        b.ToTable("VENDOR_CHARGES");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("VENDOR_CHARGESID").HasMaxLength(255);
        b.Property(e => e.VendorId).HasColumnName("VENDOR_ID").HasMaxLength(255);
        b.Property(e => e.Rate).HasColumnName("VENDOR_RATE").HasMaxLength(255);
        b.Property(e => e.EffectiveDate).HasColumnName("VENDOR_TAXEFFDATE");
        b.Property(e => e.ClosureDate).HasColumnName("VENDOR_TAXCLSDATE");
        b.Property(e => e.EnteredBy).HasColumnName("VENDOR_ENTBY").HasMaxLength(255);
        b.Property(e => e.EnteredOn).HasColumnName("VENDOR_ENTON");
    }
}

public class GradeCatExpenseRuleConfiguration : IEntityTypeConfiguration<GradeCatExpenseRule>
{
    public void Configure(EntityTypeBuilder<GradeCatExpenseRule> b)
    {
        b.ToTable("GRADECAT_EXPRULE");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("EXPRULE_ID").HasMaxLength(255);
        b.Property(e => e.GradeCategory).HasColumnName("EXPRULE_GRADECAT").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToUnit).HasColumnName("EXPRULE_APPLYTOUNIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.UnitId).HasColumnName("EXPRULE_UNITID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToGrade).HasColumnName("EXPRULE_APPLYTOGRADE").HasMaxLength(255).IsRequired();
        b.Property(e => e.GradeId).HasColumnName("EXPRULE_GRADEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ExpenseType).HasColumnName("EXPRULE_EXPTYPE").HasMaxLength(255).IsRequired();
        b.Property(e => e.Limit).HasColumnName("EXPRULE_LIMIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.DayLimit).HasColumnName("EXPRULE_DAYLIMIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.BrokenFlag).HasColumnName("EXPRULE_BROKENFLAG").HasMaxLength(255).IsRequired();
        b.Property(e => e.RuleType).HasColumnName("EXPRULE_YPE").HasMaxLength(3);
        b.HasMany(e => e.Breaks).WithOne().HasForeignKey(r => r.RuleId);
    }
}

public class GradeCatExpenseRuleBreakConfiguration : IEntityTypeConfiguration<GradeCatExpenseRuleBreak>
{
    public void Configure(EntityTypeBuilder<GradeCatExpenseRuleBreak> b)
    {
        b.ToTable("GRADECAT_EXPRULEBRK");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("EXPRULE_BRKID").HasMaxLength(255);
        b.Property(e => e.RuleId).HasColumnName("EXPRULE_ID").HasMaxLength(255).IsRequired();
        b.Property(e => e.FromHours).HasColumnName("EXPRULE_FROMHRS").HasMaxLength(255).IsRequired();
        b.Property(e => e.ToHours).HasColumnName("EXPRULE_TOHRS").HasMaxLength(255).IsRequired();
        b.Property(e => e.Amount).HasColumnName("EXPRULE_AMT").HasMaxLength(255).IsRequired();
    }
}

public class GradeCatModeMapConfiguration : IEntityTypeConfiguration<GradeCatModeMap>
{
    public void Configure(EntityTypeBuilder<GradeCatModeMap> b)
    {
        b.ToTable("GRADECAT_MODEMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("MODEMAP_ID").HasMaxLength(255);
        b.Property(e => e.GradeCategory).HasColumnName("MODEMAP_GRADECAT").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToUnit).HasColumnName("MODEMAP_APPLYTOUNIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.UnitId).HasColumnName("MODEMAP_UNITID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToGrade).HasColumnName("MODEMAP_APPLYTOGRADE").HasMaxLength(255).IsRequired();
        b.Property(e => e.GradeId).HasColumnName("MODEMAP_GRADEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ModeId).HasColumnName("MODEMAP_MODEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ClassId).HasColumnName("MODEMAP_CLASSID").HasMaxLength(255).IsRequired();
        b.Property(e => e.SpecialStatus).HasColumnName("MODEMAP_SPECIALSTATUS").HasMaxLength(255).IsRequired();
    }
}

public class GradeCatStayRuleConfiguration : IEntityTypeConfiguration<GradeCatStayRule>
{
    public void Configure(EntityTypeBuilder<GradeCatStayRule> b)
    {
        b.ToTable("GRADECAT_STAYRULE");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("STAYRULE_ID").HasMaxLength(255);
        b.Property(e => e.GradeCategory).HasColumnName("STAYRULE_GRADECAT").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToUnit).HasColumnName("STAYRULE_APPLYTOUNIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.UnitId).HasColumnName("STAYRULE_UNITID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToGrade).HasColumnName("STAYRULE_APPLYTOGRADE").HasMaxLength(255).IsRequired();
        b.Property(e => e.GradeId).HasColumnName("STAYRULE_GRADEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.TravelType).HasColumnName("STAYRULE_TRAVELTYPE").HasMaxLength(255).IsRequired();
        b.Property(e => e.StayType).HasColumnName("STAYRULE_TYPE").HasMaxLength(255).IsRequired();
        b.Property(e => e.CityClassId).HasColumnName("STAYRULE_CITYCLASSID").HasMaxLength(255).IsRequired();
        b.Property(e => e.Limit).HasColumnName("STAYRULE_LIMIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.BookCharges).HasColumnName("STAYRULE_BOOKCHARGES").HasMaxLength(255).IsRequired();
        b.Property(e => e.NightStayValue).HasColumnName("STAYRULE_NIGHTSTAYVAL").HasMaxLength(255).IsRequired();
        b.Property(e => e.IncidentalExpenses).HasColumnName("STAYRULE_INCEXP").HasMaxLength(255).IsRequired();
    }
}

public class GradeCatExpenseMapConfiguration : IEntityTypeConfiguration<GradeCatExpenseMap>
{
    public void Configure(EntityTypeBuilder<GradeCatExpenseMap> b)
    {
        b.ToTable("GRADECATEXP_MAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("EXPMAP_ID").HasMaxLength(255);
        b.Property(e => e.GradeCategory).HasColumnName("EXPMAP_GRADECAT").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToUnit).HasColumnName("EXPMAP_APPLYTOUNIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.UnitId).HasColumnName("EXPMAP_UNITID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToGrade).HasColumnName("EXPMAP_APPLYTOGRADE").HasMaxLength(255).IsRequired();
        b.Property(e => e.GradeId).HasColumnName("EXPMAP_GRADEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ExpenseId).HasColumnName("EXPMAP_EXPID").HasMaxLength(255).IsRequired();
    }
}

public class GradeTypeTravelParamConfiguration : IEntityTypeConfiguration<GradeTypeTravelParam>
{
    public void Configure(EntityTypeBuilder<GradeTypeTravelParam> b)
    {
        b.ToTable("GRADETYPETRAVEL_PARAMS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("PARAM_ID").HasMaxLength(255);
        b.Property(e => e.GradeCategory).HasColumnName("PARAM_GRADECAT").HasMaxLength(255).IsRequired();
        b.Property(e => e.ApplyToUnit).HasColumnName("PARAM_APPLYTOUNIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.UnitId).HasColumnName("PARAM_UNITID").HasMaxLength(255).IsRequired();
        b.Property(e => e.AdvanceEligible).HasColumnName("PARAM_ADVANCEELG").HasMaxLength(255).IsRequired();
        b.Property(e => e.AdvanceLimit).HasColumnName("PARAM_ADVANCELIMIT").HasMaxLength(255).IsRequired();
        b.Property(e => e.AdvanceDays).HasColumnName("PARAM_ADVANCEDAYS").HasMaxLength(255).IsRequired();
        b.Property(e => e.AdvanceNos).HasColumnName("PARAM_ADVANCENOS").HasMaxLength(255).IsRequired();
        b.Property(e => e.AdvanceOut).HasColumnName("PARAM_ADVANCEOUT").HasMaxLength(255).IsRequired();
        b.Property(e => e.TpApproval).HasColumnName("PARAM_TPAPPROVAL").HasMaxLength(255).IsRequired();
        b.Property(e => e.SetTimeLimit).HasColumnName("PARAM_SETTIMELIMIT").HasMaxLength(255).IsRequired();
    }
}
