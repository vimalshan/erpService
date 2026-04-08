using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Aggregates;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Commands.ProcessContribution;

public class ProcessContributionCommandHandler : IRequestHandler<ProcessContributionCommand, PFAccumulationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProcessContributionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PFAccumulationDto> Handle(ProcessContributionCommand request, CancellationToken cancellationToken)
    {
        var accumulation = await _unitOfWork.Accumulations.GetByEmpSysIdAsync(request.EmpSysId, cancellationToken);

        if (accumulation is null)
        {
            var initialBalance = request.EmpContribution + request.ErContribution + request.VolContribution;
            accumulation = new PFAccumulation(
                request.EmpSysId,
                request.MemberNo,
                request.TrustCode,
                initialBalance,
                request.EmpContribution,
                request.ErContribution,
                request.ProcessedBy);

            await _unitOfWork.Accumulations.AddAsync(accumulation, cancellationToken);
        }
        else
        {
            accumulation.AddContribution(
                request.EmpContribution,
                request.ErContribution,
                request.VolContribution,
                request.TxnMonth,
                request.ProcessedBy);

            await _unitOfWork.Accumulations.UpdateAsync(accumulation, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<PFAccumulationDto>(accumulation);
    }
}
