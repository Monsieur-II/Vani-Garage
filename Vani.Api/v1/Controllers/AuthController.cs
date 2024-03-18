using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Vani.Domain.Models;
using Vani.Services;
using Vani.Services.Auth;
using Vani.Shared;
using Vani.Shared.DTOS.Auth;
using Vani.Shared.Exceptions;

namespace Vani.Api.v1.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JWTConfig _jwtConfig;
    private readonly EmailConfig _emailConfig;

    public AuthController(
        UserManager<AppUser> userManager,
        IOptions<JWTConfig> jwtConfig,
        IOptions<EmailConfig> emailConfig
    )
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig.Value;
        _emailConfig = emailConfig.Value;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> Login([FromBody] LoginModel? model)
    {
        if (model == null)
            return BadRequest(
                new Response()
                {
                    Message = "Invalid data request",
                    status = "error",
                    Success = false
                }
            );

        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return BadRequest(
                new Response
                {
                    Message = "Username or password is incorrect",
                    status = "error",
                    Success = false
                }
            );
        }

        if (!user.EmailConfirmed)
            return Unauthorized(
                new Response()
                {
                    Message = "Email not confirmed",
                    status = "error",
                    Success = false
                }
            );

        user.LastLogin = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await TokenService.GenerateTokenAsync(user, _userManager, _jwtConfig);

        return Ok(new LoginResponse() { Username = model.Username, Token = token });
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> Register(RegisterUserModel? model)
    {
        if (model == null || model.Password != model.ConfirmPassword || await _userManager.FindByNameAsync(model.Email) != null)
        {
            return BadRequest(new Response {
                    Message = model == null ? "Invalid data request"
                            : model.Password != model.ConfirmPassword ? "Passwords do not match"
                            : "Username already exists",
                    status = "error",
                    Success = false }
            );
        }

        var user = new AppUser
        {
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            DateCreated = DateTime.UtcNow
        };
        
        
        var result = await _userManager.CreateAsync(user, model.Password);
        await _userManager.AddToRoleAsync(user, VaniRoles.User_User);
        
        if (!result.Succeeded)
        {
            return BadRequest(
                new Response()
                {
                    Message = "Error while registering user",
                    status = "error",
                    Success = false
                }
            );
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailBody =
            "Please confirm you email address: <a href=\"#URL#\"> Click here to confirm your email address</a>";
        var callbackUrl =
            Request.Scheme
            + "://"
            + Request.Host
            + Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code });

        var body = emailBody.Replace(
            "#URL#",
            System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callbackUrl)
        );

        BackgroundJob.Enqueue(() => EmailSender.SendEmail(user.Email, body, _emailConfig));
        
        return Ok();
    }

    [HttpPost("ConfirmEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiException))]
    public async Task<IActionResult> ConfirmEmail(string? userId, string? userCode)
    {
        if (userId == null || userCode == null)
            return BadRequest("User cannot be identified");

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return BadRequest();

        var code = Encoding.UTF8.GetString(Convert.FromBase64String(userCode));

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
            return Ok("Email has been successfully confirmed");

        return BadRequest("Error while confirming Email");
    }
}
