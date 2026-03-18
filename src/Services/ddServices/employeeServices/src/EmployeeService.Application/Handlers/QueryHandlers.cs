using AutoMapper;
using EmployeeService.Application.Queries.Employees;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Application.Handlers.Queries
{
    /// <summary>
    /// Handler for GetEmployeeByIdQuery
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
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    return null;

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Handler for GetEmployeeByNumberQuery
    /// </summary>
    public class GetEmployeeByNumberQueryHandler : IRequestHandler<GetEmployeeByNumberQuery, EmployeeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeByNumberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByNumberQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByEmployeeNumberAsync(request.EmployeeNumber);
                if (employee == null)
                    return null;

                return _mapper.Map<EmployeeDto>(employee);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Handler for GetAllActiveEmployeesQuery
    /// </summary>
    public class GetAllActiveEmployeesQueryHandler : IRequestHandler<GetAllActiveEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllActiveEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(GetAllActiveEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetActiveEmployeesAsync();
                return _mapper.Map<List<EmployeeDto>>(employees);
            }
            catch
            {
                return new List<EmployeeDto>();
            }
        }
    }

    /// <summary>
    /// Handler for GetEmployeesByUnitQuery
    /// </summary>
    public class GetEmployeesByUnitQueryHandler : IRequestHandler<GetEmployeesByUnitQuery, List<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeesByUnitQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesByUnitQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetEmployeesByUnitAsync(request.UnitId);
                return _mapper.Map<List<EmployeeDto>>(employees);
            }
            catch
            {
                return new List<EmployeeDto>();
            }
        }
    }

    /// <summary>
    /// Handler for GetEmployeesByGradeQuery
    /// </summary>
    public class GetEmployeesByGradeQueryHandler : IRequestHandler<GetEmployeesByGradeQuery, List<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeesByGradeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesByGradeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetEmployeesByGradeAsync(request.GradeCode);
                return _mapper.Map<List<EmployeeDto>>(employees);
            }
            catch
            {
                return new List<EmployeeDto>();
            }
        }
    }

    /// <summary>
    /// Handler for SearchEmployeesQuery
    /// </summary>
    public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _unitOfWork.Employees.SearchEmployeesAsync(request.SearchTerm);
                return _mapper.Map<List<EmployeeDto>>(employees);
            }
            catch
            {
                return new List<EmployeeDto>();
            }
        }
    }

    /// <summary>
    /// Handler for GetEmployeeStatisticsQuery
    /// </summary>
    public class GetEmployeeStatisticsQueryHandler : IRequestHandler<GetEmployeeStatisticsQuery, EmployeeStatisticsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeStatisticsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeStatisticsDto> Handle(GetEmployeeStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _unitOfWork.Employees.GetAllAsync();
                var activeEmployees = await _unitOfWork.Employees.GetActiveEmployeesAsync();

                return new EmployeeStatisticsDto
                {
                    TotalEmployees = employees.Count(),
                    ActiveEmployees = activeEmployees.Count(),
                    TerminatedEmployees = employees.Count(e => e.IsTerminated),
                    AverageSalary = employees.Average(e => e.SalaryInfo.BasicSalary),
                    EmployeesByGrade = employees.GroupBy(e => e.GradeInfo.GradeCode)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    EmployeesByUnit = employees.GroupBy(e => e.OrganizationalAssignment.Unit)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    EmployeesByDesignation = employees.GroupBy(e => e.OrganizationalAssignment.Designation)
                        .ToDictionary(g => g.Key, g => g.Count())
                };
            }
            catch
            {
                return new EmployeeStatisticsDto();
            }
        }
    }

    /// <summary>
    /// Handler for GetEmployeeWithDetailsQuery
    /// </summary>
    public class GetEmployeeWithDetailsQueryHandler : IRequestHandler<GetEmployeeWithDetailsQuery, EmployeeDetailedDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmployeeWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDetailedDto> Handle(GetEmployeeWithDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetEmployeeWithAllDetailsAsync(request.EmployeeId);
                if (employee == null)
                    return null;

                var detailedDto = _mapper.Map<EmployeeDetailedDto>(employee);
                detailedDto.Appraisals = _mapper.Map<List<AppraisalSummaryDto>>(employee.Appraisals);
                detailedDto.CareerPlans = _mapper.Map<List<CareerPlanSummaryDto>>(employee.CareerPlans);
                detailedDto.Benefits = _mapper.Map<List<BenefitSummaryDto>>(employee.Benefits);

                return detailedDto;
            }
            catch
            {
                return null;
            }
        }
    }
}
