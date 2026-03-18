using MasterService.Domain.Common;
using MasterService.Domain.Events;

namespace MasterService.Domain.Entities;

/// <summary>Aggregate: TRAIN_MAST</summary>
public sealed class TrainingProvider : AggregateRoot
{
    public long TrainingCode { get; private set; }
    public string TrainingName { get; private set; } = string.Empty;
    public string? Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string? Address3 { get; private set; }
    public string? Address4 { get; private set; }
    public string? ContactName1 { get; private set; }
    public string? ContactName2 { get; private set; }
    public string? Remark { get; private set; }
    public string? PhoneNum1 { get; private set; }
    public string? PhoneNum2 { get; private set; }
    public string? FaxNum1 { get; private set; }
    public string? FaxNum2 { get; private set; }
    public string? EmailAddress1 { get; private set; }
    public string? EmailAddress2 { get; private set; }
    public long? GroupCode { get; private set; }
    public decimal? VendorRating { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public string? CancelRemark { get; private set; }
    public string? BrochureFilePath { get; private set; }
    public string? VendorExpiry { get; private set; }

    private TrainingProvider() { }

    public static TrainingProvider Create(long trainingCode, string trainingName,
        string? address1 = null, string? contactName = null, string? phoneNum = null, long? groupCode = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(trainingName);
        if (trainingCode <= 0) throw new ArgumentException("TrainingCode must be positive.", nameof(trainingCode));

        var tp = new TrainingProvider
        {
            TrainingCode = trainingCode,
            TrainingName = trainingName.Trim(),
            Address1 = address1,
            ContactName1 = contactName,
            PhoneNum1 = phoneNum,
            GroupCode = groupCode,
            EffectiveDate = DateTime.UtcNow
        };

        tp.AddDomainEvent(new TrainingProviderCreatedEvent(tp.TrainingCode, tp.TrainingName));
        return tp;
    }

    public void UpdateContact(string? contactName1, string? phoneNum1, string? emailAddress1)
    {
        ContactName1 = contactName1;
        PhoneNum1 = phoneNum1;
        EmailAddress1 = emailAddress1;
    }

    public void UpdateBrochure(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        BrochureFilePath = filePath;
    }

    public void Cancel(string? cancelRemark = null)
    {
        CancelDate = DateTime.UtcNow;
        CancelRemark = cancelRemark;
    }

    public bool IsActive => CancelDate is null;
}
