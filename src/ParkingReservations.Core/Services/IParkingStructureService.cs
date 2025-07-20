using Paul.ParkingReservations.Dto;

namespace Paul.ParkingReservations.Core.Services;

public interface IParkingStructureService
{
    Task<IEnumerable<ParkingStructureDto>> GetAllAsync();
}