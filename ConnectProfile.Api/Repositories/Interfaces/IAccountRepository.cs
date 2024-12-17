using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByUserNameAsync(string userName);
    Task<Account?> GetByEmailAsync(string email);
    Task AddAsync(Account account);
    Task<bool> DeleteAccountAsync(Guid accountId);
    Task<IEnumerable<Account>> GetAllUsersAsync();
}
