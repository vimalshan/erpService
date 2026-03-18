using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Exceptions;
using ContributionService.Domain.Interfaces;
using MediatR;

namespace ContributionService.Application.Commands.ContributionDetail;

public class CreateContributionDetailHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateContributionDetailCommand, ContributionDetailDto>
{
    public async Task<ContributionDetailDto> Handle(CreateContributionDetailCommand request, CancellationToken ct)
    {
        var details = await uow.ContributionDetails.GetByBatchNoAsync(request.BatchNo, ct);
        var nextId = details.Count > 0 ? details.Max(d => d.ContributionId) + 1 : 1;

        var entity = Domain.Entities.ContributionDetail.Create(
            request.BatchNo, nextId, request.MemberNo, request.UnitCode,
            request.EmployeeNo, request.BasicAmount, request.FpsBasicAmount,
            request.EeAmount, request.ErAmount, request.VeAmount, request.FpAmount,
            request.LoanPrincipal, request.LoanInterest,
            request.EntByUserId, request.EntEmpSysId, request.TypeCode);

        await uow.ContributionDetails.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<ContributionDetailDto>(entity);
    }
}

public class ValidateContributionDetailHandler(IUnitOfWork uow)
    : IRequestHandler<ValidateContributionDetailCommand, string>
{
    public async Task<string> Handle(ValidateContributionDetailCommand request, CancellationToken ct)
    {
        var detail = await uow.ContributionDetails.GetByIdAsync(request.ContributionId, ct)
            ?? throw new ContributionNotFoundException("ContributionDetail", request.ContributionId);

        detail.Validate();
        await uow.ContributionDetails.UpdateAsync(detail, ct);
        await uow.SaveChangesAsync(ct);
        return "Contribution validation successful";
    }
}
