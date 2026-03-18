using OrganizationStructureService.Domain.Common;

namespace OrganizationStructureService.Domain.Entities;

public class Location : Entity
{
    public decimal LocationCode { get; private set; }
    public string? LocationName { get; private set; }
    public decimal LocationRegionCode { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public decimal? UpdatedBy { get; private set; }

    private Location() { }

    public static Location Create(decimal locationCode, string locationName, decimal regionCode, decimal updatedBy)
    {
        return new Location
        {
            LocationCode = locationCode,
            LocationName = locationName,
            LocationRegionCode = regionCode,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
    }
}

public class Region : Entity
{
    public decimal RegionCode { get; private set; }
    public string? RegionName { get; private set; }
    public decimal RegionCountryCode { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public decimal? UpdatedBy { get; private set; }

    private Region() { }

    public static Region Create(decimal regionCode, string regionName, decimal countryCode, decimal updatedBy)
    {
        return new Region
        {
            RegionCode = regionCode,
            RegionName = regionName,
            RegionCountryCode = countryCode,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
    }
}

public class Level : Entity
{
    public decimal LevelId { get; private set; }
    public string? LevelName { get; private set; }
    public string? LevelDesignation { get; private set; }
    public decimal? LevelGradeId { get; private set; }
    public string? LevelLiveFlag { get; private set; }
    public decimal? LevelPriority { get; private set; }
    public decimal? LastUpdatedBy { get; private set; }
    public DateTime? LastUpdatedOn { get; private set; }

    private Level() { }

    public static Level Create(decimal levelId, string levelName, decimal gradeId, decimal updatedBy)
    {
        return new Level
        {
            LevelId = levelId,
            LevelName = levelName,
            LevelGradeId = gradeId,
            LevelLiveFlag = "Y",
            LastUpdatedBy = updatedBy,
            LastUpdatedOn = DateTime.UtcNow
        };
    }
}

public class HrRole : Entity
{
    public decimal HrRoleId { get; private set; }
    public string HrRoleCode { get; private set; } = string.Empty;
    public string HrRoleName { get; private set; } = string.Empty;

    private HrRole() { }

    public static HrRole Create(decimal hrRoleId, string hrRoleCode, string hrRoleName)
    {
        return new HrRole { HrRoleId = hrRoleId, HrRoleCode = hrRoleCode, HrRoleName = hrRoleName };
    }
}

public class LovMaster : Entity
{
    public string? LovType { get; private set; }
    public decimal LovId { get; private set; }
    public string? LovName { get; private set; }
    public decimal? LovUpdatedBy { get; private set; }
    public DateTime? LovUpdatedOn { get; private set; }

    private LovMaster() { }

    public static LovMaster Create(decimal lovId, string lovType, string lovName, decimal updatedBy)
    {
        return new LovMaster { LovId = lovId, LovType = lovType, LovName = lovName, LovUpdatedBy = updatedBy, LovUpdatedOn = DateTime.UtcNow };
    }
}
