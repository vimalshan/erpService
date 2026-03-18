using EximManagement.Domain.Common;

namespace EximManagement.Domain.Entities;

public class EximDataImport : BaseEntity
{
    public long DataId { get; set; }
    public DateTime? EximDate { get; set; }
    public long? HsCode { get; set; }
    public string? ProdDesc { get; set; }
    public string? PortDest { get; set; }
    public string? CountryOrg { get; set; }
    public decimal? StdQty { get; set; }
    public string? StdUnit { get; set; }
    public decimal? StdUnitRate { get; set; }
    public decimal? UnitRateDol { get; set; }
    public decimal? FobInr { get; set; }
    public decimal? FobDol { get; set; }
    public string? ApplicableDutyInr { get; set; }
    public string? ModeShip { get; set; }
    public string? RecordId { get; set; }
    public string? EMonth { get; set; }
    public long? FileId { get; set; }
    public string? ImpName { get; set; }
    public string? ImpAdd1 { get; set; }
    public string? ImpAdd2 { get; set; }
    public string? ImpCity { get; set; }
    public string? ImpPinCode { get; set; }
    public string? ImpState { get; set; }
    public string? ImpPhone { get; set; }
    public string? ImpEmail { get; set; }
    public string? ImpContactPer { get; set; }
    public string? ExpName { get; set; }
    public string? ExpAdd1 { get; set; }
    public decimal? Qty { get; set; }
    public string? Unit { get; set; }
    public string? UnitRateInr { get; set; }
    public string? UnitPriceFc { get; set; }
    public string? ActualDutyInr { get; set; }
    public string? AvadInr { get; set; }
    public string? AvadUsd { get; set; }
    public string? PortOrg { get; set; }
    public string? ChaPanNo { get; set; }
    public string? ChaName { get; set; }
    public string? Ag { get; set; }
    public string? Iec { get; set; }
    public string? BeNo { get; set; }
    public string? InvNo { get; set; }
    public string? ItemNo { get; set; }
    public string? Hs2 { get; set; }
    public string? Hs4 { get; set; }
    public string? HsDesc { get; set; }
    public string? InvValue { get; set; }
    public DateTime? InvDate { get; set; }
    public string? PossibleDuplicate { get; set; }
}
