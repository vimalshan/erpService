using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineIssues.Commands;
using MedicineManagement.Application.Features.MedicineIssues.Queries;
using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.MedicineIssues.Handlers;

public class GetIssuesByVisitHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetIssuesByVisitQuery, IReadOnlyList<MedicineIssueDto>>
{
    public async Task<IReadOnlyList<MedicineIssueDto>> Handle(GetIssuesByVisitQuery request, CancellationToken ct)
    {
        var issues = await unitOfWork.MedicineIssues.GetByVisitNumberAsync(request.VisitNumber, ct);
        return mapper.Map<IReadOnlyList<MedicineIssueDto>>(issues);
    }
}

public class GetIssuesByMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetIssuesByMedicineQuery, IReadOnlyList<MedicineIssueDto>>
{
    public async Task<IReadOnlyList<MedicineIssueDto>> Handle(GetIssuesByMedicineQuery request, CancellationToken ct)
    {
        var issues = await unitOfWork.MedicineIssues.GetByMedicineCodeAsync(request.MedicineCode, ct);
        return mapper.Map<IReadOnlyList<MedicineIssueDto>>(issues);
    }
}

public class CreateMedicineIssueHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateMedicineIssueCommand, MedicineIssueDto>
{
    public async Task<MedicineIssueDto> Handle(CreateMedicineIssueCommand request, CancellationToken ct)
    {
        var entity = MedicineIssue.Create(
            request.CompanyCode, request.TransactionNumber, request.MedicineCode,
            request.IssuedQuantity, request.VisitNumber,
            request.EntryUser, request.EntryUserPin);
        await unitOfWork.MedicineIssues.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<MedicineIssueDto>(entity);
    }
}
