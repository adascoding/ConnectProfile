using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services.Interfaces;

namespace ConnectProfile.Api.Services;

public class AccountService(IAccountRepository accountRepository, IJwtService jwtService, ILogger<AccountService> logger) : IAccountService
{
    public async Task<bool> RegisterAsync(RegisterRequestDto dto)
    {
        var existingAccount = await accountRepository.GetByUserNameAsync(dto.UserName);
        if (existingAccount != null)
        {
            logger.LogWarning($"User {dto.UserName} already exists.");
            return false;
        }

        var passwordHashSalt = CreatePasswordHash(dto.Password);
        var newAccount = new Account
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = passwordHashSalt.Hash,
            PasswordSalt = passwordHashSalt.Salt,
            Role = "User"
        };

        await accountRepository.AddAsync(newAccount);
        return true;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var account = await accountRepository.GetByUserNameAsync(dto.UserName);

        if (account == null || !VerifyPassword(dto.Password, account.PasswordHash, account.PasswordSalt))
        {
            logger.LogWarning($"Invalid login attempt for {dto.UserName}");
            return null;
        }

        var token = jwtService.GenerateToken(account);
        var response = new LoginResponseDto
        {
            UserId = account.Id,
            Token = token,
            UserName = account.UserName,
            Role = account.Role
        };

        return response;
    }

    private bool VerifyPassword(string enteredPassword, byte[] storedPasswordHash, byte[] storedPasswordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedPasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPassword));
            return computedHash.SequenceEqual(storedPasswordHash);
        }
    }

    private (byte[] Hash, byte[] Salt) CreatePasswordHash(string password)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return (hash, salt);
        }
    }
}
