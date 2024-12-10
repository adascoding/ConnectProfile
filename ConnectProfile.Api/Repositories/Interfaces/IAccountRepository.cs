using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByUserNameAsync(string userName);
    Task AddAsync(Account account);
}
