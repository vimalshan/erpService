using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using travelTransactionService.Domain.Entities;

namespace travelTransactionService.Infrastructure.Data.Configurations;

public class VendorMasterConfiguration : IEntityTypeConfiguration<VendorMaster>
{
    public void Configure(EntityTypeBuilder<VendorMaster> builder)
    {
        builder.ToTable("VENDOR_MASTER");
        builder.HasKey(e => e.VendorId);

        builder.Property(e => e.VendorId).HasColumnName("VM_ID").HasColumnType("bigint").ValueGeneratedNever();
        builder.Property(e => e.Name).HasColumnName("VM_NAME").HasMaxLength(65).IsRequired();
        builder.Property(e => e.AddressLine1).HasColumnName("VM_ADD_LN1").HasMaxLength(30);
        builder.Property(e => e.AddressLine2).HasColumnName("VM_ADD_LN2").HasMaxLength(30);
        builder.Property(e => e.AddressLine3).HasColumnName("VM_ADD_LIN3").HasMaxLength(30);
        builder.Property(e => e.AddressLine4).HasColumnName("VM_ADD_LN4").HasMaxLength(30);
        builder.Property(e => e.AddressLine5).HasColumnName("VM_ADD_LN5").HasMaxLength(30);
        builder.Property(e => e.CityCode).HasColumnName("VM_CIT_COD");
        builder.Property(e => e.ItPanNumber).HasColumnName("VM_IT_PAN").HasColumnType("char(10)");
        builder.Property(e => e.PhoneNumber).HasColumnName("VM_PHN_NO").HasMaxLength(20);
        builder.Property(e => e.AccountNumber).HasColumnName("VM_ACC_NO").HasMaxLength(20);
        builder.Property(e => e.BankName).HasColumnName("VM_BNK_NAM").HasMaxLength(65);
        builder.Property(e => e.CategoryType).HasColumnName("VM_CAT_TYPE").HasColumnType("char(1)").IsRequired();

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);
    }
}

