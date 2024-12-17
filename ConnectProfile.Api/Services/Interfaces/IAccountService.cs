using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Services.Interfaces;

public interface IAccountService
{
    Task<Response<bool>> RegisterAsync(RegisterRequestDto dto);
    Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
    Task<Response<string>> DeleteAccountAsync(Guid accountId);
    Task<Response<IEnumerable<Account>>> GetAllUsersAsync();
}
