namespace Paul.ParkingReservations.Core.Services;

public interface IJwtTokenGenerator
{
    Task<string> GenerateToken(int contactId);
}