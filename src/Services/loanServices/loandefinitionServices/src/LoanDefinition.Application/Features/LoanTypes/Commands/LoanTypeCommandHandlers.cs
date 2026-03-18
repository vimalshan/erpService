using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Entities;
using LoanDefinition.Domain.Repositories;
using LoanDefinition.SharedKernel;
using MediatR;

namespace LoanDefinition.Application.Features.LoanTypes.Commands;

public class CreateLoanTypeCommandHandler(ILoanTypeMasterRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateLoanTypeCommand, LoanTypeMasterDto>
{
    public async Task<LoanTypeMasterDto> Handle(CreateLoanTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = LoanTypeMaster.Create(request.LoanType, request.LoanName, request.LoanCategory, request.CreatedBy);
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<LoanTypeMasterDto>(entity);
    }
}

public class UpdateLoanTypeCommandHandler(ILoanTypeMasterRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateLoanTypeCommand, LoanTypeMasterDto>
{
    public async Task<LoanTypeMasterDto> Handle(UpdateLoanTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanType, cancellationToken)
            ?? throw new KeyNotFoundException($"Loan type {request.LoanType} not found.");
        entity.Update(request.LoanName, request.LoanCategory, request.ModifiedBy);
        await repository.UpdateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<LoanTypeMasterDto>(entity);
    }
}

public class DeleteLoanTypeCommandHandler(ILoanTypeMasterRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteLoanTypeCommand, bool>
{
    public async Task<bool> Handle(DeleteLoanTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LoanType, cancellationToken);
        if (entity is null) return false;
        await repository.DeleteAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
