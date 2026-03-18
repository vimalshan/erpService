using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.Medicines.Queries;

public record GetAllMedicinesQuery : IRequest<IReadOnlyList<MedicineDto>>;
public record GetMedicineByCodeQuery(string MedicineCode) : IRequest<MedicineDto?>;
public record SearchMedicinesQuery(string Name) : IRequest<IReadOnlyList<MedicineDto>>;
public record GetLowStockMedicinesQuery : IRequest<IReadOnlyList<StockSummaryDto>>;
