using ConnectProfile.Api.Data;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectProfile.Api.Repositories;

public class UserInfoRepository(ApplicationDbContext context) : IUserInfoRepository
{
    public async Task<UserInfo?> GetByAccountIdAsync(Guid accountId)
    {
        return await context.UserInfos
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.AccountId == accountId);
    }

    public async Task AddAsync(UserInfo userInfo)
    {
        await context.UserInfos.AddAsync(userInfo);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
