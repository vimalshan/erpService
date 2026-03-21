using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Commands;

public record CreateConveyanceCommand : IRequest<ConveyanceDto>
{
    public long RequestNumber { get; init; }
    public DateTime? Date { get; init; }
    public string? Particulars { get; init; }
    public long? Mode { get; init; }
    public long? Amount { get; init; }
}
