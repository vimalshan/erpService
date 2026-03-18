using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.MedicineTypes.Queries;

public record GetAllMedicineTypesQuery : IRequest<IReadOnlyList<MedicineTypeDto>>;
public record GetMedicineTypeByCodeQuery(string TypeCode) : IRequest<MedicineTypeDto?>;
