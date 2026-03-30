using EmployeeManagement.Application.Promotions.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using EmployeeManagement.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Promotions.Commands.CreatePromotion;

public sealed record CreatePromotionCommand(
    long PromotionNo,
    string Source,
    long RequestNo,
    long EmployeeId,
    long OldGradeId,
    long NewGradeId,
    long OldPositionId,
    long NewPositionId,
    long ReasonId,
    string? Remarks,
    long IncrementNo,
    string? Designation,
    char? PromotionType,
    long CreatedBy
) : IRequest<PromotionDto>;

public sealed class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);
        RuleFor(x => x.NewGradeId).GreaterThan(0);
        RuleFor(x => x.NewPositionId).GreaterThan(0);
        RuleFor(x => x.Source).NotEmpty().MaximumLength(3);
    }
}

public sealed class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, PromotionDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePromotionCommandHandler(IEmployeeRepository employeeRepository,
        IPromotionRepository promotionRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _promotionRepository = promotionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PromotionDto> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new EmployeeNotFoundException(request.EmployeeId);

        var promotion = EmployeePromotion.Create(
            request.PromotionNo, request.Source, request.RequestNo, request.EmployeeId,
            request.OldGradeId, request.NewGradeId, request.OldPositionId, request.NewPositionId,
            request.ReasonId, request.Remarks, request.IncrementNo, request.Designation,
            request.PromotionType, request.CreatedBy);

        employee.Promote(request.NewGradeId, request.NewPositionId, request.PromotionNo, request.CreatedBy);
        _employeeRepository.Update(employee);
        await _promotionRepository.AddAsync(promotion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new PromotionDto(promotion.PromotionNo, promotion.EmployeeId, promotion.Source,
            promotion.OldGradeId, promotion.NewGradeId, promotion.Status.ToString(),
            promotion.Designation, promotion.PromotionType?.ToString(), promotion.CreatedOn, promotion.CreatedBy);
    }
}
