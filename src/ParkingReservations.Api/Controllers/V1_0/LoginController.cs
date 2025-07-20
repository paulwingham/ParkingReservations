using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Paul.ParkingReservations.Core.Services;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Dto.DomainSettings;

namespace Paul.ParkingReservations.Api.Controllers.V1_0;

[Route("api/[controller]")]
[ApiVersion("1.0")]
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

    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Login(string username, string password)
    {
        _logger.LogDebug($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        try
        {
            var userDto = await _loginService.LoginAsync();
            if (userDto == null)
            {
                return BadRequest();
            }

            var token = _jwtTokenGenerator.GenerateToken(userDto.Contact.ContactId);

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
            return BadRequest();
        }
    }
}