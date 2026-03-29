using FluentValidation;
using HealthTransaction.Application.DTOs;
using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.PfiHistories.Commands.Create;

public record CreatePfiHistoryCommand(CreatePfiHistoryDto Dto) : IRequest<PfiHistoryDto>;

public class CreatePfiHistoryCommandHandler : IRequestHandler<CreatePfiHistoryCommand, PfiHistoryDto>
{
    private readonly IUnitOfWork _uow;
    public CreatePfiHistoryCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PfiHistoryDto> Handle(CreatePfiHistoryCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var entity = new PfiHistory
        {
            HlthNum = dto.HlthNum,
            EmpNum = dto.EmpNum,
            SympId = dto.SympId,
            YnFlag = dto.YnFlag?.Length > 0 ? dto.YnFlag[0] : null,
            ImmDate = dto.ImmDate,
            TestValue = dto.TestValue
        };
        await _uow.PfiHistories.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return MapToDto(entity);
    }

    internal static PfiHistoryDto MapToDto(PfiHistory p) => new(
        p.HlthNum, p.EmpNum, p.SympId,
        p.YnFlag.HasValue ? p.YnFlag.Value.ToString() : null,
        p.ImmDate, p.TestValue);
}

public class CreatePfiHistoryCommandValidator : AbstractValidator<CreatePfiHistoryCommand>
{
    public CreatePfiHistoryCommandValidator()
    {
        RuleFor(x => x.Dto.HlthNum).GreaterThan(0);
        RuleFor(x => x.Dto.EmpNum).GreaterThan(0);
        RuleFor(x => x.Dto.SympId).GreaterThan(0);
    }
}
