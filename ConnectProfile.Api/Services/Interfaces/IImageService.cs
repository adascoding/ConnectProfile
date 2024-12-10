using ConnectProfile.Api.Entities;

namespace ConnectProfile.Api.Services.Interfaces;

public interface IImageService
{
    Task AddImageAsync(Image image);
    Task<Image?> GetImageByAccountIdAsync(Guid accountId);
}
