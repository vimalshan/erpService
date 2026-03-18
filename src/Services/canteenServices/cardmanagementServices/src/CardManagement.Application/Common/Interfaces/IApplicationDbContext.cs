using CardManagement.Domain.Entities;

namespace CardManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    System.Linq.IQueryable<GuestCardMaster> GuestCardMasters { get; }
    System.Linq.IQueryable<CanteenCardMap> CanteenCardMaps { get; }
    System.Linq.IQueryable<CardSettlement> CardSettlements { get; }
    System.Linq.IQueryable<GuestCardMasterHistory> GuestCardMasterHistories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
