using AutoMapper;
using BankService.Application.DTOs;
using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.BankMasters;

public class CreateBankMasterHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateBankMasterCommand, BankMasterDto>
{
    public async Task<BankMasterDto> Handle(CreateBankMasterCommand request, CancellationToken cancellationToken)
    {
        var bank = BankMaster.Create(
            request.BankTrustCode, request.BankCode, request.BankName,
            request.MicrCode, request.BranchName, request.BranchAddressLine1,
            request.BranchEffDate);

        bank.UpdateBranchDetails(request.BranchName, request.BranchAddressLine1,
            request.BranchAddressLine2, request.BranchAddressLine3,
            request.BranchAddressLine4, request.BranchPhoneNo, request.BranchFaxNo);

        await unitOfWork.BankMasters.AddAsync(bank, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<BankMasterDto>(bank);
    }
}
