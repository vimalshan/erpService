using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Medicines.Commands;
using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.Medicines.Handlers;

public class CreateMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateMedicineCommand, MedicineDto>
{
    public async Task<MedicineDto> Handle(CreateMedicineCommand request, CancellationToken ct)
    {
        var entity = Medicine.Create(
            request.MedicineCode, request.MedicineName, request.MedicineTypeCode,
            request.Category, request.OrderLevelMin, request.OrderLevelMax,
            request.EntryUser, request.UserPin);
        await unitOfWork.Medicines.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<MedicineDto>(entity);
    }
}

public class UpdateMedicineHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateMedicineCommand, MedicineDto>
{
    public async Task<MedicineDto> Handle(UpdateMedicineCommand request, CancellationToken ct)
    {
        var entity = await unitOfWork.Medicines.GetByCodeAsync(request.MedicineCode, ct)
            ?? throw new KeyNotFoundException($"Medicine '{request.MedicineCode}' not found.");
        entity.Update(request.MedicineName, request.MedicineTypeCode,
            request.Category, request.OrderLevelMin, request.OrderLevelMax,
            request.ModifiedUser, request.ModifiedUserPin);
        await unitOfWork.Medicines.UpdateAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return mapper.Map<MedicineDto>(entity);
    }
}

public class DeleteMedicineHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteMedicineCommand, bool>
{
    public async Task<bool> Handle(DeleteMedicineCommand request, CancellationToken ct)
    {
        await unitOfWork.Medicines.DeleteAsync(request.MedicineCode, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
