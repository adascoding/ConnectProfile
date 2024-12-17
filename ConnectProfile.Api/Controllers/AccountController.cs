using ConnectProfile.Api.Dtos;
using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectProfile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpPost("Register")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Response<bool>>> Register(RegisterRequestDto dto)
    {
        _logger.LogInformation($"Creating account for {dto.UserName}");
        var result = await _accountService.RegisterAsync(dto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(Response<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Response<LoginResponseDto>>> Login(LoginRequestDto dto)
    {
        _logger.LogInformation($"Attempting login for {dto.UserName}");

        var result = await _accountService.LoginAsync(dto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        _logger.LogInformation($"Admin attempting to delete account with ID: {accountId}");

        var result = await _accountService.DeleteAccountAsync(accountId);

        if (!result.Success)
        {
            _logger.LogWarning($"Failed to delete account with ID: {accountId}");
            return NotFound(result);
        }

        _logger.LogInformation($"Successfully deleted account with ID: {accountId}");
        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    [ProducesResponseType(typeof(Response<IEnumerable<Account>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("Admin attempting to fetch all users");

        var result = await _accountService.GetAllUsersAsync();

        if (!result.Success)
        {
            _logger.LogWarning("Failed to fetch users");
            return NotFound(result);
        }

        _logger.LogInformation("Successfully fetched all users");
        return Ok(result);
    }
}
