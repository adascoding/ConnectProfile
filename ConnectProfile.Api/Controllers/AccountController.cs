using ConnectProfile.Api.Dtos.Account;
using ConnectProfile.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConnectProfile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(ILogger<AccountController> logger, IAccountService accountService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterRequestDto dto)
    {
        logger.LogInformation($"Creating account for {dto.UserName}");
        var result = await accountService.RegisterAsync(dto);

        if (!result)
        {
            return BadRequest("Account creation failed.");
        }

        return Created("", new { message = "Account created successfully." });
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        logger.LogInformation($"Attempting login for {dto.UserName}");

        var response = await accountService.LoginAsync(dto);

        if (response == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(response);
    }
}
