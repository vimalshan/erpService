using EximManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EximManagement.Infrastructure.Data.Configurations;

public class EximDataFileConfiguration : IEntityTypeConfiguration<EximDataFile>
{
    public void Configure(EntityTypeBuilder<EximDataFile> builder)
    {
        builder.ToTable("EXIM_DATAFILE");
        builder.HasKey(e => e.FileId);
        builder.Property(e => e.FileId).HasColumnName("FILE_ID");
        builder.Property(e => e.FileType).HasColumnName("FILE_TYPE").HasMaxLength(10).IsRequired();
        builder.Property(e => e.FileName).HasColumnName("FILE_NAME").HasMaxLength(200);
        builder.Property(e => e.OriginalCount).HasColumnName("ORIGINALCOUNT");
        builder.Property(e => e.FinalCount).HasColumnName("FINALCOUNT");
        builder.Property(e => e.FileUploadedBy).HasColumnName("FILE_UPLOADEDBY");
        builder.Property(e => e.FileUploadedOn).HasColumnName("FILE_UPLOADEDON");
        builder.Property(e => e.Remarks).HasColumnName("REMARKS").HasMaxLength(1000);
        builder.Property(e => e.FileSource).HasColumnName("FILE_SOURCE").HasMaxLength(10);
        builder.Property(e => e.DelFlag).HasColumnName("DEL_FLAG").HasMaxLength(1);
        builder.Property(e => e.DeletedDate).HasColumnName("DELETED_DATE").HasMaxLength(255);
        builder.Property(e => e.DeletedBy).HasColumnName("DELETED_BY").HasMaxLength(255);
        builder.Property(e => e.DataTypeCode).HasColumnName("DATATYPE_CODE").HasMaxLength(1);
        builder.Property(e => e.DataTypeMonth).HasColumnName("DATATYPE_MONTH").HasMaxLength(255);
        builder.Property(e => e.DataXml).HasColumnName("DATA_XML").HasColumnType("nvarchar(max)");
    }
}

public class EximProductConfiguration : IEntityTypeConfiguration<EximProduct>
{
    public void Configure(EntityTypeBuilder<EximProduct> builder)
    {
        builder.ToTable("EXIM_PRODUCT");
        builder.HasKey(e => e.ProductId);
        builder.Property(e => e.ProductId).HasColumnName("PRODUCT_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.ProductName).HasColumnName("PRODUCT_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.ProductOracleCode).HasColumnName("PRODUCT_ORACLE_CODE").HasMaxLength(50);
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.LastUpdatedOn).HasColumnName("LAST_UPDATED_ON");
        builder.Property(e => e.Status).HasColumnName("STATUS").HasColumnType("char(1)");
        builder.Ignore(e => e.Searches);

