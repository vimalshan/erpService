using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineTypes.Commands;
using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.MedicineTypes.Handlers;

public class CreateMedicineTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateMedicineTypeCommand, MedicineTypeDto>
{
    public async Task<MedicineTypeDto> Handle(CreateMedicineTypeCommand request, CancellationToken ct)
    {
        var entity = MedicineType.Create(request.TypeCode, request.TypeName, request.EntryUser, request.UserPin);
        await unitOfWork.MedicineTypes.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<MedicineTypeDto>(entity);
    }
}

public class UpdateMedicineTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateMedicineTypeCommand, MedicineTypeDto>
{
    public async Task<MedicineTypeDto> Handle(UpdateMedicineTypeCommand request, CancellationToken ct)
    {
        var entity = await unitOfWork.MedicineTypes.GetByCodeAsync(request.TypeCode, ct)
            ?? throw new KeyNotFoundException($"Medicine type '{request.TypeCode}' not found.");
        entity.Update(request.TypeName, request.ModifiedUser, request.ModifiedUserPin);
        await unitOfWork.MedicineTypes.UpdateAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<MedicineTypeDto>(entity);
    }
}

public class DeleteMedicineTypeHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteMedicineTypeCommand, bool>
{
    public async Task<bool> Handle(DeleteMedicineTypeCommand request, CancellationToken ct)
    {
        await unitOfWork.MedicineTypes.DeleteAsync(request.TypeCode, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
