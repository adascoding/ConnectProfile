using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Repositories.Interfaces;

public interface IUserInfoRepository
{
    Task<UserInfo?> GetByAccountIdAsync(Guid accountId);
    Task AddAsync(UserInfo userInfo);
    Task SaveChangesAsync();
}
