namespace RequestServices.Domain.Entities;

/// <summary>Represents REQUEST_APP — approval records for a request line.</summary>
public class RequestApp
{
    public long   RequestId    { get; private set; }
    public long   SerialNumber { get; private set; }
    public DateTime ApprovalDate { get; private set; }
    public long   ApprovalNumber { get; private set; }
    public string ApprovalRemark { get; private set; } = default!;
    public string ApprovalUser  { get; private set; } = default!;

    private RequestApp() { }

    public static RequestApp Create(
        long requestId, long serialNumber,
        DateTime approvalDate, long approvalNumber,
        string approvalRemark, string approvalUser)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(approvalRemark);
        ArgumentException.ThrowIfNullOrWhiteSpace(approvalUser);

        return new RequestApp
        {
            RequestId      = requestId,
            SerialNumber   = serialNumber,
            ApprovalDate   = approvalDate,
            ApprovalNumber = approvalNumber,
            ApprovalRemark = approvalRemark,
            ApprovalUser   = approvalUser
        };
    }
}
