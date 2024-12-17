using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Dtos.UserInfo;

namespace ConnectProfile.Api.Validators.Interfaces;

public interface IValidationService
{
    Task<string?> ValidateRegisterAsync(RegisterRequestDto dto);
    string? ValidateLoginAsync(LoginRequestDto dto);
    string? ValidateImageUpload(IFormFile file, string name, string description);
    string? ValidateUserInfo(UserInfoCreateDto dto);
    public string? ValidateField(string fieldName, string fieldValue);
}
