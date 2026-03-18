using AutoMapper;
using LoanDefinition.Application.DTOs;
using LoanDefinition.Domain.Entities;
using LoanDefinition.Domain.Repositories;
using LoanDefinition.SharedKernel;
using MediatR;

namespace LoanDefinition.Application.Features.Festivals.Commands;

public class CreateFestivalCommandHandler(ILoanFestivalRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateFestivalCommand, LoanFestivalDto>
{
    public async Task<LoanFestivalDto> Handle(CreateFestivalCommand request, CancellationToken cancellationToken)
    {
        var entity = LoanFestival.Create(request.FestivalId, request.Description, request.StartDate, request.EndDate, request.ModifiedBy);
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<LoanFestivalDto>(entity);
    }
}

public class UpdateFestivalCommandHandler(ILoanFestivalRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateFestivalCommand, LoanFestivalDto>
{
    public async Task<LoanFestivalDto> Handle(UpdateFestivalCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.FestivalId, cancellationToken)
            ?? throw new KeyNotFoundException($"Festival {request.FestivalId} not found.");
        entity.Update(request.Description, request.StartDate, request.EndDate, request.ModifiedBy);
        await repository.UpdateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<LoanFestivalDto>(entity);
    }
}

public class DeleteFestivalCommandHandler(ILoanFestivalRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteFestivalCommand, bool>
{
    public async Task<bool> Handle(DeleteFestivalCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.FestivalId, cancellationToken);
        if (entity is null) return false;
        await repository.DeleteAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
