namespace MamAllocationService.Application.DTOs;

public record AllocationDetailDto
{
    public DateTime AllDate { get; init; }
    public int AllRm { get; init; }
    public decimal? AllEntOpen { get; init; }
    public decimal? AllFormIvdDf { get; init; }
    public decimal? AllFormIvIdF { get; init; }
    public decimal? AllFormIvIdP { get; init; }
    public decimal? AllFormIvDdP { get; init; }
    public decimal? AllFormIvIdFWo { get; init; }
    public decimal? AllFormIvDdFWo { get; init; }
    public decimal? AllClosedDf { get; init; }
    public decimal? AllCloseIdF { get; init; }
    public decimal? AllCloseIdP { get; init; }
    public decimal? AllCloseDdP { get; init; }
    public decimal? AllEntDebit { get; init; }
    public decimal? AllProdEntDebit { get; init; }
    public decimal? AllDispEntCredit { get; init; }
    public decimal? AllNetEnt { get; init; }
    public decimal? AllAddDdf { get; init; }
    public decimal? AllAddIdF { get; init; }
    public decimal? AllAddIdP { get; init; }
    public decimal? AllAddDdP { get; init; }
    public decimal? AllProd { get; init; }
    public decimal? AllCons { get; init; }
    public decimal? AllRg1Ddf { get; init; }
    public decimal? AllRg1Ddp { get; init; }
    public decimal? AllCloseRg1Ddf { get; init; }
    public decimal? AllCloseRg1Ddp { get; init; }
    public decimal? AllSaleFormIvIdP { get; init; }
    public decimal? AllSaleFormIvDdP { get; init; }
    public decimal? AllSaleRg1Ddp { get; init; }
    public decimal? AllSale { get; init; }
    public decimal? AllAddRgDdf { get; init; }
    public decimal? AllAddRgDdp { get; init; }
}

public record AllocationSummaryDto
{
    public DateTime AllDate { get; init; }
    public int AllRm { get; init; }
    public decimal? AllProd { get; init; }
    public decimal? AllCons { get; init; }
    public decimal? AllSale { get; init; }
}

public record AllocationProdDetailDto
{
    public DateTime? AllDate { get; init; }
    public long? AllSrl { get; init; }
    public int? AllFg { get; init; }
    public decimal? DdfQty { get; init; }
    public decimal? DdpQty { get; init; }
    public decimal? PrdQty { get; init; }
    public decimal? AllRm { get; init; }
}

public record AllocationFgDto
{
    public DateTime? AllDate { get; init; }
    public long? FgCode { get; init; }
    public int? DomDispatch { get; init; }
    public decimal? ExpDispatch { get; init; }
    public decimal? DutyFree { get; init; }
    public decimal? DutyPaid { get; init; }
}

public record ArrivalDetailDto
{
    public long? ArrivalNo { get; init; }
    public DateTime? ArrivalDate { get; init; }
    public decimal? ArrivalQty { get; init; }
    public int? ArrivalItem { get; init; }
    public decimal? ArrivalReceiptNo { get; init; }
}

public record ConsumptionDetailDto
{
    public long? ConsumptionNo { get; init; }
    public DateTime? ConsumptionDate { get; init; }
    public int? ConsumptionRm { get; init; }
    public decimal? ConsumptionQty { get; init; }
}

public record DispatchDetailDto
{
    public decimal? DispatchNo { get; init; }
    public DateTime? DispatchDate { get; init; }
    public int? DispatchFg { get; init; }
    public decimal? DispatchQty { get; init; }
    public string? DispatchType { get; init; }
    public DateTime? DispatchAreDate { get; init; }
    public string? DispatchInvoiceNo { get; init; }
    public long? DispatchAdvNo { get; init; }
}

public record FgAllocationDto
{
    public long? Sno { get; init; }
    public long? FgCode { get; init; }
    public string? Flag { get; init; }
}

public record ProductAllocationDto
{
    public long? Sno { get; init; }
    public long? RmCode { get; init; }
}
