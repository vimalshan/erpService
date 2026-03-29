using HealthTransaction.Domain.Common;
using HealthTransaction.Domain.Events;

namespace HealthTransaction.Domain.Entities;

/// <summary>Maps HLTH_CHKUP_CARD — Health checkup card (aggregate root)</summary>
public class CheckupCard : BaseEntity
{
    public decimal HlthNum { get; set; }           // HCC_HLTH_NUM (PK)
    public decimal EmpNum { get; set; }            // HCC_EMP_NUM
    public DateTime? EmpDate { get; set; }         // HCC_EMP_DATE
    public string? ComCode { get; set; }           // HCC_COM_COD
    public string? PersonalDetails { get; set; }   // HCC_PER_DET
    public string? ComplaintDetails { get; set; }  // HCC_COMPL_DET
    public string? AdvRemark1 { get; set; }        // HCC_ADV_RMK1
    public string? AdvRemark2 { get; set; }        // HCC_ADV_RMK2
    public DateTime? DocDate1 { get; set; }        // HCC_DOC_DATE1
    public DateTime? DocDate2 { get; set; }        // HCC_DOC_DATE2
    public string? AdvFollow1 { get; set; }        // HCC_ADV_FOLLOW1
    public string? AdvFollow2 { get; set; }        // HCC_ADV_FOLLOW2

    public ICollection<CheckupCardSub> SubRecords { get; set; } = new List<CheckupCardSub>();

    public void RaiseCreatedEvent() => AddDomainEvent(new CheckupCardCreatedEvent(this));
}
