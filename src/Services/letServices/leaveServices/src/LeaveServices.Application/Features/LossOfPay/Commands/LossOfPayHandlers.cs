using MediatR;
using LeaveServices.Application.DTOs;
using LeaveServices.Domain.Repositories;
using LopEntity = LeaveServices.Domain.Entities.LossOfPay;

namespace LeaveServices.Application.Features.LossOfPay.Commands;

public sealed class RecordLossOfPayHandler : IRequestHandler<RecordLossOfPayCommand, LossOfPayDto>
{
    private readonly ILossOfPayRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RecordLossOfPayHandler(ILossOfPayRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LossOfPayDto> Handle(RecordLossOfPayCommand command, CancellationToken ct)
    {
        var entity = LopEntity.Record(
            command.EmpSysId, command.LopDays, command.LopMonth,
            command.Remarks, command.RecordedBy);

        await _repository.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new LossOfPayDto(entity.LopId, entity.EmpSysId, entity.LopDays,
            entity.LopMonth, entity.LopRemarks, entity.CreatedOn);
    }
}
