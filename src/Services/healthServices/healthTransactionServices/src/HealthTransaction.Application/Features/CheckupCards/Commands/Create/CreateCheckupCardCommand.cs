using FluentValidation;
using HealthTransaction.Application.DTOs;
using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.CheckupCards.Commands.Create;

public record CreateCheckupCardCommand(CreateCheckupCardDto Dto) : IRequest<CheckupCardDto>;

public class CreateCheckupCardCommandHandler : IRequestHandler<CreateCheckupCardCommand, CheckupCardDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMessagePublisher _publisher;

    public CreateCheckupCardCommandHandler(IUnitOfWork uow, IMessagePublisher publisher)
    {
        _uow = uow;
        _publisher = publisher;
    }

    public async Task<CheckupCardDto> Handle(CreateCheckupCardCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var card = new CheckupCard
        {
            HlthNum = dto.HlthNum,
            EmpNum = dto.EmpNum,
            EmpDate = dto.EmpDate,
            ComCode = dto.ComCode,
            PersonalDetails = dto.PersonalDetails,
            ComplaintDetails = dto.ComplaintDetails,
            AdvRemark1 = dto.AdvRemark1,
            AdvRemark2 = dto.AdvRemark2,
            DocDate1 = dto.DocDate1,
            DocDate2 = dto.DocDate2,
            AdvFollow1 = dto.AdvFollow1,
            AdvFollow2 = dto.AdvFollow2,
            SubRecords = (dto.SubRecords ?? new List<CreateCheckupCardSubDto>())
                .Select(s => new CheckupCardSub
                {
                    HlthNum = dto.HlthNum,
                    SympId = s.SympId,
                    FlagYn = s.FlagYn?.Length > 0 ? s.FlagYn[0] : null,
                    SympVal = s.SympVal,
                    EmpNum = s.EmpNum
                }).ToList()
        };
        card.RaiseCreatedEvent();
        await _uow.CheckupCards.AddAsync(card, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        // Publish integration event to RabbitMQ
        try
        {
            await _publisher.PublishAsync(
                exchange: "health.transaction.events",
                routingKey: "checkupcard.created",
                message: new { card.HlthNum, card.EmpNum, card.ComCode, card.EmpDate, CreatedAt = DateTime.UtcNow },
                cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            // Publishing failure must not affect the primary operation
        }

        return MapToDto(card);
    }

    internal static CheckupCardDto MapToDto(CheckupCard c) => new(
        c.HlthNum, c.EmpNum, c.EmpDate, c.ComCode,
        c.PersonalDetails, c.ComplaintDetails,
        c.AdvRemark1, c.AdvRemark2,
        c.DocDate1, c.DocDate2,
        c.AdvFollow1, c.AdvFollow2,
        c.SubRecords.Select(s => new CheckupCardSubDto(
            s.HlthNum, s.SympId,
            s.FlagYn.HasValue ? s.FlagYn.Value.ToString() : null,
            s.SympVal, s.EmpNum)).ToList());
}

public class CreateCheckupCardCommandValidator : AbstractValidator<CreateCheckupCardCommand>
{
    public CreateCheckupCardCommandValidator()
    {
        RuleFor(x => x.Dto.HlthNum).GreaterThan(0).WithMessage("Health number must be positive.");
        RuleFor(x => x.Dto.EmpNum).GreaterThan(0).WithMessage("Employee number must be positive.");
    }
}
