namespace ProjectService.Application.DTOs;

public class ProjectMainDto
{
    public long ProjId { get; set; }
    public string ProjName { get; set; } = null!;
    public decimal ProjCharterNo { get; set; }
    public long ProjLeaderId { get; set; }
    public long ProjTypeId { get; set; }
    public string? ProjectTypeName { get; set; }
    public long ProjLocId { get; set; }
    public string? LocationName { get; set; }
    public long ProjProcessId { get; set; }
    public string? ProcessName { get; set; }
    public DateTime ProjStartDate { get; set; }
    public DateTime ProjEndDate { get; set; }
    public DateTime ProjEstEndDate { get; set; }
    public string ProjStatus { get; set; } = null!;
    public int ProjRevNo { get; set; }
    public int ProjVerNo { get; set; }
    public long? ProjObjId { get; set; }
    public string? ProjObjDesc { get; set; }
    public string? ProjTargetProd { get; set; }
    public string? ProjTargetProdRem { get; set; }
    public string? ProjTargetSpecFile { get; set; }
    public string? ProjTargetSpecRem { get; set; }
    public string? ProjTargetYieldFile { get; set; }
    public string? ProjTargetYieldRem { get; set; }
    public string? ProjNotes { get; set; }
    public string? ProjActualProd { get; set; }
    public string? ProjActualProdRem { get; set; }
    public string? ProjActualSpecFile { get; set; }
    public string? ProjActualSpecRem { get; set; }
    public string? ProjActualYieldFile { get; set; }
    public string? ProjActualYieldRem { get; set; }
    public DateTime? ProjClsDate { get; set; }
    public string? ProjPlanFile { get; set; }
    public string? ProjPptxFile { get; set; }
    public List<ProjectMemberDto> Members { get; set; } = [];
    public List<ProjectStatusHistoryDto> StatusHistory { get; set; } = [];
}

public class ProjectMasterDto
{
    public long ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    public long ProjectCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime ProjectEffDate { get; set; }
    public DateTime? ProjectClsDate { get; set; }
    public decimal ProjectTeamId { get; set; }
    public string ProjectListAll { get; set; } = null!;
}

public class ProjectMemberDto
{
    public long ProjMemId { get; set; }
    public long ProjMemProjId { get; set; }
    public long ProjMemFuncId { get; set; }
    public string? FunctionName { get; set; }
    public long ProjMemEmpSysId { get; set; }
}

public class ProjectStatusHistoryDto
{
    public long ProjStatusId { get; set; }
    public long ProjStatusProjId { get; set; }
    public string? ProjStatusFile { get; set; }
    public DateTime ProjStatusDate { get; set; }
    public string ProjStatusRem { get; set; } = null!;
    public long ProjStatusRevNo { get; set; }
    public long ProjStatusVerNo { get; set; }
}

public class ProjectTypeMasterDto
{
    public long ProjTypeId { get; set; }
    public string ProjTypeName { get; set; } = null!;
    public string ProjTypeCode { get; set; } = null!;
    public decimal ProjTypeDepId { get; set; }
    public decimal ProjTypeCatId { get; set; }
    public string? CategoryName { get; set; }
    public List<ProjectTypeDeliverableMapDto> Deliverables { get; set; } = [];
    public List<ProjectTypeObjectiveMapDto> Objectives { get; set; } = [];
    public List<ProjectTypeScopeMapDto> Scopes { get; set; } = [];
}

public class ProjectTypeDeliverableMapDto
{
    public long DelId { get; set; }
    public string DelDesc { get; set; } = null!;
}

public class ProjectTypeObjectiveMapDto
{
    public long ObjId { get; set; }
    public string ObjDesc { get; set; } = null!;
}

public class ProjectTypeScopeMapDto
{
    public long ScopeId { get; set; }
    public string ScopeDesc { get; set; } = null!;
}

public class ProjectHoldDto
{
    public long ProjHoldId { get; set; }
    public long ProjHoldProjId { get; set; }
    public string ProjHoldType { get; set; } = null!;
    public DateTime ProjHoldDate { get; set; }
    public string ProjHoldReason { get; set; } = null!;
    public long ProjHoldUpdatedBy { get; set; }
    public DateTime ProjHoldUpdatedOn { get; set; }
}

public class ProjectApprovalDetailDto
{
    public long ProjApprId { get; set; }
    public long ProjApprProjId { get; set; }
    public string ProjApprType { get; set; } = null!;
    public DateTime ProjApprSentOn { get; set; }
    public long ProjAppEmpSysId { get; set; }
    public DateTime ProjApprAppDate { get; set; }
    public string ProjApprStatus { get; set; } = null!;
    public string ProjApprRemarks { get; set; } = null!;
}

public class ProjectLocationDto
{
    public long LocId { get; set; }
    public string LocName { get; set; } = null!;
}

public class ProjectProcessDto
{
    public long ProcId { get; set; }
    public string ProcName { get; set; } = null!;
}

public class ProjectDepartmentDto
{
    public decimal ProjDepId { get; set; }
    public string ProjDepName { get; set; } = null!;
    public string ProjDepCode { get; set; } = null!;
}

public class ProjectFunctionDto
{
    public long ProjFuncId { get; set; }
    public string ProjFuncName { get; set; } = null!;
}

public class ProjectCategoryDto
{
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public long CategoryTeamId { get; set; }
}

public class ProjectTypeCategoryDto
{
    public long ProjCatId { get; set; }
    public string ProjCatName { get; set; } = null!;
}
