using AutoMapper;
using MediatR;
using StipendService.Application.DTOs;
using StipendService.Domain.Exceptions;
using StipendService.Domain.Interfaces;

namespace StipendService.Application.Features.StipendDisbursement.Commands;

public class ProcessMonthlyStipendCommandHandler : IRequestHandler<ProcessMonthlyStipendCommand, ProcessMonthlyStipendResultDto>
{
    private readonly IStipendDisbursementRepository _repository;

    public ProcessMonthlyStipendCommandHandler(IStipendDisbursementRepository repository) => _repository = repository;

    public async Task<ProcessMonthlyStipendResultDto> Handle(ProcessMonthlyStipendCommand request, CancellationToken cancellationToken)
    {
        var disbursements = await _repository.GetByMonthYearAsync(request.MonthYear, cancellationToken);
        var draftItems = disbursements.Where(d => d.DisbursementStatus == "D").ToList();

        foreach (var disbursement in draftItems)
            disbursement.Process(request.ProcessedBy);

        await _repository.SaveChangesAsync(cancellationToken);

        return new ProcessMonthlyStipendResultDto(request.MonthYear, draftItems.Count, true,
            $"Successfully processed {draftItems.Count} records for {request.MonthYear}");
    }
}

public class CalculateAndDisburseStipendCommandHandler : IRequestHandler<CalculateAndDisburseStipendCommand, CalculateDisbursementResultDto>
{
    private readonly IStipendMasterRepository _stipendMasterRepository;
    private readonly IStipendDisbursementRepository _disbursementRepository;

    public CalculateAndDisburseStipendCommandHandler(
        IStipendMasterRepository stipendMasterRepository,
        IStipendDisbursementRepository disbursementRepository)
    {
        _stipendMasterRepository = stipendMasterRepository;
        _disbursementRepository = disbursementRepository;
    }

    public async Task<CalculateDisbursementResultDto> Handle(CalculateAndDisburseStipendCommand request, CancellationToken cancellationToken)
    {
        var activeMasters = (await _stipendMasterRepository.GetAllAsync(cancellationToken))
            .Where(m => m.IsActiveOn(DateTime.UtcNow))
            .ToList();

        var newDisbursements = new List<Domain.Entities.StipendDisbursement>();

        foreach (var master in activeMasters)
        {
            var exists = await _disbursementRepository.ExistsForMonthAsync(1, master.Id, request.MonthYear, cancellationToken);
            if (!exists)
            {
                var disbursement = Domain.Entities.StipendDisbursement.Create(
                    1, // SrfId placeholder
                    master.Id,
                    DateTime.UtcNow,
                    master.TotalStipend(),
                    request.MonthYear,
                    request.ProcessedBy);
                newDisbursements.Add(disbursement);
            }
        }

        if (newDisbursements.Count > 0)
            await _disbursementRepository.AddRangeAsync(newDisbursements, cancellationToken);

        await _disbursementRepository.SaveChangesAsync(cancellationToken);

        return new CalculateDisbursementResultDto(request.MonthYear, newDisbursements.Count, true,
            $"Created {newDisbursements.Count} disbursement records for {request.MonthYear}");
    }
}

public class RejectDisbursementCommandHandler : IRequestHandler<RejectDisbursementCommand, bool>
{
    private readonly IStipendDisbursementRepository _repository;

    public RejectDisbursementCommandHandler(IStipendDisbursementRepository repository) => _repository = repository;

    public async Task<bool> Handle(RejectDisbursementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new KeyNotFoundException($"Disbursement {request.DisbursementId} not found.");

        entity.Reject(request.UpdatedBy);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class SetBankReferenceCommandHandler : IRequestHandler<SetBankReferenceCommand, StipendDisbursementDto>
{
    private readonly IStipendDisbursementRepository _repository;
    private readonly IMapper _mapper;

    public SetBankReferenceCommandHandler(IStipendDisbursementRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StipendDisbursementDto> Handle(SetBankReferenceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.DisbursementId, cancellationToken)
            ?? throw new KeyNotFoundException($"Disbursement {request.DisbursementId} not found.");

        entity.SetBankReference(request.BankReference, request.ReferenceNo, request.UpdatedBy);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<StipendDisbursementDto>(entity);
    }
}
