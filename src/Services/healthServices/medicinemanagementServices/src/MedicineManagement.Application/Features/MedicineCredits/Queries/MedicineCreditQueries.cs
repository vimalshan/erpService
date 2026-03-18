using MediatR;
using MedicineManagement.Application.DTOs;

namespace MedicineManagement.Application.Features.MedicineCredits.Queries;

public record GetStockByMedicineQuery(string MedicineCode) : IRequest<long>;
public record GetTransactionsByDateRangeQuery(DateTime From, DateTime To) : IRequest<IReadOnlyList<MedicineCreditDto>>;
public record GetTransactionsByMedicineQuery(string MedicineCode) : IRequest<IReadOnlyList<MedicineCreditDto>>;
