using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services.Interfaces;
using ConnectProfile.Api.Validators.Interfaces;

namespace ConnectProfile.Api.Services;

public class AccountService(
    IAccountRepository accountRepository,
    IJwtService jwtService,
    IValidationService validationService,
    ILogger<AccountService> logger
    ) : IAccountService
{
    public async Task<Response<bool>> RegisterAsync(RegisterRequestDto dto)
    {
        logger.LogInformation($"Registering new account for user {dto.UserName}");

        var validationError = await validationService.ValidateRegisterAsync(dto);
        if (!string.IsNullOrEmpty(validationError))
        {
            logger.LogWarning($"Registration failed for {dto.UserName}: {validationError}");
            return Response<bool>.Fail(validationError);
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

        logger.LogInformation($"Account for {dto.UserName} validated. Saving new account to the database.");
        await accountRepository.AddAsync(newAccount);

        logger.LogInformation($"Account for {dto.UserName} created successfully.");
        return Response<bool>.Ok(true);
    }

    public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        logger.LogInformation($"Login attempt for {dto.UserName}");

        var account = await accountRepository.GetByUserNameAsync(dto.UserName);
        if (account == null || !VerifyPassword(dto.Password, account.PasswordHash, account.PasswordSalt))
        {
            logger.LogWarning($"Invalid login attempt for {dto.UserName}: User not found or incorrect password.");
            return Response<LoginResponseDto>.Fail("Invalid username or password.");
        }

        logger.LogInformation($"Login successful for {dto.UserName}. Generating JWT token.");
        var token = jwtService.GenerateToken(account);
        var response = new LoginResponseDto
        {
            UserId = account.Id,
            Token = token,
            UserName = account.UserName,
            Role = account.Role
        };

        logger.LogInformation($"Login successful for {dto.UserName}. Token generated.");
        return Response<LoginResponseDto>.Ok(response);
    }

    public async Task<Response<string>> DeleteAccountAsync(Guid accountId)
    {
        logger.LogInformation($"Attempting to delete account with ID: {accountId}");

        var success = await accountRepository.DeleteAccountAsync(accountId);

        if (!success)
        {
            logger.LogWarning($"Deletion failed. Account with ID {accountId} not found.");
            return Response<string>.Fail($"Account with ID {accountId} not found.");
        }

        logger.LogInformation($"Account with ID {accountId} and related data deleted successfully.");
        return Response<string>.Ok($"Account with ID {accountId} has been deleted successfully.");
    }
    public async Task<Response<IEnumerable<Account>>> GetAllUsersAsync()
    {
        var users = await accountRepository.GetAllUsersAsync();

        if (users == null || users.Count() == 0)
        {
            return Response<IEnumerable<Account>>.Fail("No users found.");
        }

        return Response<IEnumerable<Account>>.Ok(users);
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
