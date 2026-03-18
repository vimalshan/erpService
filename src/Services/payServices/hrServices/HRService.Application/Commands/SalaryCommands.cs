using MediatR;

namespace HRService.Application.Commands;

public class UpdateEmployeeSalaryCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal NewSalary { get; set; }
}

public class CreateSalaryCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal TotalBaseSalary { get; set; }
}
