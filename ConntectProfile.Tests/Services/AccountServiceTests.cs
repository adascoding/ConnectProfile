using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services;
using ConnectProfile.Api.Services.Interfaces;
using ConnectProfile.Api.Validators.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConnectProfile.Api.Tests.Services;

[TestFixture]
public class AccountServiceTests
{
    private Mock<IAccountRepository> _accountRepositoryMock;
    private Mock<IJwtService> _jwtServiceMock;
    private Mock<IValidationService> _validationServiceMock;
    private Mock<ILogger<AccountService>> _loggerMock;
    private AccountService _accountService;

    [SetUp]
    public void SetUp()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _validationServiceMock = new Mock<IValidationService>();
        _loggerMock = new Mock<ILogger<AccountService>>();

        _accountService = new AccountService(
            _accountRepositoryMock.Object,
            _jwtServiceMock.Object,
            _validationServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task RegisterAsync_InvalidRequest_ReturnsErrorResponse()
    {
        // Arrange
        var dto = new RegisterRequestDto
        {
            UserName = "testUser",
            Email = "test@example.com",
            Password = "password123"
        };

        _validationServiceMock.Setup(v => v.ValidateRegisterAsync(dto)).ReturnsAsync("Validation failed.");

        // Act
        var result = await _accountService.RegisterAsync(dto);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Validation failed.", result.Message);
    }

    [Test]
    public async Task LoginAsync_InvalidCredentials_ReturnsErrorResponse()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            UserName = "testUser",
            Password = "wrongPassword"
        };

        _accountRepositoryMock.Setup(r => r.GetByUserNameAsync(dto.UserName)).ReturnsAsync((Account)null);

        // Act
        var result = await _accountService.LoginAsync(dto);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid username or password.", result.Message);
    }

    [Test]
    public async Task DeleteAccountAsync_AccountDoesNotExist_ReturnsErrorResponse()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _accountRepositoryMock.Setup(r => r.DeleteAccountAsync(accountId)).ReturnsAsync(false);

        // Act
        var result = await _accountService.DeleteAccountAsync(accountId);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual($"Account with ID {accountId} not found.", result.Message);
    }


    [Test]
    public async Task GetAllUsersAsync_NoUsers_ReturnsErrorResponse()
    {
        // Arrange
        _accountRepositoryMock.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(new List<Account>());

        // Act
        var result = await _accountService.GetAllUsersAsync();

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("No users found.", result.Message);
    }
}
