using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Mappers.Interfaces;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services;
using ConnectProfile.Api.Validators.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConnectProfile.Api.Tests.Services;

[TestFixture]
public class UserInfoServiceTests
{
    private Mock<IUserInfoRepository> _repositoryMock;
    private Mock<IValidationService> _validationServiceMock;
    private Mock<IMapperService> _mapperMock;
    private Mock<ILogger<UserInfoService>> _loggerMock;
    private UserInfoService _userInfoService;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IUserInfoRepository>();
        _validationServiceMock = new Mock<IValidationService>();
        _mapperMock = new Mock<IMapperService>();
        _loggerMock = new Mock<ILogger<UserInfoService>>();

        _userInfoService = new UserInfoService(
            _repositoryMock.Object,
            _validationServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task GetUserInfoAsync_UserExists_ReturnsUserInfo()
    {
        var accountId = Guid.NewGuid();
        var userInfoEntity = new UserInfo
        {
            AccountId = accountId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var userInfoDto = new UserInfoDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        _repositoryMock.Setup(r => r.GetByAccountIdAsync(accountId)).ReturnsAsync(userInfoEntity);
        _mapperMock.Setup(m => m.MapToDto(userInfoEntity)).Returns(userInfoDto);

        var result = await _userInfoService.GetUserInfoAsync(accountId);

        Assert.IsTrue(result.Success);
        Assert.AreEqual(userInfoDto, result.Data);
    }

    [Test]
    public async Task GetUserInfoAsync_UserNotFound_ReturnsErrorResponse()
    {
        var accountId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByAccountIdAsync(accountId)).ReturnsAsync((UserInfo)null);

        var result = await _userInfoService.GetUserInfoAsync(accountId);

        Assert.IsFalse(result.Success);
        Assert.AreEqual("User info not found.", result.Message);
    }

    [Test]
    public async Task CreateUserInfoAsync_ValidInput_CreatesUserInfo()
    {
        var userInfoCreateDto = new UserInfoCreateDto
        {
            AccountId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var userInfoEntity = new UserInfo
        {
            AccountId = userInfoCreateDto.AccountId,
            FirstName = userInfoCreateDto.FirstName,
            LastName = userInfoCreateDto.LastName,
            Email = userInfoCreateDto.Email
        };

        var userInfoDto = new UserInfoDto
        {
            FirstName = userInfoCreateDto.FirstName,
            LastName = userInfoCreateDto.LastName,
            Email = userInfoCreateDto.Email
        };

        _validationServiceMock.Setup(v => v.ValidateUserInfo(userInfoCreateDto)).Returns((string)null);
        _mapperMock.Setup(m => m.MapToEntity(userInfoCreateDto)).Returns(userInfoEntity);
        _mapperMock.Setup(m => m.MapToDto(userInfoEntity)).Returns(userInfoDto);

        var result = await _userInfoService.CreateUserInfoAsync(userInfoCreateDto);

        Assert.IsTrue(result.Success);
        Assert.AreEqual(userInfoDto, result.Data);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<UserInfo>()), Times.Once);
    }

    [Test]
    public async Task CreateUserInfoAsync_ValidationFails_ReturnsErrorResponse()
    {
        var userInfoCreateDto = new UserInfoCreateDto
        {
            AccountId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        _validationServiceMock.Setup(v => v.ValidateUserInfo(userInfoCreateDto)).Returns("Invalid email");

        var result = await _userInfoService.CreateUserInfoAsync(userInfoCreateDto);

        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid email", result.Message);
    }

    [Test]
    public async Task UpdateFieldAsync_UserInfoExists_UpdatesField()
    {
        var accountId = Guid.NewGuid();
        var dto = new UpdateFieldRequestDto
        {
            FieldName = "FirstName",
            FieldValue = "Jane"
        };

        var userInfoEntity = new UserInfo
        {
            AccountId = accountId,
            FirstName = "John",
            LastName = "Doe"
        };

        _repositoryMock.Setup(r => r.GetByAccountIdAsync(accountId)).ReturnsAsync(userInfoEntity);

        var result = await _userInfoService.UpdateFieldAsync(accountId, dto);

        Assert.IsTrue(result.Success);
        Assert.AreEqual("Jane", userInfoEntity.FirstName);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdateFieldAsync_UserInfoNotFound_ReturnsErrorResponse()
    {
        var accountId = Guid.NewGuid();
        var dto = new UpdateFieldRequestDto
        {
            FieldName = "FirstName",
            FieldValue = "Jane"
        };

        _repositoryMock.Setup(r => r.GetByAccountIdAsync(accountId)).ReturnsAsync((UserInfo)null);

        var result = await _userInfoService.UpdateFieldAsync(accountId, dto);

        Assert.IsFalse(result.Success);
        Assert.AreEqual("User info not found.", result.Message);
    }
}
