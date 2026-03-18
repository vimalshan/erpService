using MediatR;
using TaxService.Application.Commands;
using TaxService.Application.Common;
using TaxService.Application.DTOs;
using TaxService.Domain.Repositories;
using TaxService.Domain.ValueObjects;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace TaxService.Infrastructure.CommandHandlers;

public class CreateTaxMarginalDetailCommandHandler : IRequestHandler<CreateTaxMarginalDetailCommand, Result<TaxMarginalDetailDto>>
{
    private readonly ITaxMarginalDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTaxMarginalDetailCommandHandler> _logger;

    public CreateTaxMarginalDetailCommandHandler(
        ITaxMarginalDetailRepository repository,
        IMapper mapper,
        ILogger<CreateTaxMarginalDetailCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TaxMarginalDetailDto>> Handle(
        CreateTaxMarginalDetailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = Domain.Entities.TaxMarginalDetail.Create(
                request.Detail.EmployeeSystemId,
                request.Detail.FinancialYear,
                new Money(request.Detail.GrossIncome),
                new Money(request.Detail.StandardDeduction),
                request.UserId);

            await _repository.AddAsync(detail, cancellationToken);

            var dto = _mapper.Map<TaxMarginalDetailDto>(detail);
            return Result<TaxMarginalDetailDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tax marginal detail");
            return Result<TaxMarginalDetailDto>.Failure(ex.Message);
        }
    }
}

public class CalculateTaxCommandHandler : IRequestHandler<CalculateTaxCommand, Result<TaxMarginalDetailDto>>
{
    private readonly ITaxMarginalDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CalculateTaxCommandHandler> _logger;

    public CalculateTaxCommandHandler(
        ITaxMarginalDetailRepository repository,
        IMapper mapper,
        ILogger<CalculateTaxCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TaxMarginalDetailDto>> Handle(
        CalculateTaxCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await _repository.GetByIdAsync(request.TaxMarginalDetailId, cancellationToken);
            if (detail == null)
                return Result<TaxMarginalDetailDto>.Failure("Tax marginal detail not found");

            // Define tax rates based on financial year and regime
            var taxRates = GetDefaultTaxRates();
            detail.CalculateTax(taxRates);

            await _repository.UpdateAsync(detail, cancellationToken);

            var dto = _mapper.Map<TaxMarginalDetailDto>(detail);
            return Result<TaxMarginalDetailDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating tax");
            return Result<TaxMarginalDetailDto>.Failure(ex.Message);
        }
    }

    private List<TaxRate> GetDefaultTaxRates()
    {
        // Sample tax brackets for Indian financial year 2023-24
        return new List<TaxRate>
        {
            new TaxRate(0, 0, 250000),
            new TaxRate(5, 250001, 500000),
            new TaxRate(20, 500001, 1000000),
            new TaxRate(30, 1000001, decimal.MaxValue)
        };
    }
}

public class CreateConditionalMasterCommandHandler : IRequestHandler<CreateConditionalMasterCommand, Result<ConditionalMasterDto>>
{
    private readonly IConditionalMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateConditionalMasterCommandHandler> _logger;

    public CreateConditionalMasterCommandHandler(
        IConditionalMasterRepository repository,
        IMapper mapper,
        ILogger<CreateConditionalMasterCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ConditionalMasterDto>> Handle(
        CreateConditionalMasterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var master = Domain.Entities.ConditionalMaster.Create(
                request.Master.PayeeId,
                request.Master.PayeeName,
                request.Master.PayeeAddress,
                request.Master.PayeePAN,
                request.Master.TaxRegime,
                request.Master.FinancialYear,
                request.UserId);

            await _repository.AddAsync(master, cancellationToken);

            var dto = _mapper.Map<ConditionalMasterDto>(master);
            return Result<ConditionalMasterDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating conditional master");
            return Result<ConditionalMasterDto>.Failure(ex.Message);
        }
    }
}

public class AddExemptionCommandHandler : IRequestHandler<AddExemptionCommand, Result<ConditionalMasterDto>>
{
    private readonly IConditionalMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddExemptionCommandHandler> _logger;

    public AddExemptionCommandHandler(
        IConditionalMasterRepository repository,
        IMapper mapper,
        ILogger<AddExemptionCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ConditionalMasterDto>> Handle(
        AddExemptionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var master = await _repository.GetByIdAsync(request.Exemption.ConditionalMasterId, cancellationToken);
            if (master == null)
                return Result<ConditionalMasterDto>.Failure("Conditional master not found");

            var exemption = new Domain.Entities.TaxExemption
            {
                Code = request.Exemption.Code,
                Description = request.Exemption.Description,
                Amount = new Money(request.Exemption.Amount),
                EffectiveFrom = DateTime.UtcNow
            };

            master.AddExemption(exemption);
            await _repository.UpdateAsync(master, cancellationToken);

            var dto = _mapper.Map<ConditionalMasterDto>(master);
            return Result<ConditionalMasterDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding exemption");
            return Result<ConditionalMasterDto>.Failure(ex.Message);
        }
    }
}

public class AddDeductionCommandHandler : IRequestHandler<AddDeductionCommand, Result<ConditionalMasterDto>>
{
    private readonly IConditionalMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<AddDeductionCommandHandler> _logger;

    public AddDeductionCommandHandler(
        IConditionalMasterRepository repository,
        IMapper mapper,
        ILogger<AddDeductionCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ConditionalMasterDto>> Handle(
        AddDeductionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var master = await _repository.GetByIdAsync(request.Deduction.ConditionalMasterId, cancellationToken);
            if (master == null)
                return Result<ConditionalMasterDto>.Failure("Conditional master not found");

            var deduction = new Domain.Entities.TaxDeduction
            {
                Code = request.Deduction.Code,
                Description = request.Deduction.Description,
                Amount = new Money(request.Deduction.Amount),
                EffectiveFrom = DateTime.UtcNow
            };

            master.AddDeduction(deduction);
            await _repository.UpdateAsync(master, cancellationToken);

            var dto = _mapper.Map<ConditionalMasterDto>(master);
            return Result<ConditionalMasterDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding deduction");
            return Result<ConditionalMasterDto>.Failure(ex.Message);
        }
    }
}