public class AccountMasterConfiguration : IEntityTypeConfiguration<AccountMaster>
{
    public void Configure(EntityTypeBuilder<AccountMaster> builder)
    {
        builder.ToTable("ACC_MASTER");
        builder.HasNoKey();

        builder.Property(e => e.CompanyCode).HasColumnName("AC_COM_COD").HasColumnType("char(3)");
        builder.Property(e => e.EdCode).HasColumnName("AC_ED_COD").HasColumnType("char(6)");
        builder.Property(e => e.AccountCode).HasColumnName("AC_ACC_COD").HasColumnType("char(6)");
        builder.Property(e => e.GradeType).HasColumnName("AC_GRD_TYP").HasColumnType("char(3)");
        builder.Property(e => e.DebitCreditFlag).HasColumnName("AC_DC_FLG").HasColumnType("char(1)");
        builder.Property(e => e.SubCode).HasColumnName("AC_SUB_COD").HasColumnType("char(6)");
        builder.Property(e => e.AccountDescription).HasColumnName("AC_ACC_DES").HasMaxLength(200);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class GlCodeCombinationConfiguration : IEntityTypeConfiguration<GlCodeCombination>
{
    public void Configure(EntityTypeBuilder<GlCodeCombination> builder)
    {
        builder.ToTable("GL_CODE_COMBINATIONS_KFV");
        builder.HasKey(e => e.RowId);

        builder.Property(e => e.RowId).HasColumnName("ROW_ID");
        builder.Property(e => e.CodeCombinationId).HasColumnName("CODE_COMBINATION_ID");
        builder.Property(e => e.ChartOfAccountsId).HasColumnName("CHART_OF_ACCOUNTS_ID");
        builder.Property(e => e.ConcatenatedSegments).HasColumnName("CONCATENATED_SEGMENTS").HasMaxLength(207);
        builder.Property(e => e.PaddedConcatenatedSegments).HasColumnName("PADDED_CONCATENATED_SEGMENTS").HasMaxLength(26);
        builder.Property(e => e.GlAccountType).HasColumnName("GL_ACCOUNT_TYPE").HasMaxLength(1).IsRequired();
        builder.Property(e => e.DetailBudgetingAllowed).HasColumnName("DETAIL_BUDGETING_ALLOWED").HasMaxLength(1).IsRequired();
        builder.Property(e => e.DetailPostingAllowed).HasColumnName("DETAIL_POSTING_ALLOWED").HasMaxLength(1).IsRequired();
        builder.Property(e => e.EnabledFlag).HasColumnName("ENABLED_FLAG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.SummaryFlag).HasColumnName("SUMMARY_FLAG").HasMaxLength(1).IsRequired();
        builder.Property(e => e.Segment1).HasColumnName("SEGMENT1").HasMaxLength(25);
        builder.Property(e => e.Segment2).HasColumnName("SEGMENT2").HasMaxLength(25);
        builder.Property(e => e.Segment3).HasColumnName("SEGMENT3").HasMaxLength(25);
        builder.Property(e => e.Segment4).HasColumnName("SEGMENT4").HasMaxLength(25);
        builder.Property(e => e.Segment5).HasColumnName("SEGMENT5").HasMaxLength(25);
        builder.Property(e => e.Segment6).HasColumnName("SEGMENT6").HasMaxLength(25);
        builder.Property(e => e.Segment7).HasColumnName("SEGMENT7").HasMaxLength(25);
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(240);
        builder.Property(e => e.StartDateActive).HasColumnName("START_DATE_ACTIVE").HasColumnType("datetime2(3)");
        builder.Property(e => e.EndDateActive).HasColumnName("END_DATE_ACTIVE").HasColumnType("datetime2(3)");
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY").HasColumnType("decimal(38)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class TaxMasterConfiguration : IEntityTypeConfiguration<TaxMaster>
{
    public void Configure(EntityTypeBuilder<TaxMaster> builder)
    {
        builder.ToTable("TAX_MASTER");
        builder.HasKey(e => e.TaxType);

        builder.Property(e => e.TaxVendorId).HasColumnName("TAX_VENDORID");
        builder.Property(e => e.TaxType).HasColumnName("TAX_TYPE").HasColumnType("char(5)");
        builder.Property(e => e.TaxRate).HasColumnName("TAX_RATE").HasColumnType("decimal(19,0)");
        builder.Property(e => e.TaxEffectiveDate).HasColumnName("TAX_EFFDAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.TaxCloseDate).HasColumnName("TAX_CLSDAT").HasColumnType("datetime2(3)");
        builder.Property(e => e.ModifiedBy).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedOn).HasColumnName("MODIFIED_ON").HasColumnType("datetime2(3)");

        // TaxComponent is keyless – cannot be a navigation target
        builder.Ignore(e => e.Components);

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);
    }
}

public class TaxComponentConfiguration : IEntityTypeConfiguration<TaxComponent>
{
    public void Configure(EntityTypeBuilder<TaxComponent> builder)
    {
        builder.ToTable("TAX_COMPONENT");
        builder.HasNoKey();

        builder.Property(e => e.VendorCode).HasColumnName("VENDORCODE");
        builder.Property(e => e.Component).HasColumnName("COMPONENT").HasMaxLength(50);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class JvInterfaceConfiguration : IEntityTypeConfiguration<JvInterface>
{
    public void Configure(EntityTypeBuilder<JvInterface> builder)
    {
        builder.ToTable("JV_INTERFACE");
        builder.HasNoKey();

        builder.Property(e => e.CodeCombination).HasColumnName("CODE_COMBINATION").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Segment1).HasColumnName("SEGMENT1").HasMaxLength(2);
        builder.Property(e => e.Io).HasColumnName("IO").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Unit).HasColumnName("UNIT").HasColumnType("char(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class JvMissingCombiCodeConfiguration : IEntityTypeConfiguration<JvMissingCombiCode>
{
    public void Configure(EntityTypeBuilder<JvMissingCombiCode> builder)
    {
        builder.ToTable("JV_MISSING_COMBICODE");
        builder.HasNoKey();

        builder.Property(e => e.AgencyName).HasColumnName("AM_AGN_NAM").HasMaxLength(20);
        builder.Property(e => e.InvoiceNumber).HasColumnName("INVOICE_NUM").HasMaxLength(4000);
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(4000);
        builder.Property(e => e.DistCodeConcatenated).HasColumnName("DIST_CODE_CONCENATED").HasMaxLength(4000);
        builder.Property(e => e.JvNumber).HasColumnName("JV_NO");
        builder.Property(e => e.LogSysId).HasColumnName("LOG_SYSID");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class JaiInterfaceLineConfiguration : IEntityTypeConfiguration<JaiInterfaceLine>
{
    public void Configure(EntityTypeBuilder<JaiInterfaceLine> builder)
    {
        builder.ToTable("JAI_INTERFACE_LINES_ALL");
        builder.HasKey(e => e.InterfaceLineId);

        builder.Property(e => e.InterfaceLineId).HasColumnName("INTERFACE_LINE_ID").HasColumnType("decimal(38)").ValueGeneratedOnAdd();
        builder.Property(e => e.OrgId).HasColumnName("ORG_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.LocationId).HasColumnName("LOCATION_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.PartyId).HasColumnName("PARTY_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.PartySiteId).HasColumnName("PARTY_SITE_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.ImportModule).HasColumnName("IMPORT_MODULE").HasMaxLength(255).IsRequired();
        builder.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.TransactionNum).HasColumnName("TRANSACTION_NUM").HasMaxLength(240).IsRequired();
        builder.Property(e => e.TransactionLineNum).HasColumnName("TRANSACTION_LINE_NUM").HasColumnType("decimal(38)");
        builder.Property(e => e.ErrorFlag).HasColumnName("ERROR_FLAG").HasMaxLength(1);
        builder.Property(e => e.BatchSourceName).HasColumnName("BATCH_SOURCE_NAME").HasMaxLength(240);
        builder.Property(e => e.TaxableBasis).HasColumnName("TAXABLE_BASIS").HasMaxLength(20);
        builder.Property(e => e.TaxableEvent).HasColumnName("TAXABLE_EVENT").HasMaxLength(20);
        builder.Property(e => e.InclusiveTaxAmount).HasColumnName("INCLUSIVE_TAX_AMOUNT").HasMaxLength(255);
        builder.Property(e => e.ExclusiveTaxAmount).HasColumnName("EXCLUSIVE_TAX_AMOUNT").HasMaxLength(255);
        builder.Property(e => e.CreationDate).HasColumnName("CREATION_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").HasColumnType("decimal(38)");
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY").HasColumnType("decimal(38)");
        builder.Property(e => e.ImportStatus).HasColumnName("IMPORT_STATUS").HasMaxLength(30);
        builder.Property(e => e.HsnCode).HasColumnName("HSN_CODE").HasMaxLength(3);
        builder.Property(e => e.SacCode).HasColumnName("SAC_CODE").HasMaxLength(30);
        builder.Property(e => e.BatchId).HasColumnName("BATCHID").HasColumnType("decimal(19,0)");
        builder.Property(e => e.InvoiceId).HasColumnName("INVOICEID").HasColumnType("decimal(19,0)");
        builder.Property(e => e.LineNumber).HasColumnName("LINE_NUMBER").HasColumnType("decimal(19,0)");
        builder.Property(e => e.BatchBu).HasColumnName("BATCH_BU").HasMaxLength(25);
        builder.Property(e => e.Type).HasColumnName("TYPE").HasMaxLength(255);
        builder.Property(e => e.TypeTour).HasColumnName("TYPE_TOUR").HasMaxLength(255);
        builder.Property(e => e.TravelClass).HasColumnName("TRV_CLASS");
        builder.Property(e => e.SgstAmount).HasColumnName("SGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.CgstAmount).HasColumnName("CGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.IgstAmount).HasColumnName("IGSTAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.JvNumber).HasColumnName("JV_NO");
        builder.Property(e => e.AgencyId).HasColumnName("JAI_AGENCY_ID");
        builder.Property(e => e.CombinationId).HasColumnName("COMBINATION_ID");

        // Keyless entities cannot be principal in a relationship – ignore navigation
        builder.Ignore(e => e.TaxLines);

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);
    }
}

public class JaiInterfaceTaxLineConfiguration : IEntityTypeConfiguration<JaiInterfaceTaxLine>
{
    public void Configure(EntityTypeBuilder<JaiInterfaceTaxLine> builder)
    {
        builder.ToTable("JAI_INTERFACE_TAX_LINES_ALL");
        builder.HasNoKey();

        builder.Property(e => e.InterfaceTaxLineId).HasColumnName("INTERFACE_TAX_LINE_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.InterfaceLineId).HasColumnName("INTERFACE_LINE_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.PartyId).HasColumnName("PARTY_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.PartySiteId).HasColumnName("PARTY_SITE_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.ImportModule).HasColumnName("IMPORT_MODULE").HasMaxLength(10).IsRequired();
        builder.Property(e => e.TransactionNum).HasColumnName("TRANSACTION_NUM").HasMaxLength(240).IsRequired();
        builder.Property(e => e.TransactionLineNum).HasColumnName("TRANSACTION_LINE_NUM").HasColumnType("decimal(38)");
        builder.Property(e => e.TaxLineNo).HasColumnName("TAX_LINE_NO");
        builder.Property(e => e.ExternalTaxCode).HasColumnName("EXTERNAL_TAX_CODE").HasMaxLength(255);
        builder.Property(e => e.TaxId).HasColumnName("TAX_ID");
        builder.Property(e => e.TaxRate).HasColumnName("TAX_RATE").HasColumnType("decimal(38)");
        builder.Property(e => e.TaxAmount).HasColumnName("TAX_AMOUNT").HasColumnType("decimal(38)");
        builder.Property(e => e.FuncTaxAmount).HasColumnName("FUNC_TAX_AMOUNT").HasColumnType("decimal(38)");
        builder.Property(e => e.BaseTaxAmount).HasColumnName("BASE_TAX_AMOUNT").HasColumnType("decimal(38)");
        builder.Property(e => e.InclusiveTaxFlag).HasColumnName("INCLUSIVE_TAX_FLAG").HasMaxLength(255);
        builder.Property(e => e.CodeCombinationId).HasColumnName("CODE_COMBINATION_ID");
        builder.Property(e => e.CreationDate).HasColumnName("CREATION_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY").HasColumnType("decimal(38)");
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY").HasColumnType("decimal(38)");
        builder.Property(e => e.JvNumber).HasColumnName("JV_NO");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class BatchSubBreakupConfiguration : IEntityTypeConfiguration<BatchSubBreakup>
{
    public void Configure(EntityTypeBuilder<BatchSubBreakup> builder)
    {
        builder.ToTable("TRAVEL_BATCH_SUB_BREAKUP");
        builder.HasNoKey();

        builder.Property(e => e.SlNo).HasColumnName("SLNO");
        builder.Property(e => e.BookingNumber).HasColumnName("BOK_NUM").HasColumnType("decimal(38)");
        builder.Property(e => e.CostUnit).HasColumnName("COST_UNIT").HasColumnType("char(3)").IsRequired();
        builder.Property(e => e.CostCode).HasColumnName("COST_CODE").HasMaxLength(25).IsRequired();
        builder.Property(e => e.ProductCode).HasColumnName("PRODUCT_CODE").HasMaxLength(25);
        builder.Property(e => e.SubAccountCode).HasColumnName("SUBACCOUNT_CODE").HasMaxLength(25);

        builder.Ignore(e => e.DomainEvents);
    }
}

public class TravelApParamsConfiguration : IEntityTypeConfiguration<TravelApParams>
{
    public void Configure(EntityTypeBuilder<TravelApParams> builder)
    {
        builder.ToTable("TRAVEL_AP_PARAMS");
        builder.HasKey(e => e.ApUnitId);

        builder.Property(e => e.ApUnitId).HasColumnName("AP_UNIT_ID").ValueGeneratedNever();
        builder.Property(e => e.AccountStatus).HasColumnName("AP_ACCOUNT_STATUS").HasColumnType("char(1)").IsRequired();
        builder.Property(e => e.AccountCode).HasColumnName("AP_ACCOUNT_CODE").HasMaxLength(25).IsRequired();
        builder.Property(e => e.ControlCombId).HasColumnName("AP_CONTROLCOMBID");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class SourceHistoryConfiguration : IEntityTypeConfiguration<SourceHistory>
{
    public void Configure(EntityTypeBuilder<SourceHistory> builder)
    {
        builder.ToTable("SOURCE_HIST");
        builder.HasNoKey();

        builder.Property(e => e.ChangeDate).HasColumnName("CHANGE_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(30);
        builder.Property(e => e.Type).HasColumnName("TYPE").HasMaxLength(12);
        builder.Property(e => e.Line).HasColumnName("LINE").HasColumnType("decimal(38)");
        builder.Property(e => e.Text).HasColumnName("TEXT").HasMaxLength(4000);

        builder.Ignore(e => e.DomainEvents);
    }
}
