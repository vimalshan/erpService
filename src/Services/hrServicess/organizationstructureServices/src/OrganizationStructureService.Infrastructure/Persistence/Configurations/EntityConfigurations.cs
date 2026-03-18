using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationStructureService.Domain.Entities;
using OrganizationStructureService.Domain.ValueObjects;

namespace OrganizationStructureService.Infrastructure.Persistence.Configurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.ToTable("BUSINESS_MASTER");
        builder.HasKey(b => b.BusinessId);
        builder.Property(b => b.BusinessId).HasColumnName("BUSINESS_ID").HasColumnType("decimal(38,0)");
        builder.Property(b => b.BusinessName).HasColumnName("BUSINESS_NAME").HasMaxLength(50).IsRequired();
        builder.Property(b => b.BusinessShortName).HasColumnName("BUSINESS_SHTNAME").HasMaxLength(10).IsRequired();
        builder.Property(b => b.BusinessCode).HasColumnName("BUSINESS_CODE").HasMaxLength(9).IsFixedLength().IsRequired();
        builder.Property(b => b.BusinessCompanyId).HasColumnName("BUSINESS_COMPID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(b => b.BusinessCompanyCode).HasColumnName("BUSINESS_COMP_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(b => b.UpdatedOn).HasColumnName("BUSINESS_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(b => b.UpdatedBy).HasColumnName("BUSINESS_UPDATEDBY").HasColumnType("decimal(22,0)");

        builder.Property(b => b.LiveFlag)
            .HasColumnName("BUSINESS_LIVFLAG")
            .HasMaxLength(1)
            .IsFixedLength()
            .IsRequired()
            .HasConversion(lf => lf.Value, v => LiveFlag.From(v));

        builder.Ignore(b => b.DomainEvents);
        builder.Ignore(b => b.Version);
    }
}

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("UNIT_MASTER");
        builder.HasKey(u => u.UnitId);
        builder.Property(u => u.UnitId).HasColumnName("UNIT_ID").HasColumnType("decimal(38,0)");
        builder.Property(u => u.UnitName).HasColumnName("UNIT_NAME").HasMaxLength(50).IsRequired();
        builder.Property(u => u.UnitShortName).HasColumnName("UNIT_SHTNAME").HasMaxLength(20).IsRequired();
        builder.Property(u => u.UnitBusinessId).HasColumnName("UNIT_BUSINESSID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(u => u.UnitBusinessCode).HasColumnName("UNIT_BUSINESS_CODE").HasMaxLength(9).IsFixedLength().IsRequired();
        builder.Property(u => u.UpdatedOn).HasColumnName("UNIT_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(u => u.UpdatedBy).HasColumnName("UNIT_UPDATEDBY").HasColumnType("decimal(22,0)");
        builder.Property(u => u.OrgId).HasColumnName("UNIT_ORGID").HasColumnType("decimal(22,0)").IsRequired();
        builder.Property(u => u.ReportFlag).HasColumnName("UNIT_RPTFLAG").HasMaxLength(1).IsFixedLength().IsRequired();
        builder.Property(u => u.PayFlag).HasColumnName("UNIT_PAYFLAG").HasMaxLength(1).IsFixedLength();
        builder.Property(u => u.PayLiveFlag).HasColumnName("UNIT_PAYLIVEFLAG").HasMaxLength(1).IsFixedLength();
        builder.Property(u => u.RegionalLanguageFlag).HasColumnName("UNIT_REGLANGFLAG").HasMaxLength(1).IsFixedLength();
        builder.Property(u => u.RegionalLanguageCode).HasColumnName("UNIT_REGLANGCODE").HasMaxLength(3);
        builder.Property(u => u.PfFlag).HasColumnName("UNIT_PFFLG").HasMaxLength(1).IsFixedLength();

        builder.Property(u => u.UnitCode)
            .HasColumnName("UNIT_CODE")
            .HasMaxLength(3)
            .IsFixedLength()
            .IsRequired()
            .HasConversion(uc => uc.Value, v => UnitCode.From(v));

        builder.Property(u => u.LiveFlag)
            .HasColumnName("UNIT_LIVFLAG")
            .HasMaxLength(1)
            .IsFixedLength()
            .IsRequired()
            .HasConversion(lf => lf.Value, v => LiveFlag.From(v));

        builder.Ignore(u => u.DomainEvents);
        builder.Ignore(u => u.Version);
    }
}

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("DEPARTMENT_MASTER");
        builder.HasKey(d => d.DepartmentId);
        builder.Property(d => d.DepartmentId).HasColumnName("DEPARTMENT_ID").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DepartmentName).HasColumnName("DEPARTMENT_NAME").HasMaxLength(50);
        builder.Property(d => d.DepartmentCode).HasColumnName("DEPARTMENT_CODE").HasMaxLength(50);
        builder.Property(d => d.UpdatedOn).HasColumnName("DEPARTMENT_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(d => d.UpdatedBy).HasColumnName("DEPARTMENT_UPDATEDBY").HasColumnType("decimal(22,0)");

        builder.Property(d => d.LiveFlag)
            .HasColumnName("DEPARTMENT_LIVFLAG")
            .HasMaxLength(1)
            .IsFixedLength()
            .HasConversion(lf => lf != null ? lf.Value : null, v => v != null ? LiveFlag.From(v) : null);

        builder.Ignore(d => d.DomainEvents);
    }
}

