using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.Medicines.Commands;

public record CreateMedicineCommand(
    string MedicineCode, string MedicineName, string MedicineTypeCode,
    char? Category, decimal? OrderLevelMin, decimal? OrderLevelMax,
    string EntryUser, decimal? UserPin) : IRequest<MedicineDto>;

public record UpdateMedicineCommand(
    string MedicineCode, string MedicineName, string MedicineTypeCode,
    char? Category, decimal? OrderLevelMin, decimal? OrderLevelMax,
    string ModifiedUser, decimal? ModifiedUserPin) : IRequest<MedicineDto>;

public record DeleteMedicineCommand(string MedicineCode) : IRequest<bool>;
