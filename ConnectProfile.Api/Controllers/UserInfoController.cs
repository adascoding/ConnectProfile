using System.Security.Claims;
using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.UserInfo;
using ConnectProfile.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectProfile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserInfoController : ControllerBase
{
    private readonly ILogger<UserInfoController> _logger;
    private readonly IUserInfoService _userInfoService;
    private readonly Guid _userId;

    public UserInfoController(
        ILogger<UserInfoController> logger,
        IUserInfoService userInfoService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userInfoService = userInfoService;

        var userIdValue = httpContextAccessor.HttpContext?.User.FindFirstValue("Id");
        if (string.IsNullOrEmpty(userIdValue))
        {
            _logger.LogError("User ID not found in the request.");
            throw new UnauthorizedAccessException("User not found.");
        }

        _userId = new Guid(userIdValue);
        _logger.LogInformation("UserInfoController initialized for UserId: {UserId}", _userId);
    }

    [HttpGet("{accountId}")]
    [ProducesResponseType(typeof(Response<UserInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Response<UserInfoDto>>> GetUserInfo(Guid accountId)
    {
        _logger.LogInformation("GET request received for user info. AccountId: {AccountId}, UserId: {UserId}", accountId, _userId);

        if (accountId != _userId)
        {
            _logger.LogWarning("Forbidden request: AccountId {AccountId} does not match UserId {UserId}", accountId, _userId);
            return Forbid();
        }

        var response = await _userInfoService.GetUserInfoAsync(accountId);
        _logger.LogInformation("Fetched user info for AccountId: {AccountId}. Success: {Success}", accountId, response.Success);

        if (!response.Success)
        {
            _logger.LogWarning("User info not found for AccountId: {AccountId}", accountId);
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost("{accountId}")]
    [ProducesResponseType(typeof(Response<UserInfoDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Response<UserInfoDto>>> CreateUserInfo([FromBody] UserInfoCreateDto dto)
    {
        _logger.LogInformation("POST request received to create user info. AccountId: {AccountId}", dto.AccountId);

        if (dto.AccountId != _userId)
        {
            _logger.LogWarning("Forbidden request: Provided AccountId {AccountId} does not match UserId {UserId}", dto.AccountId, _userId);
            return Forbid();
        }

        var response = await _userInfoService.CreateUserInfoAsync(dto);
        _logger.LogInformation("User info creation attempt for AccountId: {AccountId}. Success: {Success}", dto.AccountId, response.Success);

        if (!response.Success)
        {
            _logger.LogWarning("Failed to create user info for AccountId: {AccountId}. Error: {Message}", dto.AccountId, response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("User info successfully created for AccountId: {AccountId}", dto.AccountId);
        return CreatedAtAction(nameof(GetUserInfo), new { accountId = _userId }, response);
    }

    [HttpPatch("{accountId}/UpdateField")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Response<bool>>> UpdateField(Guid accountId, [FromBody] UpdateFieldRequestDto dto)
    {
        _logger.LogInformation("PATCH request received to update field for AccountId: {AccountId}. Field: {FieldName}", accountId, dto.FieldName);

        if (accountId != _userId)
        {
            _logger.LogWarning("Forbidden request: AccountId {AccountId} does not match UserId {UserId}", accountId, _userId);
            return Forbid();
        }

        var response = await _userInfoService.UpdateFieldAsync(accountId, dto);
        _logger.LogInformation("Field update attempt for AccountId: {AccountId}. Field: {FieldName}. Success: {Success}", accountId, dto.FieldName, response.Success);

        if (!response.Success)
        {
            _logger.LogWarning("Failed to update field for AccountId: {AccountId}. Field: {FieldName}. Error: {Message}", accountId, dto.FieldName, response.Message);
            return BadRequest(response);
        }

        _logger.LogInformation("Successfully updated field for AccountId: {AccountId}. Field: {FieldName}", accountId, dto.FieldName);
        return Ok(response);
    }
}
