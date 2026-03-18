using BankService.Domain.Interfaces;
using MediatR;

namespace BankService.Application.Commands.BankMasters;

public class UpdateBankMasterHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBankMasterCommand, bool>
{
    public async Task<bool> Handle(UpdateBankMasterCommand request, CancellationToken cancellationToken)
    {
        var bank = await unitOfWork.BankMasters.GetByCodeAsync(
            request.BankTrustCode, request.BankCode, cancellationToken);

        if (bank is null) return false;

        bank.UpdateBranchDetails(request.BranchName, request.BranchAddressLine1,
            request.BranchAddressLine2, request.BranchAddressLine3,
            request.BranchAddressLine4, request.BranchPhoneNo, request.BranchFaxNo);

        unitOfWork.BankMasters.Update(bank);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
