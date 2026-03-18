namespace CommunityService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Persistence;

public interface ICommunityRepository
{
    Task<Community?> GetByIdAsync(long id);
    Task<Community?> GetByCodeAsync(string code);
    Task<IEnumerable<Community>> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Community>> GetByTypeAsync(string type, int pageNumber, int pageSize);
    Task<IEnumerable<Community>> GetByOwnerAsync(long ownerId, int pageNumber, int pageSize);
    Task<IEnumerable<Community>> SearchAsync(string searchTerm, int pageNumber, int pageSize);
    Task AddAsync(Community community);
    Task UpdateAsync(Community community);
    Task DeleteAsync(long id);
    Task SaveChangesAsync();
}

public class CommunityRepository : ICommunityRepository
{
    private readonly CommunityDbContext _context;

    public CommunityRepository(CommunityDbContext context)
    {
        _context = context;
    }

    public async Task<Community?> GetByIdAsync(long id)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.CommunityId == id);
    }

    public async Task<Community?> GetByCodeAsync(string code)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.CommunityCode.Value == code);
    }

    public async Task<IEnumerable<Community>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Community>> GetByTypeAsync(string type, int pageNumber, int pageSize)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .Where(c => c.CommunityType.Value == type)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Community>> GetByOwnerAsync(long ownerId, int pageNumber, int pageSize)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .Where(c => c.OwnerId == ownerId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Community>> SearchAsync(string searchTerm, int pageNumber, int pageSize)
    {
        return await _context.Communities
            .Include(c => c.Members)
            .Where(c => c.CommunityName.Value.Contains(searchTerm) || 
                        c.CommunityCode.Value.Contains(searchTerm))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(Community community)
    {
        await _context.Communities.AddAsync(community);
    }

    public async Task UpdateAsync(Community community)
    {
        _context.Communities.Update(community);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id)
    {
        var community = await GetByIdAsync(id);
        if (community != null)
        {
            _context.Communities.Remove(community);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public interface ICommunityMemberRepository
{
    Task<CommunityMember?> GetByIdAsync(long memberId);
    Task<CommunityMember?> GetByUserAndCommunityAsync(long communityId, long userId);
    Task<IEnumerable<CommunityMember>> GetByCommunityAsync(long communityId);
    Task<IEnumerable<CommunityMember>> GetByUserAsync(long userId);
    Task AddAsync(CommunityMember member);
    Task UpdateAsync(CommunityMember member);
    Task DeleteAsync(long memberId);
    Task SaveChangesAsync();
}

public class CommunityMemberRepository : ICommunityMemberRepository
{
    private readonly CommunityDbContext _context;

    public CommunityMemberRepository(CommunityDbContext context)
    {
        _context = context;
    }

    public async Task<CommunityMember?> GetByIdAsync(long memberId)
    {
        return await _context.CommunityMembers.FirstOrDefaultAsync(m => m.MemberId == memberId);
    }

    public async Task<CommunityMember?> GetByUserAndCommunityAsync(long communityId, long userId)
    {
        return await _context.CommunityMembers
            .FirstOrDefaultAsync(m => m.CommunityId == communityId && m.UserSysId == userId);
    }

    public async Task<IEnumerable<CommunityMember>> GetByCommunityAsync(long communityId)
    {
        return await _context.CommunityMembers
            .Where(m => m.CommunityId == communityId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CommunityMember>> GetByUserAsync(long userId)
    {
        return await _context.CommunityMembers
            .Where(m => m.UserSysId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(CommunityMember member)
    {
        await _context.CommunityMembers.AddAsync(member);
    }

    public async Task UpdateAsync(CommunityMember member)
    {
        _context.CommunityMembers.Update(member);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long memberId)
    {
        var member = await GetByIdAsync(memberId);
        if (member != null)
        {
            _context.CommunityMembers.Remove(member);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
