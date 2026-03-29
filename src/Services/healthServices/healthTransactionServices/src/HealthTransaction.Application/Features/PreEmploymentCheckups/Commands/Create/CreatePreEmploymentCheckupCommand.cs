using FluentValidation;
using HealthTransaction.Application.DTOs;
using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Events;
using HealthTransaction.Domain.Interfaces;
using MediatR;

namespace HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;

public record CreatePreEmploymentCheckupCommand(CreatePreEmploymentCheckupDto Dto) : IRequest<PreEmploymentCheckupDto>;

public class CreatePreEmploymentCheckupCommandHandler : IRequestHandler<CreatePreEmploymentCheckupCommand, PreEmploymentCheckupDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMessagePublisher _publisher;

    public CreatePreEmploymentCheckupCommandHandler(IUnitOfWork uow, IMessagePublisher publisher)
    {
        _uow = uow;
        _publisher = publisher;
    }

    public async Task<PreEmploymentCheckupDto> Handle(CreatePreEmploymentCheckupCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var entity = new PreEmploymentCheckup
        {
            EmpNum = dto.EmpNum,
            ComCode = dto.ComCode,
            HlthNum = dto.HlthNum,
            PhysHandicap = dto.PhysHandicap,
            ProposedEmp = dto.ProposedEmp,
            IdentMarks = dto.IdentMarks,
            FinalRemarks = dto.FinalRemarks,
            FitPh = dto.FitPh?.Length > 0 ? dto.FitPh[0] : null,
            FitFinal = dto.FitFinal,
            CheckupDate = dto.CheckupDate
        };
        entity.AddDomainEvent(new PreEmploymentCheckupCreatedEvent(entity));
        await _uow.PreEmploymentCheckups.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        // Publish integration event to RabbitMQ
        try
        {
            await _publisher.PublishAsync(
                exchange: "health.transaction.events",
                routingKey: "preemployment.created",
                message: new { entity.EmpNum, entity.ComCode, entity.HlthNum, entity.CheckupDate, CreatedAt = DateTime.UtcNow },
                cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            // Publishing failure must not affect the primary operation
        }

        return MapToDto(entity);
    }

    internal static PreEmploymentCheckupDto MapToDto(PreEmploymentCheckup e) => new(
        e.EmpNum, e.ComCode, e.HlthNum, e.PhysHandicap, e.ProposedEmp,
        e.IdentMarks, e.FinalRemarks,
        e.FitPh.HasValue ? e.FitPh.Value.ToString() : null,
        e.FitFinal, e.CheckupDate);
}

public class CreatePreEmploymentCheckupCommandValidator : AbstractValidator<CreatePreEmploymentCheckupCommand>
{
    public CreatePreEmploymentCheckupCommandValidator()
    {
        RuleFor(x => x.Dto.EmpNum).GreaterThan(0).WithMessage("Employee number must be positive.");
        RuleFor(x => x.Dto.ComCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Dto.HlthNum).GreaterThan(0).WithMessage("Health number must be positive.");
    }
}
