using FluentValidation;
using HealthTransaction.Application.DTOs;
using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.DynamicHealthDetails.Commands.Save;

public record SaveDynamicHealthDetailsCommand(IList<SaveDynamicHealthDetailDto> Items) : IRequest<IList<DynamicHealthDetailDto>>;

public class SaveDynamicHealthDetailsCommandHandler : IRequestHandler<SaveDynamicHealthDetailsCommand, IList<DynamicHealthDetailDto>>
{
    private readonly IUnitOfWork _uow;
    public SaveDynamicHealthDetailsCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IList<DynamicHealthDetailDto>> Handle(SaveDynamicHealthDetailsCommand request, CancellationToken cancellationToken)
    {
        var entities = request.Items.Select(dto => new DynamicHealthDetail
        {
            HlthNum = dto.HlthNum,
            ChkupCod = dto.ChkupCod,
            ComCode = dto.ComCode,
            CtrlSrcId = dto.CtrlSrcId,
            DynVal = dto.DynVal,
            EmpNum = dto.EmpNum,
            SysDate = DateTime.UtcNow
        }).ToList();

        await _uow.DynamicHealthDetails.AddRangeAsync(entities, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return entities.Select(MapToDto).ToList();
    }

    internal static DynamicHealthDetailDto MapToDto(DynamicHealthDetail d) => new(
        d.HlthNum, d.ChkupCod, d.ComCode, d.CtrlSrcId, d.DynVal, d.EmpNum, d.SysDate);
}

public class SaveDynamicHealthDetailsCommandValidator : AbstractValidator<SaveDynamicHealthDetailsCommand>
{
    public SaveDynamicHealthDetailsCommandValidator()
    {
        RuleFor(x => x.Items).NotEmpty().WithMessage("Items list cannot be empty.");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.HlthNum).GreaterThan(0);
            item.RuleFor(x => x.ChkupCod).NotEmpty();
            item.RuleFor(x => x.ComCode).NotEmpty().MaximumLength(3);
        });
    }
}
