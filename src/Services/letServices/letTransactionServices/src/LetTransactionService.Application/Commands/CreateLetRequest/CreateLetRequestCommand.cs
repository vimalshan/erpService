using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Commands.CreateLetRequest;

public record CreateLetRequestCommand(
    long RequestNumber,
    int FinancialYearSerialNo,
    string EmployeeUserId,
    string? SupervisorUserId,
    DateTime? RequestDate
) : IRequest<LetMainDto>;
