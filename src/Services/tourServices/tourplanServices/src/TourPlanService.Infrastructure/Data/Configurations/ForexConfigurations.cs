using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourPlanService.Domain.Entities;

namespace TourPlanService.Infrastructure.Data.Configurations;

public sealed class ForexRequisitionConfiguration : IEntityTypeConfiguration<ForexRequisition>
{
    public void Configure(EntityTypeBuilder<ForexRequisition> builder)
    {
        builder.ToTable("TOURPLAN_FOREXMAIN");
        builder.HasKey(x => x.ForReqId);
        builder.Property(x => x.ForReqId).HasColumnName("FORREQ_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqTpId).HasColumnName("FORREQ_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqPassNo).HasColumnName("FORREQ_PASSNO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqPassName).HasColumnName("FORREQ_PASSNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqPassLocation).HasColumnName("FORREQ_PASSLOCATION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqPassExpDate).HasColumnName("FORREQ_PASSEXPDATE").IsRequired();
        builder.Property(x => x.ForReqDestination).HasColumnName("FORREQ_DESTINATION").HasMaxLength(255);
        builder.Property(x => x.ForReqStatus).HasColumnName("FORREQ_STATUS").HasMaxLength(255);
        builder.Property(x => x.ForReqDate).HasColumnName("FORREQ_DATE").HasMaxLength(255);
        builder.Property(x => x.ForReqLastModifiedBy).HasColumnName("FORREQ_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqLastModifiedOn).HasColumnName("FORREQ_LASTMODIFIEDON").IsRequired();
        builder.Property(x => x.ForReqReceivedOn).HasColumnName("FORREQ_RECEIVEDON");
        builder.Property(x => x.ForReqRefNo).HasColumnName("FORREQ_REFNO").HasMaxLength(255);
        builder.Property(x => x.ForReqTax1).HasColumnName("FORREQ_TAX1").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqTax2).HasColumnName("FORREQ_TAX2").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqTax3).HasColumnName("FORREQ_TAX3").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqTax4).HasColumnName("FORREQ_TAX4").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqTax5).HasColumnName("FORREQ_TAX5").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqVendorId).HasColumnName("FORREQ_VENDORID").HasMaxLength(255);
        builder.Property(x => x.ForReqCurrency).HasColumnName("FORREQ_CURRENCY").HasMaxLength(255);
        builder.Property(x => x.ForReqTotValue).HasColumnName("FORREQ_TOTVALUE").HasMaxLength(255);
        builder.Property(x => x.ForReqRecBy).HasColumnName("FORREQ_RECBY").HasMaxLength(255);
        builder.Property(x => x.ForReqType).HasColumnName("FORREQ_TYPE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqAdlRemarks).HasColumnName("FORREQ_ADLREMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqAdvRefNo).HasColumnName("FORREQ_ADVREFNO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForReqNetPay).HasColumnName("FORREQ_NETPAY").HasMaxLength(255);
        builder.Property(x => x.ForReqCurDenoAdj).HasColumnName("FORREQ_CURDENOADJ").HasMaxLength(255);
        builder.Property(x => x.ForReqEncashCertDate).HasColumnName("FORREQ_ENCASHCERTDATE");
        builder.Property(x => x.ForReqBasAmt).HasColumnName("FORREQ_BASAMT").HasMaxLength(255);
        builder.Property(x => x.ForReqCgstAmt).HasColumnName("FORREQ_CGSTAMT").HasMaxLength(255);
        builder.Property(x => x.ForReqSgstAmt).HasColumnName("FORREQ_SGSTAMT").HasMaxLength(255);
        builder.Property(x => x.ForReqIgstAmt).HasColumnName("FORREQ_IGSTAMT").HasMaxLength(255);
        builder.Property(x => x.ForReqCgstCharges).HasColumnName("FORREQ_CGSTCHARGES").HasMaxLength(255);
        builder.Property(x => x.ForReqSgstCharges).HasColumnName("FORREQ_SGSTCHARGES").HasMaxLength(255);
        builder.Property(x => x.ForReqIgstCharges).HasColumnName("FORREQ_IGSTCHARGES").HasMaxLength(255);
        builder.HasMany(x => x.Details).WithOne(x => x.ForexRequisition).HasForeignKey(x => x.ForexReqId);
        builder.HasMany(x => x.ChequeDetails).WithOne(x => x.ForexRequisition).HasForeignKey(x => x.ForexReqId);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class ForexDetailConfiguration : IEntityTypeConfiguration<ForexDetail>
{
    public void Configure(EntityTypeBuilder<ForexDetail> builder)
    {
        builder.ToTable("TOURPLAN_FOREXDET");
        builder.HasKey(x => x.ForexId);
        builder.Property(x => x.ForexId).HasColumnName("FOREX_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexReqId).HasColumnName("FOREX_REQID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexSrcValue).HasColumnName("FOREX_SRCVALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexCurrency).HasColumnName("FOREX_CURRENCY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexValue).HasColumnName("FOREX_VALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexExgRate).HasColumnName("FOREX_EXGRATE").HasMaxLength(255);
        builder.Property(x => x.ForexExgValue).HasColumnName("FOREX_EXGVALUE").HasMaxLength(255);
        builder.Property(x => x.ForexPayMode).HasColumnName("FOREX_PAYMODE").HasMaxLength(255);
        builder.Property(x => x.ForexReqCurVal).HasColumnName("FOREX_REQCURVAL").HasMaxLength(255);
        builder.Property(x => x.ForexReqCurRecd).HasColumnName("FOREX_REQCURRECD").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class ForexChequeDetailConfiguration : IEntityTypeConfiguration<ForexChequeDetail>
{
    public void Configure(EntityTypeBuilder<ForexChequeDetail> builder)
    {
        builder.ToTable("TOURPLAN_FOREXCHQDET");
        builder.HasKey(x => x.ForexChqDetId);
        builder.Property(x => x.ForexChqDetId).HasColumnName("FOREX_CHQDETID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexReqId).HasColumnName("FOREX_REQID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexChqNo).HasColumnName("FOREX_CHQNO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ForexChqDate).HasColumnName("FOREX_CHQDATE");
        builder.Property(x => x.ForexBankName).HasColumnName("FOREX_BANKNAME").HasMaxLength(200).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class DomesticDaBreakConfiguration : IEntityTypeConfiguration<DomesticDaBreak>
{
    public void Configure(EntityTypeBuilder<DomesticDaBreak> builder)
    {
        builder.ToTable("TRAVEL_DOMDABREAK");
        builder.HasKey(x => x.DomDaId);
        builder.Property(x => x.DomDaId).HasColumnName("DOMDA_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.DomDaTpId).HasColumnName("DOMDA_TPID").HasMaxLength(255);
        builder.Property(x => x.DomDaFromDate).HasColumnName("DOMDA_FROMDATE");
        builder.Property(x => x.DomDaToDate).HasColumnName("DOMDA_TODATE");
        builder.Property(x => x.DomDaDaDays).HasColumnName("DOMDA_DADAYS").HasMaxLength(255);
        builder.Property(x => x.DomDaDaEffDate).HasColumnName("DOMDA_DAEFFDATE");
        builder.Property(x => x.DomDaDaClsDate).HasColumnName("DOMDA_DACLSDATE");
        builder.Property(x => x.DomDaDaActualDays).HasColumnName("DOMDA_DAACTUALDAYS").HasMaxLength(255);
        builder.Property(x => x.DomDaDaRate).HasColumnName("DOMDA_DARATE").HasMaxLength(255);
        builder.Property(x => x.DomDaLeaveDays).HasColumnName("DOMDA_LEAVEDAYS").HasMaxLength(255);
        builder.Property(x => x.DomDaFoodExpDays).HasColumnName("DOMDA_FOODEXPDAYS").HasMaxLength(255);
        builder.Property(x => x.DomDaOwnStayTDays).HasColumnName("DOMDA_OWNSTAYTDAYS").HasMaxLength(255);
        builder.Property(x => x.DomDaFinalDays).HasColumnName("DOMDA_FINALDAYS").HasMaxLength(255);
        builder.Property(x => x.DomDaFinalValue).HasColumnName("DOMDA_FINALVALUE").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class ForeignExpenseMainConfiguration : IEntityTypeConfiguration<ForeignExpenseMain>
{
    public void Configure(EntityTypeBuilder<ForeignExpenseMain> builder)
    {
        builder.ToTable("TRAVEL_EXPENSEINTMAIN");
        builder.HasKey(x => x.TpExpMainId);
        builder.Property(x => x.TpExpMainId).HasColumnName("TPEXPMAIN_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainTpId).HasColumnName("TPEXPMAIN_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainClaimType).HasColumnName("TPEXPMAIN_CLAIMTYPE").HasMaxLength(255);
        builder.Property(x => x.TpExpMainLocCur).HasColumnName("TPEXPMAIN_LOCCUR").HasMaxLength(255);
        builder.Property(x => x.TpExpMainSetDate).HasColumnName("TPEXPMAIN_SETDATE").IsRequired();
        builder.Property(x => x.TpExpMainAppSetDate).HasColumnName("TPEXPMAIN_APPSETDATE").IsRequired();
        builder.Property(x => x.TpExpMainIntCur1).HasColumnName("TPEXPMAIN_INTCUR1").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainIntCur2).HasColumnName("TPEXPMAIN_INTCUR2").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainIntCnv1).HasColumnName("TPEXPMAIN_INTCNV1").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainIntCnv2).HasColumnName("TPEXPMAIN_INTCNV2").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainIntVal1).HasColumnName("TPEXPMAIN_INTVAL1").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainIntVal2).HasColumnName("TPEXPMAIN_INTVAL2").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpMainBalAmt).HasColumnName("TPEXPMAIN_BALAMT").HasMaxLength(255).IsRequired();
        builder.HasMany(x => x.Details).WithOne(x => x.ForeignExpenseMain).HasForeignKey(x => x.TpExpDetTpId);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class ForeignExpenseDetailConfiguration : IEntityTypeConfiguration<ForeignExpenseDetail>
{
    public void Configure(EntityTypeBuilder<ForeignExpenseDetail> builder)
    {
        builder.ToTable("TRAVEL_EXPENSEINTDET");
        builder.HasKey(x => x.TpExpDetId);
        builder.Property(x => x.TpExpDetId).HasColumnName("TPEXPDET_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetTpId).HasColumnName("TPEXPDET_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetGroupId).HasColumnName("TPEXPDET_GROUPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetCurrency).HasColumnName("TPEXPDET_CURRENCY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetValue).HasColumnName("TPEXPDET_VALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetActValue).HasColumnName("TPEXPDET_ACTVALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetAppAmt).HasColumnName("TPEXPDET_APPAMT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpDetExpFlag).HasColumnName("TPEXPDET_EXPFLAG").HasMaxLength(255).IsRequired();
        builder.HasMany(x => x.Breakups).WithOne(x => x.ForeignExpenseDetail).HasForeignKey(x => x.TpExpBrkDetId);
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class ForeignExpenseBreakupConfiguration : IEntityTypeConfiguration<ForeignExpenseBreakup>
{
    public void Configure(EntityTypeBuilder<ForeignExpenseBreakup> builder)
    {
        builder.ToTable("TRAVEL_EXPENSEINTBRK");
        builder.HasKey(x => x.TpExpBrkId);
        builder.Property(x => x.TpExpBrkId).HasColumnName("TPEXPBRK_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkDetId).HasColumnName("TPEXPBRK_DETID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkExpId).HasColumnName("TPEXPBRK_EXPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkDate).HasColumnName("TPEXPBRK_DATE");
        builder.Property(x => x.TpExpBrkRemarks).HasColumnName("TPEXPBRK_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkAmt).HasColumnName("TPEXPBRK_AMT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkActAmt).HasColumnName("TPEXPBRK_ACTAMT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkAppAmt).HasColumnName("TPEXPBRK_APPAMT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TpExpBrkPayMode).HasColumnName("TPEXPBRK_PAYMODE").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}
