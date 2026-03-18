namespace RequestServices.Domain.Entities;

/// <summary>Represents REQUEST_MAIN — the header record for a training request.</summary>
public class RequestMain
{
    public long RequestId       { get; private set; }
    public string EmployeeUser  { get; private set; } = default!;
    public DateTime RequestDate { get; private set; }
    public string SupervisorUser{ get; private set; } = default!;

    // Navigation
    public ICollection<RequestSub> SubRequests { get; private set; } = new List<RequestSub>();

    private RequestMain() { }

    public static RequestMain Create(long requestId, string employeeUser, DateTime requestDate, string supervisorUser)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(employeeUser);
        ArgumentException.ThrowIfNullOrWhiteSpace(supervisorUser);

        return new RequestMain
        {
            RequestId      = requestId,
            EmployeeUser   = employeeUser,
            RequestDate    = requestDate,
            SupervisorUser = supervisorUser
        };
    }
}
