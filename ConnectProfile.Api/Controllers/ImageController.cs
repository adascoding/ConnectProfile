using ConnectProfile.Api.Dtos.Image;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectProfile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImageController(IImageService imageService, ILogger<ImageController> logger) : ControllerBase
{

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await request.File.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            var imageEntity = new Image
            {
                Name = request.Name,
                Description = request.Description,
                ImageBytes = imageBytes,
                AccountId = request.AccountId
            };

            await imageService.AddImageAsync(imageEntity);

            return CreatedAtAction(
                nameof(GetImageByAccountId),
                new { accountId = imageEntity.AccountId },
                new { message = "Image uploaded successfully", accountId = imageEntity.AccountId }
            );
        }
        catch (Exception ex)
        {
            logger.LogError($"Error uploading image: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }



    [HttpGet("{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImageByAccountId(Guid accountId)
    {
        var imageEntity = await imageService.GetImageByAccountIdAsync(accountId);
        if (imageEntity == null)
        {
            return NotFound("Image not found for this account.");
        }

        return File(imageEntity.ImageBytes, "image/jpeg");
    }
}
