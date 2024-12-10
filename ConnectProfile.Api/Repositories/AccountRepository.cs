using ConnectProfile.Api.Data;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConnectProfile.Api.Repositories;

public class AccountRepository(ApplicationDbContext context) : IAccountRepository
{
    public async Task<Account?> GetByUserNameAsync(string userName)
    {
        return await context.Accounts
            .FirstOrDefaultAsync(a => a.UserName == userName);
    }

    public async Task AddAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }
}
