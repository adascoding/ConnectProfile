using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(Account account);
}
