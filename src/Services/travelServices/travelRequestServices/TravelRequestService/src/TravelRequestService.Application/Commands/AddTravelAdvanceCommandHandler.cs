using AutoMapper;
using MediatR;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Application.Commands;

public class AddTravelAdvanceCommandHandler : IRequestHandler<AddTravelAdvanceCommand, TravelAdvanceDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddTravelAdvanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TravelAdvanceDto> Handle(AddTravelAdvanceCommand request, CancellationToken cancellationToken)
    {
        var existingAdvances = await _unitOfWork.TravelAdvances.GetByRequestAsync(request.RequestNumber, cancellationToken);
        var nextAdvanceNumber = existingAdvances.Count > 0
            ? existingAdvances.Max(a => a.AdvanceNumber) + 1
            : 1;

        var advance = TravelAdvance.Create(
            request.RequestNumber,
            nextAdvanceNumber,
            request.AdvanceAmount,
            request.UnitCode,
            request.EmployeeNumber);

        await _unitOfWork.TravelAdvances.AddAsync(advance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TravelAdvanceDto>(advance);
    }
}
