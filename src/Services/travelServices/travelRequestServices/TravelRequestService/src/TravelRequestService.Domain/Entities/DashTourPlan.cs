using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Entities;

public class DashTourPlan : BaseEntity
{
    public DateTime? TourDate { get; private set; }
    public string? Business { get; private set; }
    public string? Unit { get; private set; }
    public long? EmployeeSystemId { get; private set; }
    public string? EmployeeName { get; private set; }
    public string? Grade { get; private set; }
    public string? GradeCategory { get; private set; }
    public long? TourNumber { get; private set; }
    public decimal? ExpenseAmount { get; private set; }
    public string? Nature { get; private set; }
}
