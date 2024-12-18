using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Repositories.Interfaces;
using ConnectProfile.Api.Services;
using ConnectProfile.Api.Validators.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConnectProfile.Api.Tests.Services;

[TestFixture]
public class ImageServiceTests
{
    private Mock<IImageRepository> _imageRepositoryMock;
    private Mock<IValidationService> _validationServiceMock;
    private Mock<ILogger<ImageService>> _loggerMock;
    private ImageService _imageService;

    [SetUp]
    public void SetUp()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _validationServiceMock = new Mock<IValidationService>();
        _loggerMock = new Mock<ILogger<ImageService>>();

        _imageService = new ImageService(
            _imageRepositoryMock.Object,
            _validationServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task UploadImageAsync_InvalidImage_ReturnsErrorResponse()
    {
        var accountId = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        var name = "TestImage";
        var description = "Test image description";

        _validationServiceMock.Setup(v => v.ValidateImageUpload(fileMock.Object, name, description)).Returns("Invalid image");

        var result = await _imageService.UploadImageAsync(accountId, fileMock.Object, name, description);

        Assert.IsFalse(result.Success);
        Assert.AreEqual("Invalid image", result.Message);
    }

    [Test]
    public async Task GetImageByAccountIdAsync_ImageExists_ReturnsImage()
    {
        var accountId = Guid.NewGuid();
        var imageEntity = new Image
        {
            AccountId = accountId,
            Name = "TestImage",
            Description = "Test description",
            ImageBytes = new byte[] { 1, 2, 3 }
        };

        _imageRepositoryMock.Setup(r => r.GetImageByAccountIdAsync(accountId)).ReturnsAsync(imageEntity);

        var result = await _imageService.GetImageByAccountIdAsync(accountId);

        Assert.IsNotNull(result);
        Assert.AreEqual(accountId, result.AccountId);
        Assert.AreEqual("TestImage", result.Name);
    }

    [Test]
    public async Task GetImageByAccountIdAsync_ImageNotFound_ReturnsNull()
    {
        var accountId = Guid.NewGuid();
        _imageRepositoryMock.Setup(r => r.GetImageByAccountIdAsync(accountId)).ReturnsAsync((Image)null);

        var result = await _imageService.GetImageByAccountIdAsync(accountId);

        Assert.IsNull(result);
    }
}
