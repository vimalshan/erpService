using InvestmentService.Application.DTOs;
using MediatR;

namespace InvestmentService.Application.Commands;

public record CreateInvestmentCommand(
    long InvNo,
    int CategoryId,
    int? SubCategoryId,
    decimal Units,
    decimal PurchaseRate,
    DateTime PurchaseDate,
    DateTime MaturityDate,
    decimal InterestRate,
    int? Tenure,
    string? InterestOption,
    string? InterestFrequency,
    string? PaymentMode,
    int? BrokerId,
    string? CertificateNumber,
    long EnteredBy
) : IRequest<InvestmentDto>;

public record RedeemInvestmentCommand(
    long SaleNo,
    long InvNo,
    string SaleType,
    DateTime SaleDate,
    decimal SaleValue,
    decimal InterestAdjusted,
    decimal SalePremium,
    string? Remarks,
    long EnteredBy
) : IRequest<SaleDetailDto>;

public record UpdateInvestmentCommand(
    long InvNo,
    decimal? RevisedInterestRate,
    DateTime? RevisedInterestFrom,
    decimal? YtmRate,
    string? Status,
    long ModifiedBy
) : IRequest<InvestmentDto>;

public record CreateCategoryCommand(
    int Code,
    string ShortCode,
    string Name,
    long Denomination,
    int GroupId
) : IRequest<CategoryDto>;

public record CreateSubCategoryCommand(
    int Id,
    string ShortName,
    string Name,
    int CategoryId
) : IRequest<SubCategoryDto>;

public record CreateBrokerCommand(
    decimal BrokerId,
    string BrokerName,
    string BrokerStatus
) : IRequest<BrokerDto>;

public record RecordInterestReceiptCommand(
    long SchId,
    decimal ReceivedAmount,
    DateTime ReceivedDate,
    long ReceivedTransactionId
) : IRequest<ScheduleDetailDto>;

public record ApproveInvestmentCommand(
    decimal ApprovalDetailId,
    long InvestmentId,
    decimal RefId,
    decimal ApprovalLevel,
    string Flag,
    decimal ApproverSysId,
    string? Remarks
) : IRequest<bool>;

public record GenerateInterestScheduleCommand(
    long InvNo,
    long Year
) : IRequest<List<ScheduleDetailDto>>;