public class DivisionConfiguration : IEntityTypeConfiguration<Division>
{
    public void Configure(EntityTypeBuilder<Division> builder)
    {
        builder.ToTable("DIVISION_MASTER");
        builder.HasKey(d => d.DivisionId);
        builder.Property(d => d.DivisionId).HasColumnName("DIVISION_ID").HasColumnType("decimal(38,0)");
        builder.Property(d => d.DivisionCode).HasColumnName("DIVISION_CODE").HasMaxLength(3);
        builder.Property(d => d.DivisionName).HasColumnName("DIVISION_NAME").HasMaxLength(50);
        builder.Property(d => d.UpdatedOn).HasColumnName("DIVISION_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(d => d.UpdatedBy).HasColumnName("DIVISION_UPDATEDBY").HasColumnType("decimal(22,0)");

        builder.Property(d => d.LiveFlag)
            .HasColumnName("DIVISION_LIVEFLAG")
            .HasMaxLength(1)
            .IsFixedLength()
            .HasConversion(lf => lf != null ? lf.Value : null, v => v != null ? LiveFlag.From(v) : null);

        builder.Ignore(d => d.DomainEvents);
    }
}

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("GRADE_MASTER");
        builder.HasKey(g => g.GradeId);
        builder.Property(g => g.GradeId).HasColumnName("GRADE_ID").HasColumnType("decimal(38,0)");
        builder.Property(g => g.GradeCode).HasColumnName("GRADE_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(g => g.GradeName).HasColumnName("GRADE_NAME").HasMaxLength(50);
        builder.Property(g => g.GradeDesignation).HasColumnName("GRADE_DESIGNATION").HasMaxLength(50);
        builder.Property(g => g.GradeCategoryCode).HasColumnName("GRADE_CATEGORYCODE").HasMaxLength(3).IsFixedLength();
        builder.Property(g => g.ManagementCategoryCode).HasColumnName("GRADE_MAN_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(g => g.Priority).HasColumnName("GRADE_PRIORITY").HasColumnType("decimal(38,0)");
        builder.Property(g => g.SubCategory).HasColumnName("GRADE_SUBCAT").HasMaxLength(3).IsFixedLength();
        builder.Property(g => g.DefaultRating).HasColumnName("GRADE_DEFRATING").HasMaxLength(25);
        builder.Property(g => g.PromotionScore).HasColumnName("GRADE_PROMSCORE").HasColumnType("decimal(38,0)");
        builder.Property(g => g.LevelCount).HasColumnName("GRADE_LEVELNOS").HasColumnType("decimal(38,0)");
        builder.Property(g => g.CadreId).HasColumnName("GRADE_CADREID").HasColumnType("decimal(38,0)");

        builder.Property(g => g.LiveFlag)
            .HasColumnName("GRADE_LIVFLAG")
            .HasMaxLength(1)
            .IsFixedLength()
            .HasConversion(lf => lf != null ? lf.Value : null, v => v != null ? LiveFlag.From(v) : null);

        builder.Ignore(g => g.DomainEvents);
        builder.Ignore(g => g.Version);
    }
}

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("POSITION_MASTER");
        builder.HasKey(p => p.PositionId);
        builder.Property(p => p.PositionId).HasColumnName("POSITION_ID").HasColumnType("decimal(38,0)");
        builder.Property(p => p.PosUnitCode).HasColumnName("POS_UNIT_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(p => p.PosGradeId).HasColumnName("POS_GRADE_ID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(p => p.PositionName).HasColumnName("POSITION_NAME").HasMaxLength(100);
        builder.Property(p => p.PositionDesignation).HasColumnName("POSITION_DESIGNATION").HasMaxLength(100).IsRequired();
        builder.Property(p => p.PosEffectiveDate).HasColumnName("POS_EFFECTIVE_DATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(p => p.PosClosedDate).HasColumnName("POS_CLOSED_DATE").HasColumnType("datetime2(3)");
        builder.Property(p => p.ReferenceCode).HasColumnName("REFERENCE_CODE").HasMaxLength(3).IsFixedLength().IsRequired();
        builder.Property(p => p.DeletedFlag).HasColumnName("DELETED_FLAG").HasMaxLength(1).IsFixedLength();
        builder.Property(p => p.PositionJdId).HasColumnName("POSITION_JD_ID").HasColumnType("decimal(38,0)");
        builder.Property(p => p.EnteredDate).HasColumnName("ENTERED_DATE").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(p => p.EnteredPinNo).HasColumnName("ENTERED_PIN_NO").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(p => p.Ctc).HasColumnName("CTC").HasColumnType("decimal(38,0)");
        builder.Property(p => p.ProcessId).HasColumnName("PROCESS_ID").HasColumnType("decimal(38,0)");
        builder.Property(p => p.ReasonId).HasColumnName("REASON_ID").HasColumnType("decimal(38,0)");
        builder.Property(p => p.ReplacePositionId).HasColumnName("REPLACE_POSID").HasColumnType("decimal(38,0)");
        builder.Property(p => p.PosModifiedBy).HasColumnName("POS_MODIFIED_BY").HasColumnType("decimal(38,0)");
        builder.Property(p => p.PosModifiedOn).HasColumnName("POS_MODIFIED_ON").HasColumnType("datetime2(3)");
        builder.Property(p => p.PosUnitId).HasColumnName("POS_UNIT_ID").HasColumnType("decimal(38,0)");
        builder.Property(p => p.PosRefNo).HasColumnName("POS_REFNO");
        builder.Property(p => p.PosEvaluatedGradeId).HasColumnName("POS_EVEGRADE_ID").IsRequired();
        builder.Property(p => p.PositionEvaluatedDesignation).HasColumnName("POSITION_EVEDESIGNATION").HasMaxLength(100).IsRequired();

        builder.Ignore(p => p.DomainEvents);
        builder.Ignore(p => p.Version);
    }
}

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("SITE_MASTER");
        builder.HasKey(s => s.SiteId);
        builder.Property(s => s.SiteId).HasColumnName("SITE_ID").HasColumnType("decimal(38,0)");
        builder.Property(s => s.SiteName).HasColumnName("SITE_NAME").HasMaxLength(200);
        builder.Property(s => s.SiteShortName).HasColumnName("SITE_SHORT_NAME").HasMaxLength(200);
        builder.Property(s => s.AddressLine1).HasColumnName("SITE_ADDRESS_LINE_1").HasMaxLength(200);
        builder.Property(s => s.AddressLine2).HasColumnName("SITE_ADDRESS_LINE_2").HasMaxLength(200);
        builder.Property(s => s.AddressLine3).HasColumnName("SITE_ADDRESS_LINE_3").HasMaxLength(200);
        builder.Property(s => s.AddressLine4).HasColumnName("SITE_ADDRESS_LINE_4").HasMaxLength(200);
        builder.Property(s => s.AddressPin).HasColumnName("SITE_ADDRESS_PIN").HasMaxLength(50);
        builder.Property(s => s.SiteCityCode).HasColumnName("SITE_CITY_CODE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(s => s.SiteCategoryCode).HasColumnName("SITE_CATEGORY_CODE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(s => s.Phone1).HasColumnName("SITE_PHONE_1").HasMaxLength(200);
        builder.Property(s => s.Phone2).HasColumnName("SITE_PHONE_2").HasMaxLength(200);
        builder.Property(s => s.FaxNo).HasColumnName("SITE_FAXNO").HasMaxLength(200);
        builder.Property(s => s.Landmark1).HasColumnName("SITE_LANDMARK_1").HasMaxLength(200);
        builder.Property(s => s.Landmark2).HasColumnName("SITE_LANDMARK_2").HasMaxLength(200);
        builder.Property(s => s.ImagePath).HasColumnName("SITE_IMAGEPATH").HasMaxLength(200);
        builder.Property(s => s.VisitorPolicyPath).HasColumnName("SITE_VISITORPOLICYPATH").HasMaxLength(200);
        builder.Property(s => s.NearestAirport).HasColumnName("SITE_NEARESTAIRPORT").HasMaxLength(200);
        builder.Property(s => s.DistanceAirport).HasColumnName("SITE_DISTANCEAIRPORT").HasMaxLength(200);
        builder.Property(s => s.NearestRail).HasColumnName("SITE_NEARESTRAIL").HasMaxLength(200);
        builder.Property(s => s.DistanceRail).HasColumnName("SITE_DISTANCERAIL").HasMaxLength(200);
        builder.Property(s => s.Remarks).HasColumnName("SITE_REMARKS").HasMaxLength(200);
        builder.Property(s => s.LocationCode).HasColumnName("SITE_LOCATION_CODE").HasColumnType("decimal(38,0)");
        builder.Property(s => s.AttachedEmployee).HasColumnName("SITE_ATTACHEDEMPLOYEE").HasMaxLength(1).IsFixedLength().IsRequired();
        builder.Property(s => s.ContactName1).HasColumnName("SITE_CONTACT_NAME1").HasMaxLength(200);
        builder.Property(s => s.ContactPhone1).HasColumnName("SITE_CONTACT_PHONE1").HasMaxLength(200);
        builder.Property(s => s.ContactName2).HasColumnName("SITE_CONTACT_NAME2").HasMaxLength(200);
        builder.Property(s => s.ContactPhone2).HasColumnName("SITE_CONTACT_PHONE2").HasMaxLength(200);
        builder.Property(s => s.TravelLocationId).HasColumnName("SITE_TRAVELLOCID");

        builder.Property(s => s.LiveFlag)
            .HasColumnName("SITE_LIVFLAG")
            .HasMaxLength(1)
            .IsFixedLength()
            .HasConversion(lf => lf != null ? lf.Value : null, v => v != null ? LiveFlag.From(v) : null);

        builder.Ignore(s => s.DomainEvents);
    }
}

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("LOCATION_MASTER");
        builder.HasKey(l => l.LocationCode);
        builder.Property(l => l.LocationCode).HasColumnName("LOCATION_CODE").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LocationName).HasColumnName("LOCATION_NAME").HasMaxLength(200);
        builder.Property(l => l.LocationRegionCode).HasColumnName("LOCATION_REGION_CODE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(l => l.UpdatedOn).HasColumnName("LOCATION_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(l => l.UpdatedBy).HasColumnName("LOCATION_UPDATEDBY").HasColumnType("decimal(22,0)");
        builder.Ignore(l => l.DomainEvents);
    }
}

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("REGION_MASTER");
        builder.HasKey(r => r.RegionCode);
        builder.Property(r => r.RegionCode).HasColumnName("REGION_CODE").HasColumnType("decimal(38,0)");
        builder.Property(r => r.RegionName).HasColumnName("REGION_NAME").HasMaxLength(200);
        builder.Property(r => r.RegionCountryCode).HasColumnName("REGION_COUNTRY_CODE").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(r => r.UpdatedOn).HasColumnName("REGION_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(r => r.UpdatedBy).HasColumnName("REGION_UPDATEDBY").HasColumnType("decimal(22,0)");
        builder.Ignore(r => r.DomainEvents);
    }
}

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.ToTable("LEVEL_MASTER");
        builder.HasKey(l => l.LevelId);
        builder.Property(l => l.LevelId).HasColumnName("LEVEL_ID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LevelName).HasColumnName("LEVEL_NAME").HasMaxLength(50);
        builder.Property(l => l.LevelDesignation).HasColumnName("LEVEL_DESIGNATION").HasMaxLength(50);
        builder.Property(l => l.LevelGradeId).HasColumnName("LEVEL_GRADEID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LevelLiveFlag).HasColumnName("LEVEL_LIVEFLAG").HasMaxLength(1).IsFixedLength();
        builder.Property(l => l.LevelPriority).HasColumnName("LEVEL_PRIORITY").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LastUpdatedBy).HasColumnName("LEVEL_LASTUPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LastUpdatedOn).HasColumnName("LEVEL_LASTUPDATEDON").HasColumnType("datetime2(3)");
        builder.Ignore(l => l.DomainEvents);
    }
}

