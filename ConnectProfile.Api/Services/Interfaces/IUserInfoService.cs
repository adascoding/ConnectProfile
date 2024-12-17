using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.UserInfo;

namespace ConnectProfile.Api.Services.Interfaces;

public interface IUserInfoService
{
    Task<Response<UserInfoDto>> GetUserInfoAsync(Guid accountId);
    Task<Response<UserInfoDto>> CreateUserInfoAsync(UserInfoCreateDto userInfoCreateDto);
    Task<Response<bool>> UpdateFieldAsync(Guid accountId, UpdateFieldRequestDto dto);
}
