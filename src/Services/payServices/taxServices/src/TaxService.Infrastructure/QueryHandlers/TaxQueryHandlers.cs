using MediatR;
using TaxService.Application.Common;
using TaxService.Application.DTOs;
using TaxService.Application.Queries;
using TaxService.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace TaxService.Infrastructure.QueryHandlers;

public class GetTaxMarginalDetailByIdQueryHandler 
    : IRequestHandler<GetTaxMarginalDetailByIdQuery, Result<TaxMarginalDetailDto>>
{
    private readonly ITaxMarginalDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTaxMarginalDetailByIdQueryHandler> _logger;

    public GetTaxMarginalDetailByIdQueryHandler(
        ITaxMarginalDetailRepository repository,
        IMapper mapper,
        ILogger<GetTaxMarginalDetailByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TaxMarginalDetailDto>> Handle(
        GetTaxMarginalDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (detail == null)
                return Result<TaxMarginalDetailDto>.Failure("Tax marginal detail not found");

            var dto = _mapper.Map<TaxMarginalDetailDto>(detail);
            return Result<TaxMarginalDetailDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tax marginal detail");
            return Result<TaxMarginalDetailDto>.Failure(ex.Message);
        }
    }
}

public class GetTaxByEmployeeAndYearQueryHandler 
    : IRequestHandler<GetTaxByEmployeeAndYearQuery, Result<TaxMarginalDetailDto>>
{
    private readonly ITaxMarginalDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTaxByEmployeeAndYearQueryHandler> _logger;

    public GetTaxByEmployeeAndYearQueryHandler(
        ITaxMarginalDetailRepository repository,
        IMapper mapper,
        ILogger<GetTaxByEmployeeAndYearQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<TaxMarginalDetailDto>> Handle(
        GetTaxByEmployeeAndYearQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await _repository.GetByEmployeeAndYearAsync(
                request.EmployeeSystemId,
                request.FinancialYear,
                cancellationToken);

            if (detail == null)
                return Result<TaxMarginalDetailDto>.Failure("Tax record not found");

            var dto = _mapper.Map<TaxMarginalDetailDto>(detail);
            return Result<TaxMarginalDetailDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tax by employee and year");
            return Result<TaxMarginalDetailDto>.Failure(ex.Message);
        }
    }
}

public class GetEmployeeTaxDetailsQueryHandler 
    : IRequestHandler<GetEmployeeTaxDetailsQuery, Result<IEnumerable<TaxMarginalDetailDto>>>
{
    private readonly ITaxMarginalDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEmployeeTaxDetailsQueryHandler> _logger;

    public GetEmployeeTaxDetailsQueryHandler(
        ITaxMarginalDetailRepository repository,
        IMapper mapper,
        ILogger<GetEmployeeTaxDetailsQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<TaxMarginalDetailDto>>> Handle(
        GetEmployeeTaxDetailsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var details = await _repository.GetByEmployeeAsync(request.EmployeeSystemId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<TaxMarginalDetailDto>>(details);
            return Result<IEnumerable<TaxMarginalDetailDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee tax details");
            return Result<IEnumerable<TaxMarginalDetailDto>>.Failure(ex.Message);
        }
    }
}

public class GetConditionalMasterByIdQueryHandler 
    : IRequestHandler<GetConditionalMasterByIdQuery, Result<ConditionalMasterDto>>
{
    private readonly IConditionalMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetConditionalMasterByIdQueryHandler> _logger;

    public GetConditionalMasterByIdQueryHandler(
        IConditionalMasterRepository repository,
        IMapper mapper,
        ILogger<GetConditionalMasterByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ConditionalMasterDto>> Handle(
        GetConditionalMasterByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var master = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (master == null)
                return Result<ConditionalMasterDto>.Failure("Conditional master not found");

            var dto = _mapper.Map<ConditionalMasterDto>(master);
            return Result<ConditionalMasterDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving conditional master");
            return Result<ConditionalMasterDto>.Failure(ex.Message);
        }
    }
}

public class GetConditionalMasterByPayeeIdQueryHandler 
    : IRequestHandler<GetConditionalMasterByPayeeIdQuery, Result<ConditionalMasterDto>>
{
    private readonly IConditionalMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetConditionalMasterByPayeeIdQueryHandler> _logger;

    public GetConditionalMasterByPayeeIdQueryHandler(
        IConditionalMasterRepository repository,
        IMapper mapper,
        ILogger<GetConditionalMasterByPayeeIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<ConditionalMasterDto>> Handle(
        GetConditionalMasterByPayeeIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var master = await _repository.GetByPayeeIdAsync(
                request.PayeeId,
                request.FinancialYear,
                cancellationToken);

            if (master == null)
                return Result<ConditionalMasterDto>.Failure("Conditional master not found");

            var dto = _mapper.Map<ConditionalMasterDto>(master);
            return Result<ConditionalMasterDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving conditional master by payee ID");
            return Result<ConditionalMasterDto>.Failure(ex.Message);
        }
    }
}

public class GetActiveConditionalMastersQueryHandler 
    : IRequestHandler<GetActiveConditionalMastersQuery, Result<IEnumerable<ConditionalMasterDto>>>
{
    private readonly IConditionalMasterRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetActiveConditionalMastersQueryHandler> _logger;

    public GetActiveConditionalMastersQueryHandler(
        IConditionalMasterRepository repository,
        IMapper mapper,
        ILogger<GetActiveConditionalMastersQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ConditionalMasterDto>>> Handle(
        GetActiveConditionalMastersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var masters = await _repository.GetActiveAsync(request.FinancialYear, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<ConditionalMasterDto>>(masters);
            return Result<IEnumerable<ConditionalMasterDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active conditional masters");
            return Result<IEnumerable<ConditionalMasterDto>>.Failure(ex.Message);
        }
    }
}
