using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HRService.Application.Queries;
using HRService.Application.DTOs;
using HRService.Infrastructure.Repositories;

namespace HRService.Application.Handlers;

/// <summary>
/// Query handlers for employee operations
/// </summary>
public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
            throw new KeyNotFoundException($"Employee {request.EmployeeId} not found");

        return _mapper.Map<EmployeeDto>(employee);
    }
}

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _unitOfWork.EmployeeRepository.GetAllAsync(cancellationToken);
        
        var paginated = employees
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return _mapper.Map<List<EmployeeDto>>(paginated);
    }
}

public class GetEmployeesByDepartmentQueryHandler : IRequestHandler<GetEmployeesByDepartmentQuery, List<EmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeesByDepartmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeesByDepartmentQuery request, CancellationToken cancellationToken)
    {
        var employees = await _unitOfWork.EmployeeRepository.GetAllAsync(cancellationToken);
        
        var departmentEmployees = employees
            .Where(e => e.DepartmentId == request.DepartmentId)
            .ToList();

        return _mapper.Map<List<EmployeeDto>>(departmentEmployees);
    }
}

public class GetEmployeesByStatusQueryHandler : IRequestHandler<GetEmployeesByStatusQuery, List<EmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeesByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeesByStatusQuery request, CancellationToken cancellationToken)
    {
        var employees = await _unitOfWork.EmployeeRepository.GetAllAsync(cancellationToken);
        
        var statusEmployees = employees
            .Where(e => e.Status.ToString() == request.Status)
            .ToList();

        return _mapper.Map<List<EmployeeDto>>(statusEmployees);
    }
}
