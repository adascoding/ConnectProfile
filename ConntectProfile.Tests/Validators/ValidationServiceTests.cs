using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Validators;
using Moq;

namespace ConnectProfile.Tests.Validators;

[TestFixture]
public class ValidationServiceTests
{
    private Mock<IAccountRepository> _accountRepositoryMock;
    private ValidationService _validationService;

    [SetUp]
    public void Setup()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _validationService = new ValidationService(_accountRepositoryMock.Object);
    }

    [Test]
    public async Task ValidateRegisterAsync_WhenUserNameIsEmpty_ReturnsErrorMessage()
    {
        var dto = new RegisterRequestDto { UserName = "" };

        var result = await _validationService.ValidateRegisterAsync(dto);

        Assert.AreEqual("Username is required.", result);
    }

    [Test]
    public async Task ValidateRegisterAsync_WhenUserNameIsNotAlphabetical_ReturnsErrorMessage()
    {
        var dto = new RegisterRequestDto { UserName = "User123" };

        var result = await _validationService.ValidateRegisterAsync(dto);

        Assert.AreEqual("Username must contain only alphabetical characters.", result);
    }

    [Test]
    public async Task ValidateRegisterAsync_WhenValidDto_ReturnsNull()
    {
        var dto = new RegisterRequestDto
        {
            UserName = "ValidUser",
            Email = "test@example.com",
            Password = "ValidPass123!"
        };

        _accountRepositoryMock.Setup(x => x.GetByUserNameAsync(dto.UserName)).ReturnsAsync((Account)null);
        _accountRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync((Account)null);

        var result = await _validationService.ValidateRegisterAsync(dto);

        Assert.IsNull(result);
    }

    [Test]
    public void ValidateLoginAsync_WhenValidDto_ReturnsNull()
    {
        var dto = new LoginRequestDto
        {
            UserName = "ValidUser",
            Password = "ValidPass123!"
        };

        var result = _validationService.ValidateLoginAsync(dto);

        Assert.IsNull(result);
    }

    [Test]
    public void ValidateLoginAsync_WhenUserNameIsEmpty_ReturnsErrorMessage()
    {
        var dto = new LoginRequestDto { UserName = "" };

        var result = _validationService.ValidateLoginAsync(dto);

        Assert.AreEqual("Username is required.", result);
    }

    [Test]
    public void ValidateLoginAsync_WhenPasswordIsEmpty_ReturnsErrorMessage()
    {
        var dto = new LoginRequestDto { UserName = "name", Password = "" };

        var result = _validationService.ValidateLoginAsync(dto);

        Assert.AreEqual("Password is required.", result);
    }
}
