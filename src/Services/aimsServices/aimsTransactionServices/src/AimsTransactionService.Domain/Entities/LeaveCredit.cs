using AimsTransactionService.Domain.Common;

namespace AimsTransactionService.Domain.Entities;

public class LeaveCredit : Entity
{
    public long EmployeeSysId { get; private set; }
    public int LeaveId { get; private set; }
    public decimal CreditDays { get; private set; }
    public DateTime CreditDate { get; private set; }
    public string Remarks { get; private set; } = string.Empty;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private LeaveCredit() { }

    public static LeaveCredit Create(long id, long employeeSysId, int leaveId,
        decimal creditDays, string remarks, long createdBy)
    {
        return new LeaveCredit
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            LeaveId = leaveId,
            CreditDays = creditDays,
            CreditDate = DateTime.UtcNow,
            Remarks = remarks,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }
}
