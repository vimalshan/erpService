using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CategoryAndVendorService.Application.MainCategories.Commands;

// --- Create ---
public record CreateMainCategoryCommand(long MainCatId, string MainCatName, long MainCatPriority, long ModifiedBy, long? DefaultSubCatId, long? AvgResponseTime) : IRequest<MainCategoryDto>;

public class CreateMainCategoryCommandValidator : AbstractValidator<CreateMainCategoryCommand>
{
    public CreateMainCategoryCommandValidator()
    {
        RuleFor(x => x.MainCatName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MainCatPriority).GreaterThan(0);
    }
}

public class CreateMainCategoryCommandHandler : IRequestHandler<CreateMainCategoryCommand, MainCategoryDto>
{
    private readonly IMainCategoryRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateMainCategoryCommandHandler(IMainCategoryRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<MainCategoryDto> Handle(CreateMainCategoryCommand request, CancellationToken ct)
    {
        var entity = MainCategory.Create(request.MainCatId, request.MainCatName, request.MainCatPriority,
            request.ModifiedBy, request.DefaultSubCatId, request.AvgResponseTime);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<MainCategoryDto>(entity);
    }
}

// --- Update ---
public record UpdateMainCategoryCommand(long MainCatId, string MainCatName, long MainCatPriority, long ModifiedBy, long? DefaultSubCatId, long? AvgResponseTime) : IRequest<MainCategoryDto>;

public class UpdateMainCategoryCommandValidator : AbstractValidator<UpdateMainCategoryCommand>
{
    public UpdateMainCategoryCommandValidator()
    {
        RuleFor(x => x.MainCatName).NotEmpty().MaximumLength(200);
    }
}

public class UpdateMainCategoryCommandHandler : IRequestHandler<UpdateMainCategoryCommand, MainCategoryDto>
{
    private readonly IMainCategoryRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateMainCategoryCommandHandler(IMainCategoryRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<MainCategoryDto> Handle(UpdateMainCategoryCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.MainCatId, ct)
            ?? throw new KeyNotFoundException($"MainCategory {request.MainCatId} not found.");
        entity.Update(request.MainCatName, request.MainCatPriority, request.ModifiedBy, request.DefaultSubCatId, request.AvgResponseTime);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<MainCategoryDto>(entity);
    }
}

// --- Delete ---
public record DeleteMainCategoryCommand(long MainCatId) : IRequest<bool>;

public class DeleteMainCategoryCommandHandler : IRequestHandler<DeleteMainCategoryCommand, bool>
{
    private readonly IMainCategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteMainCategoryCommandHandler(IMainCategoryRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(DeleteMainCategoryCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.MainCatId, ct);
        if (entity is null) return false;
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
