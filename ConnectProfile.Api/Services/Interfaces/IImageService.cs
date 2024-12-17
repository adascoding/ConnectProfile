using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Services.Interfaces;

public interface IImageService
{
    Task<Response<object>> UploadImageAsync(Guid accountId, IFormFile file, string name, string description);
    Task<Image?> GetImageByAccountIdAsync(Guid accountId);
}
