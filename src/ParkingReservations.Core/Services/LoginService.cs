using Microsoft.Extensions.Logging;
using Paul.ParkingReservations.Dto;
using Paul.ParkingReservations.Infrastructure.SqlLite;

namespace Paul.ParkingReservations.Core.Services;

public class LoginService : ILoginService
{
    private readonly ILogger<LoginService> _logger;
    private readonly IDapperProvider _dapperProvider;

    public LoginService(ILogger<LoginService> logger, IDapperProvider dapperProvider)
    {
        _logger = logger;
        _dapperProvider = dapperProvider;
    }

    public async Task<UserDto> LoginAsync()
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        var userDto = await _dapperProvider.GetUserDetails();

        return userDto;
    }

    public async Task<ContactDto> GetContactByIdAsync(int id)
    {
        _logger.LogInformation($"{CurrentFunctionMethod.GetCaller(this)} - Started");

        var contactDto = await _dapperProvider.GetContactById(id);

        return contactDto;
    }
}