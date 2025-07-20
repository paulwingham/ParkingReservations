using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Dto.DomainSettings;

namespace Paul.ParkingReservations.Api.Controllers.V1_0;

public class LoginController : BaseApiController
{
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginService _loginService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ParkingReservationsAppConfigSettings _parkingReservationsAppConfigSettings;

    public LoginController(ILogger<LoginController> logger, ILoginService loginService, IJwtTokenGenerator jwtTokenGenerator, IOptions<ParkingReservationsAppConfigSettings> parkingReservationsAppConfigSettings)
    {
        _logger = logger;
        _loginService = loginService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _parkingReservationsAppConfigSettings = parkingReservationsAppConfigSettings.Value;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token and user details.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The password of the user attempting to log in.</param>
    /// <returns>Returns a JWT token and user details if authentication is successful.</returns>
    /// <response code="200">Returns the JWT token and user details.</response>
    /// <response code="400">If the username or password is empty or authentication fails.</response>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Login(string username, string password)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            _logger.LogWarning($"{CurrentFunctionMethod.GetCaller(this)} - Username or password is empty");
            return BadRequest(new { error = "Username or password cannot be empty."});
        }

        try
        {
            var userDto = await _loginService.LoginAsync();
            if (userDto == null)
            {
                return BadRequest(new { error = "Login failed" });
            }

            var token = await _jwtTokenGenerator.GenerateToken(userDto.Contact.ContactId);
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError($"{CurrentFunctionMethod.GetCaller(this)} - Token generation failed for user with ContactId:" + userDto.Contact.ContactId);
                return BadRequest("Token generation failed.");
            }

            _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - User logged in with ContactId:" + userDto.Contact.ContactId);
            return Ok(new
            {
                Token = token,
                User = userDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{CurrentFunctionMethod.GetCaller(this)} - Error occurred while login");
            return BadRequest(new { error = ex.Message });
        }
    }
}