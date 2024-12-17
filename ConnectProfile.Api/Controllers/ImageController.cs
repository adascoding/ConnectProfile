using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.Image;
using ConnectProfile.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConnectProfile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImageController> _logger;
    private readonly Guid _userId;

    public ImageController(IImageService imageService, ILogger<ImageController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _imageService = imageService;
        _logger = logger;

        var userIdValue = httpContextAccessor.HttpContext?.User.FindFirstValue("Id");
        if (string.IsNullOrEmpty(userIdValue))
            throw new UnauthorizedAccessException("User not found.");

        _userId = new Guid(userIdValue);
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto request)
    {
        if (request.AccountId != _userId)
        {
            _logger.LogWarning("Account ID mismatch: UserId {UserId} is trying to upload an image for AccountId {AccountId}", _userId, request.AccountId);
            return Forbid();
        }

        _logger.LogInformation("Received image upload request for account ID {AccountId}", request.AccountId);

        var response = await _imageService.UploadImageAsync(request.AccountId, request.File, request.Name, request.Description);

        if (!response.Success)
        {
            _logger.LogWarning("Image upload failed for account ID {AccountId}: {ErrorMessage}", request.AccountId, response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("Image uploaded successfully for account ID {AccountId}", request.AccountId);
        return CreatedAtAction(
            nameof(GetImageByAccountId),
            new { accountId = request.AccountId },
            response
        );
    }

    [HttpGet("{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetImageByAccountId(Guid accountId)
    {
        if (accountId != _userId)
        {
            _logger.LogWarning("Account ID mismatch: UserId {UserId} is trying to access image for AccountId {AccountId}", _userId, accountId);
            return Forbid();
        }

        _logger.LogInformation("Received request to retrieve image for account ID {AccountId}", accountId);

        var imageEntity = await _imageService.GetImageByAccountIdAsync(accountId);
        if (imageEntity == null)
        {
            _logger.LogWarning("No image found for account ID {AccountId}", accountId);
            return NotFound(new Response<object> { Success = false, Message = "Image not found for this account." });
        }

        _logger.LogInformation("Returning image for account ID {AccountId}", accountId);
        return File(imageEntity.ImageBytes, "image/jpeg");
    }
}
