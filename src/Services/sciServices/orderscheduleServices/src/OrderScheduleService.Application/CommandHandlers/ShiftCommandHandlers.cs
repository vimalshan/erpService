namespace OrderScheduleService.Application.CommandHandlers;

using MediatR;
using AutoMapper;
using OrderScheduleService.Application.Commands;
using OrderScheduleService.Domain.Interfaces;
using OrderScheduleService.Domain.Entities;

public class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, bool>
{
    private readonly IShiftRepository _repository;
    private readonly IMapper _mapper;

    public CreateShiftCommandHandler(IShiftRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = new Shift(
            request.Shift.ShiftCode[0],
            request.Shift.ShiftDescription,
            request.Shift.CompanyUnitId,
            request.Shift.StartTime,
            request.Shift.StartDay,
            request.Shift.EndTime,
            request.Shift.EndDay);

        await _repository.AddAsync(shift);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, bool>
{
    private readonly IShiftRepository _repository;

    public UpdateShiftCommandHandler(IShiftRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await _repository.GetByIdAsync(request.ShiftCode, request.CompanyUnitId);
        if (shift == null)
            throw new InvalidOperationException($"Shift {request.ShiftCode} not found");

        shift.ShiftDescription = request.Shift.ShiftDescription;
        shift.StartTime = request.Shift.StartTime;
        shift.StartDay = request.Shift.StartDay;
        shift.EndTime = request.Shift.EndTime;
        shift.EndDay = request.Shift.EndDay;

        await _repository.UpdateAsync(shift);
        await _repository.SaveChangesAsync();

        return true;
    }
}

public class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand, bool>
{
    private readonly IShiftRepository _repository;

    public DeleteShiftCommandHandler(IShiftRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.ShiftCode, request.CompanyUnitId);
        await _repository.SaveChangesAsync();

        return true;
    }
}
