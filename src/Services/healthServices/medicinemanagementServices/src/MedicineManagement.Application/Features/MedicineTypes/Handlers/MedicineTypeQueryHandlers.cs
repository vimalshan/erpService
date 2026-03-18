using AutoMapper;
using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineTypes.Queries;
using MedicineManagement.Domain.Interfaces;

namespace MedicineManagement.Application.Features.MedicineTypes.Handlers;

public class GetAllMedicineTypesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllMedicineTypesQuery, IReadOnlyList<MedicineTypeDto>>
{
    public async Task<IReadOnlyList<MedicineTypeDto>> Handle(GetAllMedicineTypesQuery request, CancellationToken ct)
    {
        var types = await unitOfWork.MedicineTypes.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<MedicineTypeDto>>(types);
    }
}

public class GetMedicineTypeByCodeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetMedicineTypeByCodeQuery, MedicineTypeDto?>
{
    public async Task<MedicineTypeDto?> Handle(GetMedicineTypeByCodeQuery request, CancellationToken ct)
    {
        var type = await unitOfWork.MedicineTypes.GetByCodeAsync(request.TypeCode, ct);
        return type is null ? null : mapper.Map<MedicineTypeDto>(type);
    }
}