public class HrRoleConfiguration : IEntityTypeConfiguration<HrRole>
{
    public void Configure(EntityTypeBuilder<HrRole> builder)
    {
        builder.ToTable("HRROLE_MASTER");
        builder.HasKey(h => h.HrRoleId);
        builder.Property(h => h.HrRoleId).HasColumnName("HRROLE_ID").HasColumnType("decimal(38,0)");
        builder.Property(h => h.HrRoleCode).HasColumnName("HRROLE_CODE").HasMaxLength(3).IsRequired();
        builder.Property(h => h.HrRoleName).HasColumnName("HRROLE_NAME").HasMaxLength(20).IsRequired();
        builder.Ignore(h => h.DomainEvents);
    }
}

public class LovMasterConfiguration : IEntityTypeConfiguration<LovMaster>
{
    public void Configure(EntityTypeBuilder<LovMaster> builder)
    {
        builder.ToTable("LOV_MASTER");
        builder.HasKey(l => l.LovId);
        builder.Property(l => l.LovId).HasColumnName("LOV_ID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LovType).HasColumnName("LOV_TYPE").HasMaxLength(3).IsFixedLength();
        builder.Property(l => l.LovName).HasColumnName("LOV_NAME").HasMaxLength(200);
        builder.Property(l => l.LovUpdatedBy).HasColumnName("LOV_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LovUpdatedOn).HasColumnName("LOV_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Ignore(l => l.DomainEvents);
    }
}
