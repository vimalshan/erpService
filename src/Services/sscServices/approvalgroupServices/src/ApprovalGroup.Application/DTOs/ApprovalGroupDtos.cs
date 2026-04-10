namespace ApprovalGroup.Application.DTOs;

public class ApprovalGroupDto
{
    public long GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public long GroupCreatedBy { get; set; }
    public DateTime GroupCreatedOn { get; set; }
    public long? GroupModifiedBy { get; set; }
    public DateTime? GroupModifiedOn { get; set; }
    public long? GroupPriorityId { get; set; }
    public List<ApprovalGroupMapDto> GroupMaps { get; set; } = new();
    public List<ApprovalGroupUserMapDto> UserMaps { get; set; } = new();
}

public class ApprovalGroupMapDto
{
    public long MapId { get; set; }
    public long MapGroupId { get; set; }
    public int MapPayBySpecific { get; set; }
    public int MapBuSpecific { get; set; }
    public long MapMainCat { get; set; }
    public long MapSubCat { get; set; }
    public string? MapCurrency { get; set; }
    public long MapCreatedBy { get; set; }
    public DateTime MapCreatedOn { get; set; }
    public List<ApprovalGroupUnitMapDto> UnitMaps { get; set; } = new();
    public List<ApprovalGroupPayByDto> PayByMaps { get; set; } = new();
    public List<ApprovalGroupMainCatMapDto> MainCatMaps { get; set; } = new();
}

public class ApprovalGroupUnitMapDto
{
    public long MapId { get; set; }
    public long MapGroupMapId { get; set; }
    public string MapBuId { get; set; } = string.Empty;
}

public class ApprovalGroupPayByDto
{
    public long MapId { get; set; }
    public long MapGroupMapId { get; set; }
    public int MapPayBy { get; set; }
}

public class ApprovalGroupMainCatMapDto
{
    public long MapId { get; set; }
    public long MapGroupMapId { get; set; }
    public long MapMainCat { get; set; }
}

public class ApprovalGroupUserMapDto
{
    public long MapId { get; set; }
    public long MapGroupId { get; set; }
    public long MapUserId { get; set; }
    public DateTime MapEffectiveDate { get; set; }
    public DateTime? MapClosureDate { get; set; }
    public long MapCreatedBy { get; set; }
    public DateTime MapCreatedOn { get; set; }
}

public class PullMatrixDetailDto
{
    public long MatId { get; set; }
    public long MatUnitId { get; set; }
    public string MatPayBy { get; set; } = string.Empty;
    public string MatFlag { get; set; } = string.Empty;
    public long MatMainCat { get; set; }
    public long MatEmpSysId { get; set; }
    public long MatMaxNos { get; set; }
    public long MatCreatedBy { get; set; }
    public DateTime MatCreatedOn { get; set; }
    public long MatModifiedBy { get; set; }
    public DateTime MatModifiedOn { get; set; }
}
