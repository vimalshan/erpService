using MediatR;

namespace InvestmentService.Domain.Events;

public record InvestmentPurchasedEvent(long InvestmentNo, DateTime PurchaseDate, decimal PurchaseValue) : INotification;
public record InvestmentRedeemedEvent(long InvestmentNo, DateTime RedemptionDate, decimal RedemptionValue) : INotification;
public record InvestmentMaturedEvent(long InvestmentNo, DateTime MaturityDate) : INotification;
public record InterestScheduleGeneratedEvent(long InvestmentNo, int ScheduleCount) : INotification;
public record InvestmentApprovedEvent(long InvestmentNo, decimal ApproverSysId, DateTime ApprovedOn) : INotification;
public record InvestmentCallExercisedEvent(long InvestmentNo, DateTime CallDate, decimal CallAmount) : INotification;
