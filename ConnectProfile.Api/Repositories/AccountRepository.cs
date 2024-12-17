using ConnectProfile.Api.Data;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectProfile.Api.Repositories;

public class AccountRepository(ApplicationDbContext context) : IAccountRepository
{
    private readonly ApplicationDbContext context = context;

    public async Task<Account?> GetByUserNameAsync(string userName)
    {
        return await context.Accounts
            .FirstOrDefaultAsync(a => a.UserName == userName);
    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        return await context.Accounts
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task AddAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }
    public async Task<bool> DeleteAccountAsync(Guid accountId)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        var userInfo = await context.UserInfos.FirstOrDefaultAsync(u => u.AccountId == accountId);
        var profilePicture = await context.Images.FirstOrDefaultAsync(i => i.AccountId == accountId);

        if (account == null)
            return false;

        if (userInfo != null)
            context.UserInfos.Remove(userInfo);

        if (profilePicture != null)
            context.Images.Remove(profilePicture);

        context.Accounts.Remove(account);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Account>> GetAllUsersAsync()
    {
        var users = await context.Accounts
                                  .Where(r => r.Role == "User")
                                  .ToListAsync();
        return users;
    }

}
