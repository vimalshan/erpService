using MediatR;
using PayrollServices.Application.Commands;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Interfaces;
using AutoMapper;

namespace PayrollServices.Application.Services;

/// <summary>
/// Handler for ProcessMonthlySalaryCommand
/// </summary>
public class ProcessMonthlySalaryCommandHandler : IRequestHandler<ProcessMonthlySalaryCommand, ProcessMonthlySalaryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ProcessMonthlySalaryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<ProcessMonthlySalaryResult> Handle(ProcessMonthlySalaryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Implementation will be added in infrastructure layer
            // with actual salary calculation logic
            return new ProcessMonthlySalaryResult
            {
                Success = true,
                Message = "Monthly salary processing initiated"
            };
        }
        catch (Exception ex)
        {
            return new ProcessMonthlySalaryResult
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}
