using MediatR;
using FluentValidation;
using AutoMapper;
using ApprovalGroup.Domain.Interfaces;
using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Domain.Exceptions;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Application.PullMatrix.Commands;

// ─── Create Pull Matrix ───────────────────────────────────────
public record CreatePullMatrixCommand(long UnitId, string PayBy, char Flag, long MainCat,
    long EmpSysId, long MaxNos, long CreatedBy) : IRequest<PullMatrixDetailDto>;

public class CreatePullMatrixValidator : AbstractValidator<CreatePullMatrixCommand>
{
    public CreatePullMatrixValidator()
    {
        RuleFor(x => x.UnitId).GreaterThan(0);
        RuleFor(x => x.PayBy).NotEmpty().MaximumLength(2);
        RuleFor(x => x.MainCat).GreaterThan(0);
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.MaxNos).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CreatePullMatrixHandler : IRequestHandler<CreatePullMatrixCommand, PullMatrixDetailDto>
{
    private readonly IPullMatrixRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreatePullMatrixHandler(IPullMatrixRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PullMatrixDetailDto> Handle(CreatePullMatrixCommand request, CancellationToken ct)
    {
        var nextId = await _repo.GetNextIdAsync(ct);
        var detail = PullMatrixDetail.Create(nextId, request.UnitId, request.PayBy, request.Flag,
            request.MainCat, request.EmpSysId, request.MaxNos, request.CreatedBy);
        await _repo.AddAsync(detail, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<PullMatrixDetailDto>(detail);
    }
}

// ─── Update Pull Matrix ───────────────────────────────────────
public record UpdatePullMatrixCommand(long MatId, string PayBy, char Flag, long MainCat,
    long EmpSysId, long MaxNos, long ModifiedBy) : IRequest<PullMatrixDetailDto>;

public class UpdatePullMatrixHandler : IRequestHandler<UpdatePullMatrixCommand, PullMatrixDetailDto>
{
    private readonly IPullMatrixRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdatePullMatrixHandler(IPullMatrixRepository repo, IUnitOfWork uow, IMapper mapper)
    {
        _repo = repo;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PullMatrixDetailDto> Handle(UpdatePullMatrixCommand request, CancellationToken ct)
    {
        var detail = await _repo.GetByIdAsync(request.MatId, ct)
            ?? throw new PullMatrixNotFoundException(request.MatId);
        detail.Update(request.PayBy, request.Flag, request.MainCat, request.EmpSysId, request.MaxNos, request.ModifiedBy);
        await _repo.UpdateAsync(detail, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<PullMatrixDetailDto>(detail);
    }
}
