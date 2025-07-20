using Paul.ParkingReservations.Dto;

namespace Paul.ParkingReservations.Core.Services;

public interface ILoginService
{
    Task<UserDto> LoginAsync();
}