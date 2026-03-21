using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Commands;

public record CalculateDACommand : IRequest<DaSummaryDto>
{
    public long RequestNumber { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public string ArrangementType { get; init; } = "A";
    public string GradeCode { get; init; } = string.Empty;
}
