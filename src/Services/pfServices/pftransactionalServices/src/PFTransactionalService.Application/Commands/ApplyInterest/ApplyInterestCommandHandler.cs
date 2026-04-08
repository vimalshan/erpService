using AutoMapper;
using MediatR;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Domain.Interfaces;

namespace PFTransactionalService.Application.Commands.ApplyInterest;

public class ApplyInterestCommandHandler : IRequestHandler<ApplyInterestCommand, PFAccumulationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApplyInterestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PFAccumulationDto> Handle(ApplyInterestCommand request, CancellationToken cancellationToken)
    {
        var accumulation = await _unitOfWork.Accumulations.GetByEmpSysIdAsync(request.EmpSysId, cancellationToken)
            ?? throw new KeyNotFoundException($"No active PF accumulation found for employee {request.EmpSysId}");

        accumulation.ApplyInterest(request.InterestAmount, request.ProcessedBy);

        await _unitOfWork.Accumulations.UpdateAsync(accumulation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PFAccumulationDto>(accumulation);
    }
}
