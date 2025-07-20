using Paul.ParkingReservations.Dto;

namespace Paul.ParkingReservations.Infrastructure.SqlLite;

public interface IDapperProvider
{
    ////Task<List<ParkingSlotsDto>> GetAllAvailableParkingSlotsByDateRange(DateTime startDate, DateTime endDate);

    Task<List<ParkingSlotsDto>> GetBookedParkingSlotsByDateRange(DateTime startDate, DateTime endDate);

    Task<IEnumerable<ParkingStructureDto>> GetAllParkingStructures();

    Task<IEnumerable<ParkingSpaceDto>> GetAllParkingSpaces();

    Task<int> InsertBookingAsync(BookingDto bookingDto);

    Task<int> DeleteBookingByIdAsync(int id);

    Task<UserDto> GetUserDetails();

    Task<ContactDto> GetContactById(int id);
}