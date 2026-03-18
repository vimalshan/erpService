using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Entities;
using LoanDefinition.Domain.Repositories;
using LoanDefinition.SharedKernel;
using MediatR;

namespace LoanDefinition.Application.Features.Loans.Commands;

public class CreateLoanCommandHandler(ILoanMasterRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateLoanCommand, LoanMasterDto>
{
    public async Task<LoanMasterDto> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var entity = LoanMaster.Create(
            request.LoanId, request.LoanName, request.LoanPurpose, request.LoanTypeId,
            request.MinimumLimit, request.MaximumLimit, request.EffectiveDate,
            request.RecoveryType, request.CompoundingFactor, request.InterestFrequency,
            request.PrincipalRecoveryEdId, request.InterestRecoveryEdId, request.PrincipalPaymentEdId,
            request.CreatedBy);
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<LoanMasterDto>(entity);
    }
}

public class UpdateLoanCommandHandler(ILoanMasterRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateLoanCommand, LoanMasterDto>
{
    public async Task<LoanMasterDto> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new KeyNotFoundException($"Loan {request.LoanId} not found.");
        entity.Update(request.LoanName, request.LoanPurpose, request.MinimumLimit, request.MaximumLimit, request.ModifiedBy);
        await repository.UpdateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<LoanMasterDto>(entity);
    }
}

public class CloseLoanCommandHandler(ILoanMasterRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CloseLoanCommand, bool>
{
    public async Task<bool> Handle(CloseLoanCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanId, cancellationToken);
        if (entity is null) return false;
        entity.SetClosureDate(request.ClosureDate, request.ModifiedBy);
        await repository.UpdateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteLoanCommandHandler(ILoanMasterRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteLoanCommand, bool>
{
    public async Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanId, cancellationToken);
        if (entity is null) return false;
        await repository.DeleteAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
