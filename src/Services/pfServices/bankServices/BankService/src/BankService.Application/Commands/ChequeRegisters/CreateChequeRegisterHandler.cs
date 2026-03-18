using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.ChequeRegisters;

public class CreateChequeRegisterHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateChequeRegisterCommand, ChequeRegisterDto>
{
    public async Task<ChequeRegisterDto> Handle(CreateChequeRegisterCommand request, CancellationToken cancellationToken)
    {
        var register = ChequeRegister.Create(
            request.ChequeNoFrom, request.ChequeNoTo,
            request.ChequeBookId, request.AccountId, request.IssuedDate);

        await unitOfWork.ChequeRegisters.AddAsync(register, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ChequeRegisterDto>(register);
    }
}
