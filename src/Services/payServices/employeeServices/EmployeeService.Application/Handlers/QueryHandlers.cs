using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Queries;
using EmployeeService.Domain.Repositories;
using MediatR;

namespace EmployeeService.Application.Handlers.Queries;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetBySystemIdAsync(request.EmployeeSystemId, cancellationToken);
        return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
    }
}

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetAllEmployeesQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }
}

public class GetEmployeesByCostCenterQueryHandler : IRequestHandler<GetEmployeesByCostCenterQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeesByCostCenterQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EmployeeDto>> Handle(GetEmployeesByCostCenterQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetByCostCenterAsync(request.CostCenterId, cancellationToken);
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }
}

public class GetSalaryIncrementLogsQueryHandler : IRequestHandler<GetSalaryIncrementLogsQuery, IReadOnlyList<SalaryIncrementLogDto>>
{
    private readonly ISalaryIncrementLogRepository _repository;
    private readonly IMapper _mapper;

    public GetSalaryIncrementLogsQueryHandler(ISalaryIncrementLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SalaryIncrementLogDto>> Handle(GetSalaryIncrementLogsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<EmployeeService.Domain.Entities.SalaryIncrementLog> logs;
        
        if (request.EmployeeSystemId.HasValue)
        {
            logs = await _repository.GetByEmployeeIdAsync(request.EmployeeSystemId.Value, cancellationToken);
        }
        else
        {
            logs = await _repository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);
        }

        return _mapper.Map<IReadOnlyList<SalaryIncrementLogDto>>(logs);
    }
}

public class GetSalaryIncrementLogsByDateRangeQueryHandler : IRequestHandler<GetSalaryIncrementLogsByDateRangeQuery, IReadOnlyList<SalaryIncrementLogDto>>
{
    private readonly ISalaryIncrementLogRepository _repository;
    private readonly IMapper _mapper;

    public GetSalaryIncrementLogsByDateRangeQueryHandler(ISalaryIncrementLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SalaryIncrementLogDto>> Handle(GetSalaryIncrementLogsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
        return _mapper.Map<IReadOnlyList<SalaryIncrementLogDto>>(logs);
    }
}

public class GetEmployeeCTCHistoryQueryHandler : IRequestHandler<GetEmployeeCTCHistoryQuery, IReadOnlyList<SalaryIncrementLogDto>>
{
    private readonly ISalaryIncrementLogRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeeCTCHistoryQueryHandler(ISalaryIncrementLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SalaryIncrementLogDto>> Handle(GetEmployeeCTCHistoryQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByEmployeeIdAsync(request.EmployeeSystemId, cancellationToken);
        return _mapper.Map<IReadOnlyList<SalaryIncrementLogDto>>(logs);
    }
}

public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public SearchEmployeesQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<EmployeeDto>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync(cancellationToken);
        
        var result = employees.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            result = result.Where(e => 
                e.FirstName.ToLower().Contains(searchTerm) ||
                e.LastName.ToLower().Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm) ||
                e.EmployeeCode.ToLower().Contains(searchTerm)
            );
        }

        if (!string.IsNullOrWhiteSpace(request.EmploymentStatus))
        {
            result = result.Where(e => e.EmploymentStatus == request.EmploymentStatus);
        }

        var dtos = _mapper.Map<IReadOnlyList<EmployeeDto>>(result.ToList());
        return dtos;
    }
}
