using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.MedicineTypes.Commands;

public record CreateMedicineTypeCommand(string TypeCode, string? TypeName, string EntryUser, decimal? UserPin) : IRequest<MedicineTypeDto>;
public record UpdateMedicineTypeCommand(string TypeCode, string? TypeName, string ModifiedUser, decimal? ModifiedUserPin) : IRequest<MedicineTypeDto>;
public record DeleteMedicineTypeCommand(string TypeCode) : IRequest<bool>;
