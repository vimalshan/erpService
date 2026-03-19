using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Entities;
using CategoryAndVendorService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CategoryAndVendorService.Application.SubCategories.Commands;

// --- Create ---
public record CreateSubCategoryCommand(long SubCatId, long MainCatId, string SubCatName, long ModifiedBy) : IRequest<SubCategoryDto>;

public class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommand>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(x => x.SubCatName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MainCatId).GreaterThan(0);
    }
}

public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand, SubCategoryDto>
{
    private readonly ISubCategoryRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateSubCategoryCommandHandler(ISubCategoryRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<SubCategoryDto> Handle(CreateSubCategoryCommand request, CancellationToken ct)
    {
        var entity = SubCategory.Create(request.SubCatId, request.MainCatId, request.SubCatName, request.ModifiedBy);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<SubCategoryDto>(entity);
    }
}

// --- Update ---
public record UpdateSubCategoryCommand(long SubCatId, string SubCatName, long ModifiedBy) : IRequest<SubCategoryDto>;

public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommand, SubCategoryDto>
{
    private readonly ISubCategoryRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateSubCategoryCommandHandler(ISubCategoryRepository repo, IUnitOfWork uow, IMapper mapper)
    { _repo = repo; _uow = uow; _mapper = mapper; }

    public async Task<SubCategoryDto> Handle(UpdateSubCategoryCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.SubCatId, ct)
            ?? throw new KeyNotFoundException($"SubCategory {request.SubCatId} not found.");
        entity.Update(request.SubCatName, request.ModifiedBy);
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<SubCategoryDto>(entity);
    }
}

// --- Delete ---
public record DeleteSubCategoryCommand(long SubCatId) : IRequest<bool>;

public class DeleteSubCategoryCommandHandler : IRequestHandler<DeleteSubCategoryCommand, bool>
{
    private readonly ISubCategoryRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteSubCategoryCommandHandler(ISubCategoryRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<bool> Handle(DeleteSubCategoryCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.SubCatId, ct);
        if (entity is null) return false;
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
