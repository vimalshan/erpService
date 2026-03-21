namespace TourPlanService.Application.DTOs;

public sealed record TourPlanDto
{
    public string TpId { get; init; } = default!;
    public string TpEmpSysId { get; init; } = default!;
    public DateTime TpStartDate { get; init; }
    public DateTime? TpEndDate { get; init; }
    public string TpPurpose { get; init; } = default!;
    public string TpRemarks { get; init; } = default!;
    public string TpStatus { get; init; } = default!;
    public string TpCategory { get; init; } = default!;
    public string TpBookInc { get; init; } = default!;
    public string? TpType { get; init; }
    public string TpCreatedBy { get; init; } = default!;
    public DateTime TpCreatedOn { get; init; }
    public string? TpApprovedBy { get; init; }
    public DateTime? TpApprovedOn { get; init; }
    public string TpLastModifiedBy { get; init; } = default!;
    public DateTime TpLastModifiedOn { get; init; }
    public string TpFromCityId { get; init; } = default!;
    public string TpFromCityName { get; init; } = default!;
    public string TpToCityId { get; init; } = default!;
    public string TpToCityName { get; init; } = default!;
    public string TpSupRemarks { get; init; } = default!;
    public string? TpContactNo { get; init; }
    public string? TpGradeType { get; init; }
    public string? TpClaimType { get; init; }
    public string? TpAppRemarks { get; init; }
    public string? TpExpStatus { get; init; }
    public char? TpClosureStatus { get; init; }

    public IEnumerable<TourAdvanceDto> Advances { get; init; } = [];
    public IEnumerable<TourAgendaDto> Agendas { get; init; } = [];
    public IEnumerable<TourExpenseDto> Expenses { get; init; } = [];
}

public sealed record TourAdvanceDto
{
    public string AdvId { get; init; } = default!;
    public string AdvTpId { get; init; } = default!;
    public string AdvAmount { get; init; } = default!;
    public string AdvJvId { get; init; } = default!;
    public string AdvRemarks { get; init; } = default!;
    public string AdvAppStatus { get; init; } = default!;
    public string? AdvAppBy { get; init; }
    public DateTime? AdvAppOn { get; init; }
    public string AdvCurrency { get; init; } = default!;
    public string AdvRate { get; init; } = default!;
    public string AdvTotal { get; init; } = default!;
    public char? AdvType { get; init; }
    public string? AdvPayMode { get; init; }
}

public sealed record TourAgendaDto
{
    public string AgendaId { get; init; } = default!;
    public string AgendaTpId { get; init; } = default!;
    public string AgendaCity { get; init; } = default!;
    public string AgendaMeet { get; init; } = default!;
    public string AgendaOutcome { get; init; } = default!;
}

public sealed record TourExpenseDto
{
    public string TpExpId { get; init; } = default!;
    public string TpExpExpId { get; init; } = default!;
    public string TpExpCur { get; init; } = default!;
    public string TpExpExpAmt { get; init; } = default!;
    public string? TpExpRemarks { get; init; }
}

public sealed record ForexRequisitionDto
{
    public string ForReqId { get; init; } = default!;
    public string ForReqTpId { get; init; } = default!;
    public string ForReqPassNo { get; init; } = default!;
    public string ForReqPassName { get; init; } = default!;
    public string ForReqPassLocation { get; init; } = default!;
    public DateTime ForReqPassExpDate { get; init; }
    public string? ForReqDestination { get; init; }
    public string? ForReqStatus { get; init; }
    public string ForReqType { get; init; } = default!;
    public string? ForReqTotValue { get; init; }
    public string? ForReqCurrency { get; init; }
}

public sealed record TourPlanSummaryDto
{
    public string TpId { get; init; } = default!;
    public string TpEmpSysId { get; init; } = default!;
    public DateTime TpStartDate { get; init; }
    public DateTime? TpEndDate { get; init; }
    public string TpPurpose { get; init; } = default!;
    public string TpStatus { get; init; } = default!;
    public string TpCategory { get; init; } = default!;
    public string TpFromCityName { get; init; } = default!;
    public string TpToCityName { get; init; } = default!;
    public DateTime TpCreatedOn { get; init; }
}
