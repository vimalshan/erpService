using ConfigService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConfigService.Infrastructure.Persistence.Configurations;

public class TravelCountryConfiguration : IEntityTypeConfiguration<TravelCountry>
{
    public void Configure(EntityTypeBuilder<TravelCountry> b)
    {
        b.ToTable("TRAVEL_COUNTRYMASTER");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("COUNTRY_ID").HasMaxLength(255);
        b.Property(e => e.CountryName).HasColumnName("COUNTRY_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.AirCode).HasColumnName("COUNTRY_AIRID").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("COUNTRY_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("COUNTRY_LASTMODIFIEDON");
        b.Property(e => e.GhAvailable).HasColumnName("COUTRY_GHAVAILABLE").HasMaxLength(1);
        b.Property(e => e.GhRate).HasColumnName("COUNTRY_GHRATE").HasMaxLength(255);
        b.Property(e => e.NmsGhRate).HasColumnName("COUNTRY_NMSGHRATE").HasMaxLength(255);
        b.HasMany(e => e.ModeMaps).WithOne().HasForeignKey(m => m.CountryId);
        b.HasMany(e => e.SectorMaps).WithOne().HasForeignKey(m => m.CountryId);
        b.HasMany(e => e.CurrencyMaps).WithOne().HasForeignKey(m => m.CountryId);
    }
}

public class TravelCountryModeMapConfiguration : IEntityTypeConfiguration<TravelCountryModeMap>
{
    public void Configure(EntityTypeBuilder<TravelCountryModeMap> b)
    {
        b.ToTable("TRAVEL_COUNTRYMODEMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("MODE_MAPID").HasMaxLength(255);
        b.Property(e => e.ModeId).HasColumnName("MODE_ID").HasMaxLength(255).IsRequired();
        b.Property(e => e.CountryId).HasColumnName("MODE_COUNTRYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("MODE_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("MODE_LASTMODIFIEDON");
    }
}

public class TravelCountrySectorMapConfiguration : IEntityTypeConfiguration<TravelCountrySectorMap>
{
    public void Configure(EntityTypeBuilder<TravelCountrySectorMap> b)
    {
        b.ToTable("TRAVEL_COUNTRYSECMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("SECTOR_MAPID").HasMaxLength(255);
        b.Property(e => e.CountryId).HasColumnName("SECTOR_COUNTRYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ClassId).HasColumnName("SECTOR_CLASSID").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("SECTOR_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("SECTOR_LASTMODIFIEDON");
    }
}

public class TravelCountryCurrencyMapConfiguration : IEntityTypeConfiguration<TravelCountryCurrencyMap>
{
    public void Configure(EntityTypeBuilder<TravelCountryCurrencyMap> b)
    {
        b.ToTable("TRAVEL_COUNTRYCURMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CURMAP_ID").HasMaxLength(255);
        b.Property(e => e.CurrencyId).HasColumnName("CURMAP_CURRENCYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.CountryId).HasColumnName("CURMAP_COUNTRYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("CURMAP_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CURMAP_LASTMODIFIEDON");
    }
}

public class TravelCityConfiguration : IEntityTypeConfiguration<TravelCity>
{
    public void Configure(EntityTypeBuilder<TravelCity> b)
    {
        b.ToTable("TRAVEL_CITYMASTER");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CITY_ID").HasMaxLength(255);
        b.Property(e => e.CountryId).HasColumnName("CITY_COUNTRYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.CityName).HasColumnName("CITY_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.CityCode).HasColumnName("CITY_CODE").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("CITY_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CITY_LASTMODIFIEDON");
        b.HasMany(e => e.ModeMaps).WithOne().HasForeignKey(m => m.CityId);
        b.HasMany(e => e.SectorMaps).WithOne().HasForeignKey(m => m.CityId);
    }
}

public class TravelCityModeMapConfiguration : IEntityTypeConfiguration<TravelCityModeMap>
{
    public void Configure(EntityTypeBuilder<TravelCityModeMap> b)
    {
        b.ToTable("TRAVEL_CITYMODEMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CITYMODE_MAPID").HasMaxLength(255);
        b.Property(e => e.CityId).HasColumnName("CITYMODE_CITYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ModeId).HasColumnName("CITYMODE_MODEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("CITYMODE_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CITYMODE_LASTMODIFIEDON");
    }
}

public class TravelCitySectorMapConfiguration : IEntityTypeConfiguration<TravelCitySectorMap>
{
    public void Configure(EntityTypeBuilder<TravelCitySectorMap> b)
    {
        b.ToTable("TRAVEL_CITYSECMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CITYSEC_MAPID").HasMaxLength(255);
        b.Property(e => e.CityId).HasColumnName("CITYSEC_CITYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ClassId).HasColumnName("CITYSEC_CLASSID").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("CITYSEC_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CITYSEC_LASTMODIFIEDON");
        b.Property(e => e.GradeFCat).HasColumnName("CITYSEC_GRADEFCAT").HasMaxLength(255).IsRequired();
    }
}

public class TravelClassConfiguration : IEntityTypeConfiguration<TravelClass>
{
    public void Configure(EntityTypeBuilder<TravelClass> b)
    {
        b.ToTable("TRAVEL_CLASS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CLASS_ID").HasMaxLength(255);
        b.Property(e => e.ModeId).HasColumnName("CLASS_MODEID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ClassName).HasColumnName("CLASS_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.ClassOrder).HasColumnName("CLASS_ORDER").HasMaxLength(255).IsRequired();
    }
}

public class TravelContactConfiguration : IEntityTypeConfiguration<TravelContact>
{
    public void Configure(EntityTypeBuilder<TravelContact> b)
    {
        b.ToTable("TRAVEL_CONTACT");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CONTACT_ID").HasMaxLength(255);
        b.Property(e => e.ContactType).HasColumnName("CONTACT_TYPE").HasMaxLength(255);
        b.Property(e => e.AdminId).HasColumnName("CONTACT_ADMINID").HasMaxLength(255);
        b.Property(e => e.AdminName).HasColumnName("CONTACT_ADMNAME").HasMaxLength(255);
        b.Property(e => e.EmployeeSysId).HasColumnName("CONTACT_EMPSYSID").HasMaxLength(255);
        b.Property(e => e.PhoneNos).HasColumnName("CONTACT_PHONENOS").HasMaxLength(255);
        b.Property(e => e.EmailId).HasColumnName("CONTACT_EMAILID").HasMaxLength(255);
        b.Property(e => e.LastModifiedBy).HasColumnName("CONTACT_LASTMODIFIEDBY").HasMaxLength(255);
        b.Property(e => e.LastModifiedOn).HasColumnName("CONTACT_LASTMODIFIEDON");
    }
}

public class TravelBusCitySectorMapConfiguration : IEntityTypeConfiguration<TravelBusCitySectorMap>
{
    public void Configure(EntityTypeBuilder<TravelBusCitySectorMap> b)
    {
        b.ToTable("TRAVEL_BUSCITYSECMAP");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("CITYBUS_MAPID").HasMaxLength(255);
        b.Property(e => e.CityId).HasColumnName("CITYBUS_CITYID").HasMaxLength(255).IsRequired();
        b.Property(e => e.ClassId).HasColumnName("CITYBUS_CLASSID").HasMaxLength(255).IsRequired();
        b.Property(e => e.BusinessId).HasColumnName("CITYBUS_BUSID").HasMaxLength(255).IsRequired();
    }
}

public class TravelBuExcludeConfiguration : IEntityTypeConfiguration<TravelBuExclude>
{
    public void Configure(EntityTypeBuilder<TravelBuExclude> b)
    {
        b.ToTable("TRAVEL_BUEXCLUDE");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("BU_EXID").HasMaxLength(255);
        b.Property(e => e.EmployeeSysId).HasColumnName("BU_EMPSYSID").HasMaxLength(255);
        b.Property(e => e.UnitId).HasColumnName("BU_UNITID").HasMaxLength(255);
    }
}
