using ConnectProfile.Api.Dtos.Account;

namespace ConnectProfile.Api.Services.Interfaces;

public interface IAccountService
{
    Task<bool> RegisterAsync(RegisterRequestDto dto);
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto);
}
