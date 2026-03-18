using EximManagement.Domain.Common;

namespace EximManagement.Domain.Entities;

public class EximDataExport : BaseEntity
{
    public long DataId { get; set; }
    public DateTime? EximDate { get; set; }
    public long? HsCode { get; set; }
    public string? ProdDesc { get; set; }
    public string? PortDest { get; set; }
    public string? CountryDest { get; set; }
    public string? PortOrigin { get; set; }
    public long? StdQty { get; set; }
    public string? StdUnit { get; set; }
    public decimal? StdUnitRate { get; set; }
    public long? UnitRateDol { get; set; }
    public long? FobInr { get; set; }
    public long? FobDol { get; set; }
    public string? ModeShip { get; set; }
    public string? RecordId { get; set; }
    public string? EMonth { get; set; }
    public long? FileId { get; set; }
    public string? ExpName { get; set; }
    public string? ExpAdd1 { get; set; }
    public string? ExpAdd2 { get; set; }
    public string? ExpCity { get; set; }
    public string? ExpState { get; set; }
    public string? ImpName { get; set; }
    public string? ImpAdd1 { get; set; }
    public string? ImpAdd2 { get; set; }
    public string? ImpCountry { get; set; }
    public long? Qty { get; set; }
    public string? Unit { get; set; }
    public string? UnitRateInr { get; set; }
    public string? UnitRateFc { get; set; }
    public string? ValueFc { get; set; }
    public string? Iec { get; set; }
    public string? SbNo { get; set; }
    public string? InvNo { get; set; }
    public string? ItemNo { get; set; }
    public string? DrawBack { get; set; }
    public string? CurrentQue { get; set; }
    public string? Hs2 { get; set; }
    public string? Hs4 { get; set; }
    public string? InvSlNo { get; set; }
    public string? ChallanNo { get; set; }
    public string? HsDesc { get; set; }
    public string? ChaPanNo { get; set; }
    public string? ChaName { get; set; }
    public DateTime? InvDate { get; set; }
}
