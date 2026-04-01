using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.CreateLetRequest;

public class CreateLetRequestCommandHandler(ILetRequestRepository repository)
    : IRequestHandler<CreateLetRequestCommand, LetMainDto>
{
    public async Task<LetMainDto> Handle(CreateLetRequestCommand cmd, CancellationToken ct)
    {
        var letMain = LetMain.Create(
            cmd.RequestNumber,
            cmd.FinancialYearSerialNo,
            cmd.EmployeeUserId,
            cmd.SupervisorUserId,
            cmd.RequestDate);

        await repository.AddAsync(letMain, ct);

        return new LetMainDto(
            letMain.RequestNumber,
            letMain.FinancialYearSerialNo,
            letMain.EmployeeUserId,
            letMain.SupervisorUserId,
            letMain.RequestDate,
            []);
    }
}