        builder.HasData(
            new { ProductId = 1001L, ProductName = "Cotton Yarn", ProductOracleCode = (string?)"CY001", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { ProductId = 1002L, ProductName = "Polyester Fabric", ProductOracleCode = (string?)"PF001", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { ProductId = 1003L, ProductName = "Denim Cloth", ProductOracleCode = (string?)"DC001", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { ProductId = 1004L, ProductName = "Silk Threads", ProductOracleCode = (string?)"ST001", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { ProductId = 1005L, ProductName = "Woollen Yarn", ProductOracleCode = (string?)"WY001", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' }
        );
    }
}

public class EximProductSearchConfiguration : IEntityTypeConfiguration<EximProductSearch>
{
    public void Configure(EntityTypeBuilder<EximProductSearch> builder)
    {
        builder.ToTable("EXIM_PRODUCT_SEARCH");
        builder.HasKey(e => e.SearchId);
        builder.Property(e => e.SearchId).HasColumnName("SEARCH_ID");
        builder.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
        builder.Property(e => e.SearchItcCode).HasColumnName("SEARCH_ITC_CODE").HasMaxLength(10);
        builder.Property(e => e.SearchText).HasColumnName("SEARCH_TEXT").HasMaxLength(50);
        builder.Property(e => e.NotInText).HasColumnName("NOTIN_TEXT").HasMaxLength(50);
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.LastUpdatedOn).HasColumnName("LAST_UPDATED_ON");
    }
}

public class EximProductGroupConfiguration : IEntityTypeConfiguration<EximProductGroup>
{
    public void Configure(EntityTypeBuilder<EximProductGroup> builder)
    {
        builder.ToTable("EXIM_PRODUCTGROUP");
        builder.HasKey(e => e.GroupId);
        builder.Property(e => e.GroupId).HasColumnName("GROUP_ID");
        builder.Property(e => e.GroupName).HasColumnName("GROUP_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.LastUpdatedOn).HasColumnName("LAST_UPDATED_ON");
        builder.Property(e => e.Status).HasColumnName("STATUS").HasColumnType("char(1)");
        builder.Ignore(e => e.Mappings);

        builder.HasData(
            new { GroupId = 101L, GroupName = "Textile Yarns", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { GroupId = 102L, GroupName = "Woven Fabrics", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { GroupId = 103L, GroupName = "Denim Products", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { GroupId = 104L, GroupName = "Silk Products", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' },
            new { GroupId = 105L, GroupName = "Woollen Products", LastUpdatedBy = 1L, LastUpdatedOn = new DateTime(2024, 1, 1), Status = 'Y' }
        );
    }
}

public class EximProductGroupMapConfiguration : IEntityTypeConfiguration<EximProductGroupMap>
{
    public void Configure(EntityTypeBuilder<EximProductGroupMap> builder)
    {
        builder.ToTable("EXIM_PRODUCTGROUP_MAP");
        builder.HasKey(e => e.MapId);
        builder.Property(e => e.MapId).HasColumnName("MAP_ID");
        builder.Property(e => e.GroupId).HasColumnName("GROUP_ID");
        builder.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.LastUpdatedOn).HasColumnName("LAST_UPDATED_ON");
    }
}

public class EximDataExportConfiguration : IEntityTypeConfiguration<EximDataExport>
{
    public void Configure(EntityTypeBuilder<EximDataExport> builder)
    {
        builder.ToTable("EXIM_DATA_EXPORT");
        builder.HasKey(e => e.DataId);
        builder.Property(e => e.DataId).HasColumnName("DATA_ID");
        builder.Property(e => e.EximDate).HasColumnName("EXIM_DATE");
        builder.Property(e => e.HsCode).HasColumnName("HSCODE");
        builder.Property(e => e.ProdDesc).HasColumnName("PRODDESC").HasMaxLength(500);
        builder.Property(e => e.PortDest).HasColumnName("PORTDEST").HasMaxLength(500);
        builder.Property(e => e.CountryDest).HasColumnName("COUNTRYDEST").HasMaxLength(500);
        builder.Property(e => e.PortOrigin).HasColumnName("PORTORIGIN").HasMaxLength(200);
        builder.Property(e => e.StdQty).HasColumnName("STDQTY");
        builder.Property(e => e.StdUnit).HasColumnName("STDUNIT").HasMaxLength(18);
        builder.Property(e => e.StdUnitRate).HasColumnName("STDUNITRATE").HasColumnType("decimal(38,6)");
        builder.Property(e => e.FobInr).HasColumnName("FOBINR");
        builder.Property(e => e.FobDol).HasColumnName("FOBDOL");
        builder.Property(e => e.ModeShip).HasColumnName("MODESHIP").HasMaxLength(200);
        builder.Property(e => e.FileId).HasColumnName("FILE_ID");
        builder.Property(e => e.EMonth).HasColumnName("EMONTH").HasMaxLength(200);
        builder.Property(e => e.ExpName).HasColumnName("EXP_NAME").HasMaxLength(255);
        builder.Property(e => e.ImpName).HasColumnName("IMP_NAME").HasMaxLength(255);
        builder.Property(e => e.ImpCountry).HasColumnName("IMP_COUNTRY").HasMaxLength(255);
        builder.Property(e => e.Iec).HasColumnName("IEC").HasMaxLength(200);
        builder.Property(e => e.SbNo).HasColumnName("SB_NO").HasMaxLength(255);
        builder.Property(e => e.InvNo).HasColumnName("INV_NO").HasMaxLength(255);
        builder.Property(e => e.Hs2).HasColumnName("HS2").HasMaxLength(200);
        builder.Property(e => e.Hs4).HasColumnName("HS4").HasMaxLength(200);
        builder.Property(e => e.HsDesc).HasColumnName("HS_DESC").HasMaxLength(255);
        builder.Property(e => e.InvDate).HasColumnName("INV_DATE");
    }
}

public class EximDataImportConfiguration : IEntityTypeConfiguration<EximDataImport>
{
    public void Configure(EntityTypeBuilder<EximDataImport> builder)
    {
        builder.ToTable("EXIM_DATA_IMPORT");
        builder.HasKey(e => e.DataId);
        builder.Property(e => e.DataId).HasColumnName("DATA_ID");
        builder.Property(e => e.EximDate).HasColumnName("EXIM_DATE");
        builder.Property(e => e.HsCode).HasColumnName("HSCODE");
        builder.Property(e => e.ProdDesc).HasColumnName("PRODDESC").HasMaxLength(500);
        builder.Property(e => e.PortDest).HasColumnName("PORTDEST").HasMaxLength(500);
        builder.Property(e => e.CountryOrg).HasColumnName("COUNTRYORG").HasMaxLength(500);
        builder.Property(e => e.ModeShip).HasColumnName("MODESHIP").HasMaxLength(200);
        builder.Property(e => e.FileId).HasColumnName("FILE_ID");
        builder.Property(e => e.ImpName).HasColumnName("IMP_NAME").HasMaxLength(255);
        builder.Property(e => e.ExpName).HasColumnName("EXP_NAME").HasMaxLength(255);
        builder.Property(e => e.Iec).HasColumnName("IEC").HasMaxLength(200);
        builder.Property(e => e.BeNo).HasColumnName("BE_NO").HasMaxLength(255);
        builder.Property(e => e.Hs2).HasColumnName("HS2").HasMaxLength(200);
        builder.Property(e => e.Hs4).HasColumnName("HS4").HasMaxLength(200);
        builder.Property(e => e.HsDesc).HasColumnName("HS_DESC").HasMaxLength(255);
        builder.Property(e => e.InvDate).HasColumnName("INV_DATE");
        builder.Property(e => e.EMonth).HasColumnName("EMONTH").HasMaxLength(200);
        builder.Property(e => e.StdQty).HasColumnName("STDQTY").HasColumnType("decimal(38,6)");
        builder.Property(e => e.StdUnit).HasColumnName("STDUNIT").HasMaxLength(18);
        builder.Property(e => e.StdUnitRate).HasColumnName("STDUNITRATE").HasColumnType("decimal(38,6)");
        builder.Property(e => e.UnitRateDol).HasColumnName("UNITRATEDOL").HasColumnType("decimal(38,6)");
        builder.Property(e => e.FobInr).HasColumnName("FOBINR").HasColumnType("decimal(38,6)");
        builder.Property(e => e.FobDol).HasColumnName("FOBDOL").HasColumnType("decimal(38,6)");
        builder.Property(e => e.Qty).HasColumnName("QTY").HasColumnType("decimal(38,6)");
    }
}

public class EximUserMasterConfiguration : IEntityTypeConfiguration<EximUserMaster>
{
    public void Configure(EntityTypeBuilder<EximUserMaster> builder)
    {
        builder.ToTable("EXIM_USERMASTER");
        builder.HasKey(e => e.EximUserId);
        builder.Property(e => e.EximUserId).HasColumnName("EXIM_USERID");
        builder.Property(e => e.EximEmpSysId).HasColumnName("EXIM_EMPSYSID");
        builder.Property(e => e.EximSparshId).HasColumnName("EXIM_SPARSHID").HasMaxLength(50);
        builder.Property(e => e.EximUserEffectiveDate).HasColumnName("EXIM_USER_EFFECTIVEDATE");
        builder.Property(e => e.EximUserClosureDate).HasColumnName("EXIM_USER_CLOSUREDATE");
        builder.Property(e => e.EximUserEnteredBy).HasColumnName("EXIM_USER_ENTEREDBY");
    }
}
