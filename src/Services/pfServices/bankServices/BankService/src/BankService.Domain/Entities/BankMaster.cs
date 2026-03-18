using BankService.Domain.Common;

namespace BankService.Domain.Entities;

public class BankMaster : AggregateRoot
{
    public string BankTrustCode { get; private set; } = null!;
    public string BankCode { get; private set; } = null!;
    public string BankName { get; private set; } = null!;
    public string MicrCode { get; private set; } = null!;
    public string BranchName { get; private set; } = null!;
    public string BranchAddressLine1 { get; private set; } = null!;
    public string? BranchAddressLine2 { get; private set; }
    public string? BranchAddressLine3 { get; private set; }
    public string? BranchAddressLine4 { get; private set; }
    public string? BranchPhoneNo { get; private set; }
    public string? BranchFaxNo { get; private set; }
    public DateTime BranchEffDate { get; private set; }
    public DateTime? BranchClsDate { get; private set; }
    public string BranchStatus { get; private set; } = "A";

    private BankMaster() { }

    public static BankMaster Create(
        string bankTrustCode, string bankCode, string bankName,
        string micrCode, string branchName, string branchAddressLine1,
        DateTime branchEffDate)
    {
        var bank = new BankMaster
        {
            BankTrustCode = bankTrustCode,
            BankCode = bankCode,
            BankName = bankName,
            MicrCode = micrCode,
            BranchName = branchName,
            BranchAddressLine1 = branchAddressLine1,
            BranchEffDate = branchEffDate,
            BranchStatus = "A"
        };

        bank.AddDomainEvent(new Events.BankCreatedEvent(bankTrustCode, bankCode));
        return bank;
    }

    public void UpdateBranchDetails(string branchName, string address1, string? address2,
        string? address3, string? address4, string? phone, string? fax)
    {
        BranchName = branchName;
        BranchAddressLine1 = address1;
        BranchAddressLine2 = address2;
        BranchAddressLine3 = address3;
        BranchAddressLine4 = address4;
        BranchPhoneNo = phone;
        BranchFaxNo = fax;
    }

    public void Close(DateTime closingDate)
    {
        BranchClsDate = closingDate;
        BranchStatus = "C";
    }

    public void Activate()
    {
        BranchClsDate = null;
        BranchStatus = "A";
    }
}
